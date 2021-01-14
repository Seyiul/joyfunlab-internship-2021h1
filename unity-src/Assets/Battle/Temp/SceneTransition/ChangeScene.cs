using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ChangeScene : MonoBehaviour
{
    public int sceneIndex = 1;
    Animator animator;
    void Start()
    {
        animator = transform.GetComponent<Animator>();
    }
    void Update()
    {
        //change scene when user presses Space key
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(LoadSceneAFterTransition());
        }
    }
    private IEnumerator LoadSceneAFterTransition()
    {
        //show animate out animation
        animator.SetBool("animateOut", true);
        yield return new WaitForSeconds(1f);
        //load the scene we want
        SceneManager.LoadScene(sceneIndex);
    }
}

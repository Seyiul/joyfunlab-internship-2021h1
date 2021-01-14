using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BackToGame : MonoBehaviour
{
    //TODO: 전환될 화면 인덱스
    public int sceneIndex;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetComponent<Animator>();
    }
    void Update()
    {
        //change scene when user presses Space key
        if (HealthBarHandler.GetHealthBarValue() == 0 || PlayerHealthbarHandler.GetHealthBarValue() ==0)
        {
            StartCoroutine(LoadSceneAFterTransition());
        }
    }
    private IEnumerator LoadSceneAFterTransition()
    {
        yield return new WaitForSeconds(5f);
        animator.SetBool("animateOut", true);
        GameManager.instance.SetGameState(GameState.Game);
        //SceneManager.LoadScene(sceneIndex);
    }
  
}

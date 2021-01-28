using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BackToGame : MonoBehaviour
{
    //TODO: 전환될 화면 인덱스
    public int sceneIndex;
    Animator animator;
    public float time;
    public GameObject Nodes;
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
            Nodes.SetActive(false);
            time = PlayerPrefs.GetFloat("time");
            time += PlayerHealthbarHandler.GetHealthBarValue();
            PlayerPrefs.SetFloat("time", time);
            StartCoroutine(LoadSceneAFterTransition());
        }
    }
    private IEnumerator LoadSceneAFterTransition()
    {
        yield return new WaitForSeconds(5f);
        animator.SetBool("animateOut", true);
        StartCoroutine(SceneChage());
    }
    private IEnumerator SceneChage()
    {
        yield return new WaitForSeconds(1f);
        time = PlayerPrefs.GetFloat("time");
        Debug.Log(time);
        time += PlayerHealthbarHandler.GetHealthBarValue();
        Debug.Log(time);
        PlayerPrefs.SetFloat("time", time);

        GameManager.instance.SetGameState(GameState.Game);
        SceneManager.LoadScene(sceneIndex);

    }
  
}

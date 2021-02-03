﻿using System.Collections;
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

    public bool pause;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.SetGameState(GameState.Battle);
        animator = transform.GetComponent<Animator>();

        pause = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || pause == true)
        {
            if (GameManager.instance.GetGameState() == GameState.Battle)
            {
                Time.timeScale = 0;
                GameManager.instance.SetGameState(GameState.Pause);
            }
            else
            {
                Time.timeScale = 1;
                GameManager.instance.SetGameState(GameState.Battle);
            }

            pause = false;
        }
        //change scene when user presses Space key
        if (HealthBarHandler.GetHealthBarValue() == 0)
        {
            Nodes.SetActive(false);
            StartCoroutine(LoadSceneAFterTransition());
        }
        if (PlayerHealthbarHandler.GetHealthBarValue() == 0)
        {
            Nodes.SetActive(false);
            GameManager.instance.SetGameState(GameState.Result);
            GameManager.instance.SetStateChanged(true);
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
        if (PlayerHealthbarHandler.GetHealthBarValue() > 0)
        {
            float plusTime = PlayerHealthbarHandler.GetHealthBarValue() * 20;
            time = PlayerPrefs.GetFloat("time");
            PlayerPrefs.SetFloat("time", time + plusTime);
            PlayerPrefs.SetFloat("playtime", PlayerPrefs.GetFloat("playtime") + plusTime);
        }
        else
        {
            float leftTime = PlayerPrefs.GetFloat("time");
            PlayerPrefs.SetFloat("time", 0);
            PlayerPrefs.SetFloat("playtime", PlayerPrefs.GetFloat("playtime") - leftTime);
        }
        GameManager.instance.SetGameState(GameState.Game);
        SceneManager.LoadScene(sceneIndex);
    }

}

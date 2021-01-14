using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterMV : MonoBehaviour
{
    //public int startDelay =3;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(DelayedAnimation());

    }
    // The delay coroutine
    IEnumerator DelayedAnimation()
    {
        yield return new WaitForSeconds(10f);
        anim.Play("landing");
    }
}

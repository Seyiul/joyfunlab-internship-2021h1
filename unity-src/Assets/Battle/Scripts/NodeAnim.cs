using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeAnim : MonoBehaviour
{
    public GameObject leftKick;
    public GameObject rightKick;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartLeftKick());
        StartCoroutine(StartRightKick());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator StartLeftKick()
    {
        yield return new WaitForSeconds(1f);
        leftKick.GetComponent<Animator>().SetTrigger("start");

    }
    IEnumerator StartRightKick()
    {
        yield return new WaitForSeconds(1f);
        rightKick.GetComponent<Animator>().SetTrigger("start");

    }
}

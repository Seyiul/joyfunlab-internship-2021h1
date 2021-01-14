using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("die", false);
        animator.SetBool("victory", false);
    }
 
    // Update is called once per frame
    void Update()
    {
        if(HealthBarHandler.GetHealthBarValue()==0)
        {
            animator.SetBool("die", true);

        }
        else
        {
            if(PlayerHealthbarHandler.GetHealthBarValue() == 0)
            {
                animator.SetBool("victory", true);
            }
        }
    }

}

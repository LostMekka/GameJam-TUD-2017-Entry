using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimations : MonoBehaviour {
    public float moveHorizontal;
    public float moveVertical;
    public float axisInputH;
    public float axisInputV;


    private Animator animator;

    // Use this for initialization
    void Start () {

        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update () {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        axisInputH = Input.GetAxis("Horizontal");
        axisInputV = Input.GetAxis("Vertical");
        
        if (moveHorizontal == 1f)
        {
            animator.SetTrigger("unitWalk");
        }
        else if (moveHorizontal == -1f)
        {
            animator.SetTrigger("unitIdle");
        }


        if (moveVertical == 1f)
        {
            animator.SetTrigger("unitAttack01");
        }
        else if (moveVertical == -1f)
        {
            animator.SetTrigger("unitAttack02");
        }




    }
}

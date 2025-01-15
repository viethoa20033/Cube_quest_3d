using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;
    public AnimState animState;

    
    //-------------------//
    
    [Header("Movement")]
    public float moveSpeed;
    public Vector3 directionMove;
    public bool isSetMove;
    
    //--------------------//
    
    [Header("Check to Move")]
    public bool isWall;
    public bool isBackWood;
    public bool isCheckMove;
    
    //--------------------//

    [Header("Next point")]
    public Vector3 nextMove;
    public Vector3 nextPush;

    //-------------------//
    
    [Header("Wood")] 
    public GameObject targetWood;
    
    [Header("Layer Mask")]
    public LayerMask walllayer;
    public LayerMask woodLayer;
    public LayerMask checkBackWoodLayer;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    public void FixedUpdate()
    {
        if (GameManager.Instance.isPlaying)
        {
            SetTargetMove(); //set direction move player

            Moving(); //player moving

            CheckWood();
        }
        

        //Change animation player 
        ChangeAnimationState();
        
    }

    public void SetTargetMove()
    {
        if (!isSetMove && directionMove != Vector3.zero)
        {
            isSetMove = true;
            isCheckMove = true;
            nextMove = transform.position + directionMove;
            nextPush = transform.position + 2 * directionMove;
        }
    }
    void Moving()
    {
        if (isCheckMove)
        {
            RayCheck();
            
            if (isWall || isBackWood)
            {
                targetWood = null;
                isSetMove = false;
                directionMove = rb.velocity = Vector3.zero;
            }

            if (!isWall && !isBackWood && isSetMove)
            {
                isCheckMove = false;
            }
        }
        else
        {
            //move
            rb.velocity = directionMove * moveSpeed;
            //check disntance => set
            if (Vector3.Distance(transform.position, nextMove) < .1f)
            {
                transform.position = nextMove;
                isSetMove = false;
                directionMove = rb.velocity = Vector3.zero;
                isCheckMove = true;
                //add cout leg move
                GameController.Instance.UpdateLeg();

                if (targetWood != null)
                {
                    targetWood.transform.position =
                        new Vector3(nextPush.x, targetWood.transform.position.y, nextPush.z);
                    targetWood.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    targetWood = null;
                    
                    
                }

            }
            if (targetWood != null)
            {
                targetWood.GetComponent<Rigidbody>().velocity = directionMove * 1.1f * moveSpeed;
            }
        }
    }

    void RayCheck()
    {
        isWall = Physics.Raycast(transform.position, transform.forward, .75f, walllayer);

        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, 2.25f, checkBackWoodLayer);
        if (hits.Length > 1)
        {
            isBackWood = true;
            
        }
        else
        {
            isBackWood = false;
        }
    }

    void CheckWood()
    {
        if (isSetMove)
        {
            Physics.Raycast(transform.position, directionMove, out RaycastHit hit, .55f, woodLayer);
            if (hit.collider != null)
            {
                targetWood = hit.collider.gameObject;
            }
        }
    }

    void ChangeAnimationState()
    {
        anim.SetInteger("state", (int)animState);

        if (GameManager.Instance.isPlaying)
        {

            if (targetWood == null)
            {
                if (isSetMove)
                {
                    animState = AnimState.run;
                }
                else
                {
                    animState = AnimState.idle;
                }
            }
            else
            {
                if (isSetMove)
                {
                    animState = AnimState.push;
                }
                else
                {
                    animState = AnimState.idle;
                }
            }
        }
        else
        {
            animState = AnimState.win;
        }
    }
}

public enum AnimState
{
    idle,
    run,
    push,
    win
}

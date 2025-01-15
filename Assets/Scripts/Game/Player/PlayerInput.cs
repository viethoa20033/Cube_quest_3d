using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void MoveLeft()
    {
        if (playerController.isSetMove) return;
        
        playerController.directionMove = Vector3.left;
        transform.rotation = Quaternion.Euler(0, -90, 0);

    }
    public void MoveRight()
    {
        if (playerController.isSetMove) return;
        
        playerController.directionMove = Vector3.right;
        transform.rotation = Quaternion.Euler(0,90,0);

    }
    public void MoveUp()
    {
        if (playerController.isSetMove) return;
        
        playerController.directionMove = Vector3.forward;
        transform.rotation = Quaternion.Euler(0,0,0);

    }
    public void MoveDown()
    {
        if (playerController.isSetMove) return;
        
        playerController.directionMove = -Vector3.forward;
        transform.rotation = Quaternion.Euler(0,180,0);

    }
    
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Transform playerPos;

    public int target;
    private void Start()
    {
        FindObjectOfType<PlayerController>().transform.position = playerPos.position;

        GameController.Instance.coutTarget = target;
    }
    
}

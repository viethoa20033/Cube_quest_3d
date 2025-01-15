using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LoadWall : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 endPos;

    private float speedMove;

    private void Start()
    {
        endPos = transform.position;
        
        
        int randomY = Random.Range(5, 10);
        transform.position += new Vector3(0, randomY, 0);

        speedMove = Random.Range(6f, 10f);
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, endPos, Time.deltaTime * speedMove);

        if (Vector3.Distance(transform.position, endPos) < .1f)
        {
            transform.position = endPos;
        }
    }
}

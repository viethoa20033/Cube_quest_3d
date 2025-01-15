using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float speedMove;
    public Vector3 offSetGame;
    public Vector3 offSetWin;

    private void LateUpdate()
    {
        if (target != null)
        {
            if (GameManager.Instance.isPlaying)
            {
                transform.position =
                    Vector3.Lerp(transform.position, target.position + offSetGame, Time.deltaTime * speedMove);
            }
            else
            {
                transform.position =
                    Vector3.Lerp(transform.position, target.position + offSetWin, Time.deltaTime * speedMove);
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : SingletonBase<GameController>
{
    public static UnityAction<int> OnCoutLegChanged;
    public static UnityAction<int> OnCoutTargetChanged;
    
    
    public int coutTarget;
    public int coutLeg;

    public int boxSet;
    public Wood[] woodCubes;

    public void UpdateLeg()
    {
        coutLeg++;
        OnCoutLegChanged?.Invoke(coutLeg);
    }

    //reset map => reset leg
    public void ResetCoutLeg()
    {
        coutLeg = 0;
        OnCoutLegChanged?.Invoke(coutLeg);
    }

    public void UpdateBoxSet(int _box)
    {
        boxSet = _box;
        OnCoutTargetChanged?.Invoke(boxSet);
    }

    private void Update()
    {
        if (GameManager.Instance.isPlaying)
        {
            woodCubes = FindObjectsOfType<Wood>();
            if (woodCubes.Length > 0)
            { 
               int _boxSet = 0;
                foreach (var wood in woodCubes)
                {
                    if (wood.isTarget)
                    {
                        _boxSet++;
                        if (boxSet >= coutTarget)
                        {
                            GameManager.Instance.UpdateGameState(GameState.GameSuccess);
                        }
                    }
                }

                UpdateBoxSet(_boxSet);
            }
        }
    }
}

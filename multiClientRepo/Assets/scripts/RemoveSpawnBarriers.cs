using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveSpawnBarriers : MonoBehaviour
{
    public GameObject[] walls;
    public void RemoveSpawnWalls()
    {
        foreach(GameObject wall in walls)
        {
            wall.GetComponent<SpawnEffectWall>().StartAnimation();
        }
    }

    public void RebuildSpawnWalls()
    {
        foreach (GameObject wall in walls)
        {
            wall.GetComponent<SpawnEffectWall>().AnimBackToVisible();
        }
    }
}

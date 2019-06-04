using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<Level> levels = new List<Level>();
    public CharacterController player;

    private void FixedUpdate()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].transform.position = new Vector3(0, i * -LevelChunk.size.y, 0);
        }
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private List<Level> levels = new List<Level>();
    public CharacterController characterController;

    public int CurrentLevel
    {
        get
        {
            return Mathf.FloorToInt(characterController.transform.position.y / LevelChunk.size.y);
        }
    }

    private void Start()
    {
        levels = new List<Level>(GetComponentsInChildren<Level>());
        for (int i = 0; i < levels.Count; i++)
        {
            foreach (var tileRenderer in levels[i].GetComponentsInChildren<UnityEngine.Tilemaps.TilemapRenderer>())
            {
                tileRenderer.sortingOrder = i;
            }
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].transform.position = new Vector3(0, i * -LevelChunk.size.y, 0);
        }

        //Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, levels[-CurrentLevel].backgroundColor, .1f);

    }



}

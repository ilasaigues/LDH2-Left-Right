using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Level : MonoBehaviour
{
    public List<LevelChunk> chunks = new List<LevelChunk>();
    public Color tilemapColor = Color.white;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Tilemap tilemapRenderer in GetComponentsInChildren<Tilemap>())
        {
            tilemapRenderer.color = tilemapColor;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int levelWidth = LevelChunk.size.x * chunks.Count;
        for (int i = 0; i < chunks.Count; i++)
        {
            Vector3 camPos = Camera.main.transform.position;
            float horizontalOffset = (camPos.x - LevelChunk.size.x * (i - 0.5f)) - Modulo((camPos.x - LevelChunk.size.x * (i - 0.5f)), levelWidth);
            chunks[i].transform.localPosition = new Vector3(horizontalOffset + i * LevelChunk.size.x, 0);
        }
    }


    float Modulo(float a, float b)
    {
        return a - b * Mathf.Floor(a / b);
    }
}

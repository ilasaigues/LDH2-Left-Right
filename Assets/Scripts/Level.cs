using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Level : MonoBehaviour
{
    public Color tilemapColor = Color.white;
    [UnityEngine.Serialization.FormerlySerializedAs("BackgroundColor")]
    public Color hazardColor = Color.white;

    private List<LevelChunk> chunks = new List<LevelChunk>();
    // Start is called before the first frame update
    void Start()
    {
        chunks = new List<LevelChunk>(GetComponentsInChildren<LevelChunk>());
        foreach (Tilemap tilemapRenderer in GetComponentsInChildren<Tilemap>())
        {
            tilemapRenderer.color = tilemapColor;
        }

        foreach (var spike in GetComponentsInChildren<Spike>())
        {
            spike.GetComponent<Tilemap>().color = hazardColor;
        }

        foreach (var enemy in GetComponentsInChildren<Enemy>())
        {
            foreach (var renderer in enemy.GetComponentsInChildren<SpriteRenderer>())
            {
                renderer.color = hazardColor;
            }
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

    private void OnDrawGizmos()
    {
        Start();
    }
}

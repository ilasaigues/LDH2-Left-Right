using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChunk : MonoBehaviour
{
    public static Vector2Int size = new Vector2Int(32, 16);

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Gizmos.DrawWireCube(transform.position + new Vector3(x-.5f, y, 0), Vector3.one);
            }
        }
    }
}

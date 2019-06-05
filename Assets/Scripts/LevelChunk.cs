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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 centerPos = new Vector3(size.x / 2 - 1, size.y / 2 - .5f, 0);
        Gizmos.DrawWireCube(transform.position + centerPos, (Vector2)size);
    }
}

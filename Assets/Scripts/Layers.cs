using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layers
{
    public static LayerMask MASK_PLAYER = LayerMask.GetMask("Player");
    public static LayerMask MASK_FLOOR = LayerMask.GetMask("Floor");
    public static LayerMask MASK_PLATFORM = LayerMask.GetMask("Platform");
}

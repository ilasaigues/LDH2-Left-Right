﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Manager : MonoBehaviour
{
    public virtual void Awake()
    {
        Director.RegisterManager(this);
    }
}

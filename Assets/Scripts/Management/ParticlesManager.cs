using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesManager : Manager
{
    public ParticleSystem SpawnParticles(ParticleSystem system, Vector3 position, float destroyTime = 0)
    {
        Debug.Log(destroyTime);
        if (system == null) return null;
        ParticleSystem instance = Instantiate(system, position, Quaternion.identity);
        if (destroyTime != 0)
        {
            Destroy(instance.gameObject, destroyTime);
        }
        return instance;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : Manager
{
    public ParticleSystemReference respawnParticles;

    private Vector3 _lastRespawnPosition;
    private ParticleSystem _respawnParticlesInstance;
    public void SetCurrentRespawnPoint(RespawnPoint point)
    {
        _lastRespawnPosition = point.transform.position;
        if (_respawnParticlesInstance == null) _respawnParticlesInstance = Director.GetManager<ParticlesManager>().SpawnParticles(respawnParticles.Value, point.transform.position);
        _respawnParticlesInstance.transform.position = point.transform.position;
    }

    public Vector3 GetRespawnPosition()
    {
        return _lastRespawnPosition;
    }
}

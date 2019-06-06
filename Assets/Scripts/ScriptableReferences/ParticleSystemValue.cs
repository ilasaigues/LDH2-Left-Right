using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
[CreateAssetMenu(fileName = "ParticleValue", menuName = "Scriptables/Values/Particle")]
public class ParticleSystemValue : ScriptableValue<ParticleSystem> { }
[Serializable]
public class ParticleSystemReference : ScriptableReference<ParticleSystem, ParticleSystemValue> { }
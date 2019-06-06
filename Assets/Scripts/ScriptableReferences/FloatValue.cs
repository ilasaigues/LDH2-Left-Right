using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
[CreateAssetMenu(fileName = "FloatValue", menuName = "Scriptables/Values/Float")]
public class FloatValue : ScriptableValue<float> { }
[Serializable]
public class FloatReference : ScriptableReference<float, FloatValue> { }

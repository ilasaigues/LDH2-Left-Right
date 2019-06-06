using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScriptableReferenceType
{
    Local,
    Reference
}

public abstract class ScriptableReference<T, RT> where RT : ScriptableValue<T>
{
    [SerializeField]
    public ScriptableReferenceType refType;
    [SerializeField]
    public RT reference;
    [SerializeField]
    public T localValue;

    public T Value
    {
        get
        {
            if (!reference || refType == ScriptableReferenceType.Local) return localValue;
            return reference.value;
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ParticleSystemReference))]
[CustomPropertyDrawer(typeof(SoundReference))]
[CustomPropertyDrawer(typeof(FloatReference))]
public class ScriptableReferenceDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        //base.OnGUI(position, property, label);
        Rect enumRect = new Rect(position.x, position.y, position.height, position.height);
        Rect valueRect = new Rect(position.x + position.height, position.y, position.width - position.height, position.height);

        EditorGUI.PropertyField(enumRect, property.FindPropertyRelative("refType"), GUIContent.none);
        if (property.FindPropertyRelative("refType").enumValueIndex == (int)ScriptableReferenceType.Local)
        {
            EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("localValue"), GUIContent.none);
        }
        else
        {
            EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("reference"), GUIContent.none);
        }
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}

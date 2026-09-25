using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Transform))]
public class TransformWorldInspector : Editor
{
    public override void OnInspectorGUI()
    {
        Transform t = (Transform)target;

        EditorGUI.BeginChangeCheck();

        Vector3 localPosition = EditorGUILayout.Vector3Field("Position", t.localPosition);
        Vector3 localRotation = EditorGUILayout.Vector3Field("Rotation", t.localEulerAngles);
        Vector3 localScale = EditorGUILayout.Vector3Field("Scale", t.localScale);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(t, "Transform Change");

            t.localPosition = localPosition;
            t.localEulerAngles = localRotation;
            t.localScale = localScale;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("World Transform", EditorStyles.boldLabel);

        Vector3 worldPos = RoundVector3(t.position, 3);
        Vector3 worldRot = RoundVector3(t.eulerAngles, 3);
        Vector3 worldScale = RoundVector3(t.lossyScale, 3);

        EditorGUILayout.Vector3Field("Position", worldPos);
        EditorGUILayout.Vector3Field("Rotation", worldRot);
        EditorGUILayout.Vector3Field("Scale", worldScale);
    }

    private Vector3 RoundVector3(Vector3 value, int decimals)
    {
        return new Vector3(
            (float)System.Math.Round(value.x, decimals),
            (float)System.Math.Round(value.y, decimals),
            (float)System.Math.Round(value.z, decimals)
        );
    }
}
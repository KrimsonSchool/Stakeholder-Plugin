using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(LDV_los))]
public class LDV_LineOfSight : Editor
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnSceneGUI()
    {
        Physics.Raycast(Selection.activeGameObject.gameObject.transform.position + Selection.activeGameObject.gameObject.GetComponent<LDV_los>().eyeLevel, Selection.activeGameObject.gameObject.transform.forward, out var hit);
        Handles.DrawDottedLine(Selection.activeGameObject.gameObject.transform.position + Selection.activeGameObject.gameObject.GetComponent<LDV_los>().eyeLevel, hit.point, 4);
        Handles.DrawSolidDisc(hit.point, Vector3.up, 0.1f);
    }
}

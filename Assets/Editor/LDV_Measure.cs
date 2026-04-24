using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LDV_mes))]
public class LDV_Measure : Editor
{
    GameObject measure1;
    GameObject measure2;

    private Brief02LevelDesignVisualisation LDV;
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
        measure1 = Brief02LevelDesignVisualisation.measure1;
        measure2 = Brief02LevelDesignVisualisation.measure2;
        
        Handles.color = Color.green;
        Handles.DrawSolidDisc(measure1.transform.position, Vector3.up, 1);
        Handles.DrawSolidDisc(measure2.transform.position, Vector3.up, 1);
        Handles.color = Color.red;
        Handles.DrawDottedLine(measure1.transform.position, measure2.transform.position, 4);
        Vector3 avPos = (measure1.transform.position + measure2.transform.position) / 2;
        GUIStyle measureStyle = new GUIStyle
        { 
            normal = new GUIStyleState()
            {
                textColor = Color.black,
            }
        };
        Handles.Label(avPos, Vector3.Distance(measure1.transform.position, measure2.transform.position)+"m", measureStyle);
    }
}

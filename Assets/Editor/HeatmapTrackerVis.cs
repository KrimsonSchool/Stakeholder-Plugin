using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HeatmapTracker))]

public class HeatmapTrackerVis : Editor
{
    private List<Vector3> points;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSceneGUI()
    {
        points = Brief02LevelDesignVisualisation.points;
        if (points != null)
        {
            if (points.Count > 1)
            {
                for (int i = 1; i < points.Count; i++)
                {
                    Handles.color = Color.red;
                    Handles.DrawDottedLine(points[i - 1], points[i], 4);
                }
            }
        }
    }
}

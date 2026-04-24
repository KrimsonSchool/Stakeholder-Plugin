using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HeatmapTracker : MonoBehaviour
{
    public static event Action<List<Vector3>> UpdatePoints;
    public enum TrackerType
    {
        Player,
        Interactable
    }
    
    public TrackerType trackerType;
    public List<Vector3> points;

    public float period;

    private float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer+=Time.deltaTime;
        if (timer >= period)
        {
            timer = 0;
            points.Add(transform.position);
            
            UpdatePoints?.Invoke(points);
        }
    }
}

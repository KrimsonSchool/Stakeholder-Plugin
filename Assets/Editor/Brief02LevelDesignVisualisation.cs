using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Object = UnityEngine.Object;


public class Brief02LevelDesignVisualisation : EditorWindow
{
    //use try get component
    bool measuring = false;

    public string dependencyFile="LDV_d.txt";
    string dependencyFilePath;

    public static GameObject measure1; 
    public static GameObject measure2; 
    
    bool sec;

    private List<GameObject> prefabs;
    
    Vector2 scrollPos;

    private bool hasReport;

    private string report;

    public string groundTag;
    public string obstacleTag;
    public string enemyTag;
    
    public static List<Vector3> points;

    private Vector2 sc;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //delete before build
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnScene;
        HeatmapTracker.UpdatePoints += UpdatePoints;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //distance calc
    //Sightline visualiser
    //place common items
    //level report generator (number of types of objects ect.
    //heatmap of player movement and interaction
    
    [MenuItem("Stakeholder/Level Design Visualisation")]
    public static void ShowWindow()
    {
        GetWindow<Brief02LevelDesignVisualisation>("Level Design Visualisation");
    }

    void OnGUI()
    {
        if (measuring)
        {
            if (measure1 == null && measure2 == null)
            {
                ReaquireMeasures();
            }
            else if (measure1 == null)
            {
                Destroy(measure2);
                ReaquireMeasures();
            }
            else if (measure2 == null)
            {
                Destroy(measure1);
                ReaquireMeasures();
            }
        }

        if (Mouse.current.leftButton.isPressed && measuring)
        {
            TryClick();
        }
        
        dependencyFilePath = "Assets/"+dependencyFile;
        if (!System.IO.File.Exists(dependencyFilePath))
        {
            Debug.LogError("dependency file not found, creating...");
            System.IO.File.Create(dependencyFilePath);
        }

        /*
        if (System.IO.File.ReadAllText(dependencyFilePath) == "measuring")
        {
            //measuring = true;
        }*/
        
        
        //texture1 = Camera.main.GetComponent<RawImage>().texture;
        string[] todo;
        todo = System.IO.File.ReadAllLines("Assets/todo.txt");

        string todolist = "";

        foreach (var t in todo)
        {
            todolist += ""+ t + "\n";
        }
        
        GUILayout.Label("Stakeholder Plugin", EditorStyles.largeLabel);
        GUILayout.Label(todolist, EditorStyles.label);

        GUILayout.Label("Heatmap", EditorStyles.boldLabel);
        if (Selection.activeGameObject.GetComponent<HeatmapTracker>() == null)
        {
            if (GUILayout.Button("Add heatmap tracker"))
            {
                Selection.activeGameObject.AddComponent<HeatmapTracker>();
            }
        }

        string pnts="";
        if (points != null)
        {
            foreach (var p in points)
            {
                pnts += p + "\n";
            }
        }

        sc = EditorGUILayout.BeginScrollView(sc);
        GUILayout.Label(pnts);
        EditorGUILayout.EndScrollView();
        GUILayout.Space(8);

        GUILayout.Label("Measure tool", EditorStyles.boldLabel);
        if (measuring)
        {
            if (GUILayout.Button("Finish Measuring"))
            {
                //System.IO.File.WriteAllText(dependencyFilePath, "");
                measuring=false;
                DestroyImmediate(measure1);
                DestroyImmediate(measure2);
            }
            
            //click one first pos
            //click 2 second pos
            //calc dist and display as widget
        }
        else
        {
            if (GUILayout.Button("Start Measuring"))
            {
                //System.IO.File.WriteAllText(dependencyFilePath, "measuring");
                measuring = true;
            }
        }

        GUILayout.Space(8);
        if (Selection.activeGameObject != null)
        {
            if (Selection.activeGameObject.GetComponent<LDV_los>() == null)
            {
                GUILayout.Label("Line of sight tool", EditorStyles.boldLabel);
                if (GUILayout.Button("Add Line of sight tool"))
                {
                    Selection.activeGameObject.AddComponent<LDV_los>();
                    //Selection.activeGameObject.GetComponent<LDV_los>().eyeLevel = Selection.activeGameObject.transform.position;
                }
            }
        }
        
        
        GUILayout.Space(8);
        GUILayout.Label("Quik Prefabs", EditorStyles.boldLabel);
        //GUILayout.SelectionGrid(prefabsContent.Length, prefabsContent, 4);
        //GUILayout prefabSquare = new GUILayout();
        //GUILayout.BeginArea(new Rect(100, 100, 200, 200));
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);

        for (int i = 0; i < prefabs.Count; i++)
        {
            prefabs[i] = (GameObject)EditorGUILayout.ObjectField("Prefab", prefabs[i], typeof(GameObject), false);
            if (GUILayout.Button("Spawn Prefab"))
            {
                if (prefabs[i] != null)
                {
                    Instantiate(prefabs[i], Vector3.zero, Quaternion.identity);
                }
            }
        }
        foreach (var p in prefabs)
        {
            
        }
        EditorGUILayout.EndScrollView();
        //GUILayout.EndArea();
        if (GUILayout.Button("Add new prefab"))
        {
            prefabs.Add(null);
        }
        if (GUILayout.Button("Remove prefab"))
        {
            prefabs.RemoveAt(prefabs.Count - 1);
        }
        
        GUILayout.Space(16);
        
        GUILayout.Label("Stats Reporter", EditorStyles.boldLabel);

        groundTag = GUILayout.TextField(groundTag);
        obstacleTag = GUILayout.TextField(obstacleTag);
        enemyTag = GUILayout.TextField(enemyTag);
       
        if (GUILayout.Button("Generate Report"))
        {
            GenerateReport();
        }

        if (hasReport)
        {
            GUILayout.Label("Level Report:", EditorStyles.label);
            GUILayout.Label(report, EditorStyles.label);
        }

        GUILayout.Space(16);

        Repaint();
    }
    
    void TryClick()
    {
        
    }

    void print(string msg)
    {
        Debug.Log(msg);
    }
    
    void OnScene(SceneView scene)
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown && e.button == 0 && measuring)
        {
            //Debug.Log("Mouse was pressed");

            Vector3 mousePos = e.mousePosition;
            float ppp = EditorGUIUtility.pixelsPerPoint;
            mousePos.y = scene.camera.pixelHeight - mousePos.y * ppp;
            mousePos.x *= ppp;

            Ray ray = scene.camera.ScreenPointToRay(mousePos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                //Do something, ---Example---
                //GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                if (!sec)
                {
                    measure1.transform.position = hit.point;
                    Selection.activeObject = measure1;
                    
                    sec = true;
                }
                else
                {
                    measure2.transform.position = hit.point;
                    Selection.activeObject = measure2;
                    sec = false;
                }
            }
            e.Use();
        }
    }
    
    public void ReaquireMeasures()
    {
        measure1 = new GameObject();
        measure1.AddComponent<LDV_mes>();
        measure1.name = "Measure pos 1";
        measure2 = new GameObject();
        measure2.AddComponent<LDV_mes>();
        measure2.name = "Measure pos 2";
    }

    public void GenerateReport()
    {
        hasReport = false;
        report = "";
        //
        GameObject[] allObjs = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
       
        GameObject[] groundObjs = GetObjectTag(groundTag);
        GameObject[] obstacleObjs = GetObjectTag(obstacleTag);
        GameObject[] enemyObjs = GetObjectTag(enemyTag);
        
        int allBM = NoOfObjs(groundObjs)+NoOfObjs(obstacleObjs)+NoOfObjs(enemyObjs);
        int miscCount = allObjs.Length -  allBM;
        
        report+="Number of GameObjects: " + allObjs.Length+"\n\n";
        
        report += "Number of Ground Objects: " + NoOfObjs(groundObjs) + "\n";
        report += "Number of Obstacles: " + NoOfObjs(obstacleObjs) + "\n";
        report += "Number of Enemies: " + NoOfObjs(enemyObjs) + "\n";   
        report += "Number of Misc Objects: " + miscCount + "\n\n";
        report+= "Total Ground Area: " + GenerateAreaReport(groundObjs) + "m²";
        hasReport = true;
    }
    
    public GameObject[] GetObjectTag(string tagToCheck)
    {
        GameObject[] objs;
        try
        {
            // This throws a UnityException if the tag is completely undefined
            objs = GameObject.FindGameObjectsWithTag(tagToCheck);
            return objs;
        }
        catch (UnityException)
        {
            return null;
        }
    }

    public int NoOfObjs(GameObject[] objs)
    {
        if (objs!=null)
        {
            return objs.Length;
        }

        return 0;
    }

    public float GenerateAreaReport(GameObject[] groundObjs)
    {
        float totalArea=0;
        if (groundObjs != null)
        {
            foreach (var g in groundObjs)
            {
                totalArea += g.transform.localScale.x * g.transform.localScale.z;
            }
        }

        return totalArea;
    }

    void UpdatePoints(List<Vector3> pnts)
    {
        points = pnts;
    }
}


/*
 * public class ClickSpawn : MonoBehaviour
   {
       private void OnEnable()
       {
           if (!Application.isEditor)
           {
               Destroy(this);
           }
           SceneView.onSceneGUIDelegate += OnScene;
       }
   
       void OnScene(SceneView scene)
       {
           Event e = Event.current;
   
           if (e.type == EventType.MouseDown && e.button == 2)
           {
               Debug.Log("Middle Mouse was pressed");
   
               Vector3 mousePos = e.mousePosition;
               float ppp = EditorGUIUtility.pixelsPerPoint;
               mousePos.y = scene.camera.pixelHeight - mousePos.y * ppp;
               mousePos.x *= ppp;
   
               Ray ray = scene.camera.ScreenPointToRay(mousePos);
               RaycastHit hit;
   
               if (Physics.Raycast(ray, out hit))
               {
                   //Do something, ---Example---
                   GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                   go.transform.position = hit.point;
                   Debug.Log("Instantiated Primitive " + hit.point);
               }
               e.Use();
           }
       }
   }
 */

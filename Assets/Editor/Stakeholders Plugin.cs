using UnityEngine;
using UnityEditor;
using UnityEngine.UI;


public class StakeholdersPlugin : EditorWindow
{
    Material mat;
    private float angle;
    
    public Texture texture1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
    
    [MenuItem("Stakeholder/Plugin")]
    public static void ShowWindow()
    {
        GetWindow<StakeholdersPlugin>("Main");
    }

    void OnGUI()
    {
        //texture1 = Camera.main.GetComponent<RawImage>().texture;
        string[] todo;
        todo = System.IO.File.ReadAllLines("Assets/todo.txt");

        string todolist = "";

        foreach (var t in todo)
        {
            todolist += "[ ] "+ t + "\n";
        }
        
        GUILayout.Label("Stakeholder Plugin", EditorStyles.largeLabel);
        GUILayout.Label(todolist, EditorStyles.label);
        
        
        RenderCube(100, new Vector3(200, 400, 0));
        RenderCube(100, new Vector3(500, 400, 0));
        RenderCube(100, new Vector3(500, 800, 0));
        RenderCube(100, new Vector3(200, 800, 0));

        angle++;
        Repaint();
    }

    public void RenderCube(float size, Vector3 startPos)
    {
        //mat = new Material(Shader.Find("Hidden/Internal-Colored"));
        mat = new Material(Shader.Find("Unlit/Texture"));
        
        texture1 = Camera.main.GetComponent<RawImage>().texture;
        
        mat.SetTexture("_MainTex", texture1);
        GL.Clear(true, false, Color.black);
        //GL.ClearWithSkybox(true, Camera.main);
        
        GL.PushMatrix();
        
        //GL.LoadOrtho();
        //GL.TexCoord2(size,size);
        GL.LoadProjectionMatrix(Matrix4x4.Ortho(0, position.width, position.height, 0, -1000, 1000));
        mat.SetPass(0);
        
        GL.Begin(GL.QUADS);
        Matrix4x4 matrix = Matrix4x4.TRS(startPos, Quaternion.Euler(angle/4, angle,0), Vector3.one);
        GL.MultMatrix(matrix);
        
        // --- FRONT ---
        //GL.Color(Color.red);
        GL.TexCoord2(0, 0);
        GL.Vertex3(-size, -size,  size);
        //GL.Color(Color.green);
        GL.TexCoord2(1/6f, 0);
        GL.Vertex3( size, -size,  size);
        //GL.Color(Color.blue);
        GL.TexCoord2(1/6f, 1);
        GL.Vertex3( size,  size,  size);
        //GL.Color(Color.cyan);
        GL.TexCoord2(0, 1);
        GL.Vertex3(-size,  size,  size);

        // --- BACK ---
        //GL.Color(Color.green);
        GL.TexCoord2((1 / 6f), 0);
        GL.Vertex3( size, -size, -size);
        //GL.Color(Color.blue);
        GL.TexCoord2(2*(1 / 6f), 0);
        GL.Vertex3(-size, -size, -size);
        //GL.Color(Color.cyan);
        GL.TexCoord2(2*(1 / 6f), 1);
        GL.Vertex3(-size,  size, -size);
        //GL.Color(Color.magenta); 
        GL.TexCoord2((1 / 6f), 1);
        GL.Vertex3( size,  size, -size);

        // --- TOP ---
        //GL.Color(Color.blue);
        GL.TexCoord2(2*(1 / 6f), 0);
        GL.Vertex3(-size,  size,  size);
        //GL.Color(Color.cyan);
        GL.TexCoord2(3*(1 / 6f), 0);
        GL.Vertex3( size,  size,  size);
        //GL.Color(Color.magenta); 
        GL.TexCoord2(3*(1 / 6f), 1);
        GL.Vertex3( size,  size, -size);
        //GL.Color(Color.yellow); 
        GL.TexCoord2(2*(1 / 6f), 1);
        GL.Vertex3(-size,  size, -size);

        // --- BOTTOM ---
        //GL.Color(Color.cyan);
        GL.TexCoord2(3*(1 / 6f), 0);
        GL.Vertex3(-size, -size, -size);
        //GL.Color(Color.magenta); 
        GL.TexCoord2(4*(1 / 6f), 0);
        GL.Vertex3( size, -size, -size);
        //GL.Color(Color.yellow); 
        GL.TexCoord2(4*(1 / 6f), 1);
        GL.Vertex3( size, -size,  size);
        //GL.Color(Color.red);
        GL.TexCoord2(3*(1 / 6f), 1);
        GL.Vertex3(-size, -size,  size);

        // --- LEFT ---
        //GL.Color(Color.magenta); 
        GL.TexCoord2(4*(1 / 6f), 0);
        GL.Vertex3(-size, -size, -size);
        //GL.Color(Color.yellow); 
        GL.TexCoord2(5*(1 / 6f), 0);
        GL.Vertex3(-size, -size,  size);
        //GL.Color(Color.red);
        GL.TexCoord2(5*(1 / 6f), 1);
        GL.Vertex3(-size,  size,  size);
        //GL.Color(Color.green);
        GL.TexCoord2(4*(1 / 6f), 1);
        GL.Vertex3(-size,  size, -size);

        // --- RIGHT ---
        //GL.Color(Color.yellow); 
        GL.TexCoord2(5*(1 / 6f), 0);
        GL.Vertex3( size, -size,  size);
        //GL.Color(Color.red);
        GL.TexCoord2(6*(1 / 6f), 0);
        GL.Vertex3( size, -size, -size);
        //GL.Color(Color.green);
        GL.TexCoord2(6*(1 / 6f), 1);
        GL.Vertex3( size,  size, -size);
        //GL.Color(Color.blue);
        GL.TexCoord2(5*(1 / 6f), 1);
        GL.Vertex3( size,  size,  size);
        
        GL.End();
        
        GL.PopMatrix();
    }
}

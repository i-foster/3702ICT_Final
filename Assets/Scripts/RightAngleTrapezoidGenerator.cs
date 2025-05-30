using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class TrapezoidMeshSaver : MonoBehaviour
{
    [Header("Trapezoid Settings")]
    public float width = 4f;
    public float length = 6f;
    public float height = 2f;
    public float slopeAngle = 30f;
    public Material material;

    [Header("Save Settings")]
    public string meshName = "MyTrapezoid";
    public bool mirror = false;

    void Start()
    {
        GenerateAndSave();
    }

    public void GenerateAndSave()
    {
        GameObject trapezoid = CreateTrapezoid();
        
        #if UNITY_EDITOR
        // Save mesh as asset
        Mesh mesh = trapezoid.GetComponent<MeshFilter>().sharedMesh;
        AssetDatabase.CreateAsset(mesh, "Assets/" + meshName + (mirror ? "_Mirrored" : "") + ".asset");
        AssetDatabase.SaveAssets();
        
        // Create prefab
        PrefabUtility.SaveAsPrefabAsset(trapezoid, "Assets/" + meshName + (mirror ? "_Mirrored" : "") + ".prefab");
        DestroyImmediate(trapezoid);
        #endif
    }

    GameObject CreateTrapezoid()
    {
        GameObject trapezoid = new GameObject(meshName);
        MeshFilter meshFilter = trapezoid.AddComponent<MeshFilter>();
        MeshRenderer renderer = trapezoid.AddComponent<MeshRenderer>();
        renderer.material = material;

        Mesh mesh = new Mesh();
        float slopeOffset = height / Mathf.Tan(slopeAngle * Mathf.Deg2Rad);

        Vector3[] vertices;
        if (mirror)
        {
            vertices = new Vector3[]
            {
                // Right trapezoid vertices (same as your original)
                new Vector3(width/2, 0, length/2),
                new Vector3(width/2, 0, -length/2),
                new Vector3(width/2 - slopeOffset, height, length/2),
                new Vector3(width/2 - slopeOffset, height, -length/2),
                new Vector3(-width/2, 0, length/2),
                new Vector3(-width/2, 0, -length/2),
                new Vector3(-width/2, height, length/2),
                new Vector3(-width/2, height, -length/2)
            };
        }
        else
        {
            vertices = new Vector3[]
            {
                // Left trapezoid vertices (same as your original)
                new Vector3(-width/2, 0, length/2),
                new Vector3(-width/2, 0, -length/2),
                new Vector3(-width/2 + slopeOffset, height, length/2),
                new Vector3(-width/2 + slopeOffset, height, -length/2),
                new Vector3(width/2, 0, length/2),
                new Vector3(width/2, 0, -length/2),
                new Vector3(width/2, height, length/2),
                new Vector3(width/2, height, -length/2)
            };
        }

        int[] triangles = {
            0, 2, 3, 0, 3, 1, // Outer face
            5, 7, 6, 5, 6, 4,  // Inner face
            0, 4, 6, 0, 6, 2,  // Left side
            1, 3, 7, 1, 7, 5,  // Right side
            2, 6, 7, 2, 7, 3,  // Top
            0, 1, 5, 0, 5, 4    // Bottom
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        
        meshFilter.mesh = mesh;
        return trapezoid;
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(TrapezoidMeshSaver))]
    class Editor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Generate & Save"))
            {
                ((TrapezoidMeshSaver)target).GenerateAndSave();
            }
        }
    }
    #endif
}
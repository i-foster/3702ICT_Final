using UnityEngine;
using UnityEngine.AI;

public class StaticPathVisualizer : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform buildingTarget;

    private LineRenderer lineRenderer;
    private NavMeshPath path;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        path = new NavMeshPath();
        Debug.Log("StaticPathVisualizer initialized with no target.");
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.2f;

        lineRenderer.widthCurve = new AnimationCurve(
            new Keyframe(0, 0.3f),
            new Keyframe(1, 0.05f)
        );


        //setting style of new line render
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.cyan, 0.0f),
                new GradientColorKey(Color.blue, 1.0f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1.0f, 0.0f),
                new GradientAlphaKey(0.0f, 1.0f)
            }
        );
        lineRenderer.colorGradient = gradient;

        Debug.Log("StaticPathVisualizer initialized.");
    }

    void Update()
    {
        if (agent == null || buildingTarget == null)
        {
            Debug.LogWarning("Agent or BuildingTarget is null. Skipping path rendering.");
            return;
        }

        if (NavMesh.CalculatePath(agent.transform.position, buildingTarget.position, NavMesh.AllAreas, path))
        {
            lineRenderer.positionCount = path.corners.Length;
            lineRenderer.SetPositions(path.corners);

            Debug.Log($"Path calculated to target '{buildingTarget.name}'. Corners: {path.corners.Length}");
        }
        else
        {
            lineRenderer.positionCount = 0;
            Debug.LogWarning("Failed to calculate path.");
        }
    }

    public void SetTarget(Transform newTarget)
    {
        buildingTarget = newTarget;

        if (buildingTarget != null)
        {
            Debug.Log($"New target set for path visualization: {buildingTarget.name}");
        }
        else
        {
            Debug.LogWarning("SetTarget called with null target.");
        }
    }

    public void ForcePathRefresh()
    {
        if (agent == null || buildingTarget == null) return;

        NavMesh.CalculatePath(agent.transform.position, buildingTarget.position, NavMesh.AllAreas, path);
        lineRenderer.positionCount = path.corners.Length;
        lineRenderer.SetPositions(path.corners);
    }

}

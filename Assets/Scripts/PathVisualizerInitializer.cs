using UnityEngine;

public class PathVisualizerInitializer : MonoBehaviour
{
    public StaticPathVisualizer pathVisualizer;
    public Transform n64Target;
    public Transform n53Target;
    public Transform n76Target;

    void Start()
    {
        string building = MenuSelectionManager.selectedBuilding;
        Debug.Log("Selected building: " + building);

        if (pathVisualizer == null)
        {
            Debug.LogError("PathVisualizer not assigned!");
            return;
        }

        switch (building)
        {
            case "N64":
                pathVisualizer.SetTarget(n64Target);
                break;
            case "N53":
                pathVisualizer.SetTarget(n53Target);
                break;
            case "N76":
                pathVisualizer.SetTarget(n76Target);
                break;
            default:
                Debug.LogWarning("Unknown building selected: " + building);
                break;
        }
    }
}

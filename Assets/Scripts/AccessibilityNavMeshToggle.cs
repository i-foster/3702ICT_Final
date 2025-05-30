using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Unity.AI.Navigation;

public class AccessibilityNavMeshToggle : MonoBehaviour
{
    [Header("References")]
    public GameObject[] stairsObjects;             
    public NavMeshSurface navMeshSurface;          
    public Button accessibilityToggleButton;       
    public StaticPathVisualizer pathVisualizer;    

    [Header("Layers")]
    public string defaultLayerName = "Default";         
    public string excludeLayerName = "NavMeshIgnore";  

    private bool accessibilityMode = false;

    void Start()
    {
        if (accessibilityToggleButton != null)
        {
            accessibilityToggleButton.onClick.AddListener(ToggleAccessibilityMode);
        }
        else
        {
            Debug.LogWarning("Accessibility button is not assigned.");
        }
    }

    void ToggleAccessibilityMode()
    {
        accessibilityMode = !accessibilityMode;

        string targetLayerName = accessibilityMode ? excludeLayerName : defaultLayerName;
        int targetLayer = LayerMask.NameToLayer(targetLayerName);

        if (targetLayer == -1)
        {
            Debug.LogError($"Layer '{targetLayerName}' does not exist. Please create it in the Layer Manager.");
            return;
        }

        foreach (GameObject obj in stairsObjects)
        {
            if (obj == null) continue;

            obj.layer = targetLayer;

            // Update all children too (optional)
            foreach (Transform child in obj.transform)
            {
                child.gameObject.layer = targetLayer;
            }
        }

        navMeshSurface.BuildNavMesh();
        StartCoroutine(DelayPathRefresh());

        Debug.Log($"Accessibility mode {(accessibilityMode ? "ON" : "OFF")}: Stairs set to layer '{targetLayerName}' and NavMesh rebuilt.");
    }

    private IEnumerator DelayPathRefresh()
    {
        yield return new WaitForEndOfFrame();

        if (pathVisualizer != null)
        {
            pathVisualizer.ForcePathRefresh();
            Debug.Log("Path visualizer refreshed after NavMesh rebuild.");
        }
    }
}

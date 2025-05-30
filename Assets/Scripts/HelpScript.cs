using UnityEngine;

public class HelpScript : MonoBehaviour
{
    public GameObject hintPanel;
    public GameObject HelpBeacon;
    
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && hintPanel != null)
        {
            hintPanel.SetActive(true);
            HelpBeacon.SetActive(false);
        }
    }
     private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag) && hintPanel != null)
        {
            hintPanel.SetActive(false);
            HelpBeacon.SetActive(true);
        }
    }
}

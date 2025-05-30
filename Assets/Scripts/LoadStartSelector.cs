using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadStartSelector : MonoBehaviour
{


    public void RoutineStart()
    {
        StartCoroutine(SelectStart());
    }
    public void RoutineSetting()
    {
        StartCoroutine(Settingstart());
    }
        public void RoutineMenu()
    {
        StartCoroutine(MainMenuStart());
    }

        public void DelayedStart()
    {
        StartCoroutine(MainMenuStart());
    }

    public IEnumerator FourmilisecondDelay()
    {
        yield return new WaitForSeconds(0.4f);
    }


    
    public IEnumerator MainMenuStart()
    {
        yield return new WaitForSeconds(0.4f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);
    }

    public IEnumerator SelectStart()
    {
        yield return new WaitForSeconds(0.4f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);
    }
    public IEnumerator Settingstart() 
    {
        yield return new WaitForSeconds(0.4f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(2);
    }
    public void Quitting() 
    {
        Application.Quit();
    }
}


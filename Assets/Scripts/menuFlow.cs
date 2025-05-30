using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq; 
using System.Collections;

public class MenuFlowController : MonoBehaviour

// Initializing all items.
{
    [Header("Menu Panels")]
    public GameObject menu1Panel;
    public GameObject menu2Panel;
    public GameObject campusPanel;

    [Header("Menu 1 Images || text")]
    public GameObject n64Image;
    public GameObject n64Text;
    public GameObject n53Image;
    public GameObject n53Text;

    [Header("Campus Images ")]
    public GameObject nathanImage;
    public GameObject goldCoastImage;


    [Header("Menu 1 Buttons")]
    public Button menu1ChoiceN64Button;
    public Button menu1ChoiceN53Button;
    public Button menu1Choice3Button;
    public Button menu1Choice4Button;
    public Button Menu1ReturnButton;

    [Header("Campus Menu Buttons")]
    public Button nathanCampusButton;
    public Button goldCoastCampusButton;
    public Button campusReturnButton;

    [Header("Menu 2 Setup")]
    public Transform menu2ButtonContainer;
    public GameObject menu2ChoiceButtonPrefab;


    private List<string> allPossibleChoices = new List<string> {
        "N64", "N53", "N76", "Choice 4", "Return"
    };

    private string selectedChoiceOfCampus;
    private string selectedChoiceFromMenu1;
    private string selectedChoiceFromMenu2;

    void Start()
    {
        // Ensureall items are assinged are assigned
        if (menu1ChoiceN64Button == null || menu1ChoiceN53Button == null || menu1Choice3Button == null || menu1Choice4Button == null || Menu1ReturnButton == null)
        {
            Debug.LogError("Menu 1 buttons not assigned in the inspector!");
            return;
        }
        if (nathanCampusButton == null || goldCoastCampusButton == null || campusReturnButton == null)
        {
            Debug.LogError("Campus buttons not assigned in the inspector!");
            return;
        }
        if (menu1Panel == null || menu2Panel == null || campusPanel == null)
        {
            Debug.LogError("Menu panels not assigned in the inspector!");
            return;
        }
        if (menu2ButtonContainer == null || menu2ChoiceButtonPrefab == null)
        {
            Debug.LogError("Menu 2 container or prefab not assigned!");
            return;
        }
        if (n64Image == null || n64Text == null || n53Image == null || n53Text == null)
        {
            Debug.LogError("Menu 1 images and texts not assigned in the inspector!");
            return;
        }
        if (nathanImage == null || goldCoastImage == null)
        {
            Debug.LogError("Campus images are not assigned in the inspector!");
            return;
        }

        // Event listeners for buttons
        nathanCampusButton.onClick.AddListener(() => HandleCampusChoice("Nathan"));
        goldCoastCampusButton.onClick.AddListener(() => HandleCampusChoice("GoldCoast"));
        campusReturnButton.onClick.AddListener(() => HandleCampusChoice("Return"));
        menu1ChoiceN64Button.onClick.AddListener(() => HandleMenu1Choice("N64"));
        menu1ChoiceN53Button.onClick.AddListener(() => HandleMenu1Choice("N53"));
        menu1Choice3Button.onClick.AddListener(() => HandleMenu1Choice("N76"));
        menu1Choice4Button.onClick.AddListener(() => HandleMenu1Choice("Choice 4"));
        Menu1ReturnButton.onClick.AddListener(() => HandleMenu1Choice("Return"));


        campusPanel.SetActive(true);
        //Hiding menu panels  and images on load
        menu1Panel.SetActive(false);
        menu2Panel.SetActive(false);
        ImagesHide(); 
    }

    void HandleCampusChoice(string choice)
    {
        selectedChoiceOfCampus = choice;
        if (selectedChoiceOfCampus == "Nathan")
        {
            campusPanel.SetActive(false);
            menu1Panel.SetActive(true);
        }
        else if (selectedChoiceOfCampus == "GoldCoast")
        {

        }
        else if (selectedChoiceOfCampus == "Return")
        {
            StartCoroutine(HandleCampusReturn());
        }
    }

    void HandleMenu1Choice(string choice)
    {
        selectedChoiceFromMenu1 = choice;

        if (selectedChoiceFromMenu1 == "Return")
        {
            HandleMenu1Return();
            menu1Panel.SetActive(false);
        }
        else
        {
            menu1Panel.SetActive(false);
            menu2Panel.SetActive(true);
            PopulateMenu2();
        }

    }

    void PopulateMenu2()
    {
        // 1. Clear any existing buttons in Menu 2's container
        foreach (Transform child in menu2ButtonContainer)
        {
            Destroy(child.gameObject);
        }

        List<string> availableChoicesForMenu2 = allPossibleChoices.Where(item => item != selectedChoiceFromMenu1).ToList();


        List<string> finalChoicesForMenu2 = availableChoicesForMenu2.Take(5).ToList();


        foreach (string choiceText in finalChoicesForMenu2)
        {
            GameObject buttonGO = Instantiate(menu2ChoiceButtonPrefab, menu2ButtonContainer);

            Text buttonUIText = buttonGO.GetComponentInChildren<Text>();
            if (buttonUIText != null)
            {
                buttonUIText.text = choiceText;
            }
            else
            {
                TMPro.TextMeshProUGUI buttonTMPText = buttonGO.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                if (buttonTMPText != null)
                {
                    buttonTMPText.text = choiceText;
                }
            }

            Button buttonComponent = buttonGO.GetComponent<Button>();
            if (buttonComponent != null)
            {
                string currentChoice = choiceText;
                buttonComponent.onClick.AddListener(() => HandleMenu2Choice(currentChoice));
            }
        }
    }

    void HandleMenu2Choice(string choice)
    {
        selectedChoiceFromMenu2 = choice;
        if (selectedChoiceFromMenu2 == "Return")
        {
            menu1Panel.SetActive(true);
            menu2Panel.SetActive(false);
        }
        else
        {
            MenuSelectionManager.selectedBuilding = selectedChoiceFromMenu2;
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Assets/scenes/Edited Samplescene.unity");
        }

    }
    public IEnumerator HandleCampusReturn()
    {
        yield return new WaitForSeconds(0.4f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);
    }

    void HandleMenu1Return()
    {
        campusPanel.SetActive(true);
        menu1Panel.SetActive(false);
    }
    public void ImagesHide()
    { //Set all images to hidden on start
        ImageHideNathan();
        ImageHideGoldCoast();
        ImageHideN64();
        ImageHideN53();
    }

    /* Show and Hide functions for each image */
    public void ImageShowN64()
    {
        n64Image.SetActive(true);
        n64Text.SetActive(true);
    }
    public void ImageHideN64()
    {
        n64Image.SetActive(false);
        n64Text.SetActive(false);
    }
    public void ImageShowN53()
    {
        n53Image.SetActive(true);
        n53Text.SetActive(true);
    }
    public void ImageHideN53()
    {
        n53Image.SetActive(false);
        n53Text.SetActive(false);
    }
    public void ImageShowNathan()
    {
        nathanImage.SetActive(true);
  
    }
    public void ImageHideNathan()
    {
        nathanImage.SetActive(false);

    }
    public void ImageShowGoldCoast()
    {
        goldCoastImage.SetActive(true);
    }
    public void ImageHideGoldCoast()
    {
        goldCoastImage.SetActive(false);
    }
}
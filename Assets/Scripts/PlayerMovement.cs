using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    // Initializing all items.
    [Header("Camera")]
    public Camera playerCamera;

    [Header("Menu Panels")]
    public GameObject pausePanel;
    public GameObject reportPanel;
    public GameObject disabilityPanel;

    [Header("Pause Menu Buttons")]
    public Button resumeGameButton;
    public Button reportButton;
    public Button resetButton;
    public Button quitButton;

    [Header("Report Menu UI")]
    public Button issueReportButton;
    public Button returnMenuButton;

    [Header("Movement Speeds")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float jumpPower = 7f;
    public float gravity = 20f;
    public float lookSpeed = 2f;
    public float lookXLimit = 90f;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;

    private bool canMove = true;
    private bool isCurrentlyRunning = false; // Tracks running status

    void Start()
    {
        // Debugging messages for assigned items
        if (pausePanel == null || reportPanel == null)
        {
            Debug.LogError("Panels not assigned in the inspector!");
            return;
        }
        if (resumeGameButton == null || reportButton == null || resetButton == null || quitButton == null)
        {
            Debug.LogError("Pause Menu Buttons not assigned in the inspector!");
            return;
        }
        if (issueReportButton == null || returnMenuButton == null)
        {
            Debug.LogError("Report Menu UI not assigned in the inspector!");
            return;
        }

        // Event listeners for buttons
        resumeGameButton.onClick.AddListener(HandleResume);
        reportButton.onClick.AddListener(HandleReport);
        resetButton.onClick.AddListener(HandleReset);
        quitButton.onClick.AddListener(HandleQuit);
        issueReportButton.onClick.AddListener(handleSubmitReport);
        returnMenuButton.onClick.AddListener(handleReturn);

        // Hiding menu panels
        disabilityPanel.SetActive(false);
        pausePanel.SetActive(false);
        reportPanel.SetActive(false);

        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Determine if the player is grounded
        bool isGrounded = characterController.isGrounded;

        // Running logic (hold Left Shift)
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            isCurrentlyRunning = true; // Start running if grounded and shift is held
        }
        else if (isGrounded)
        {
            isCurrentlyRunning = false; // Stop running if grounded and shift is not held
        }

        // Determine movement speed
        float currentSpeed = isCurrentlyRunning ? runSpeed : walkSpeed;
        float curSpeedX = canMove ? currentSpeed * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? currentSpeed * Input.GetAxis("Horizontal") : 0;

        // Preserve Y-axis movement
        float movementDirectionY = moveDirection.y;

        if (isGrounded)
        {
            // Update movement direction when grounded
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        }
        else
        {
            // Maintain momentum while airborne
            Vector3 horizontalVelocity = new Vector3(moveDirection.x, 0, moveDirection.z);
            Vector3 inputVelocity = (forward * curSpeedX) + (right * curSpeedY);

            // Add input to current horizontal velocity
            if (inputVelocity != Vector3.zero)
            {
                horizontalVelocity = inputVelocity;
            }

            moveDirection = horizontalVelocity;
        }

        // Apply Y-axis movement
        moveDirection.y = movementDirectionY;

        // Jump logic
        if (Input.GetButton("Jump") && canMove && isGrounded)
        {
            moveDirection.y = jumpPower;
        }

        // Apply gravity if not grounded
        if (!isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the character
        characterController.Move(moveDirection * Time.deltaTime);

        // Handle camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        // Pause Menu 
        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            pausePanel.SetActive(true);
            disabilityPanel.SetActive(true);
            canMove = false;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
    }



    /* Pause and report menu functions */
    public void HandleResume()
    {
        disabilityPanel.SetActive(false);
        canMove = true;
        pausePanel.SetActive(false);
        Cursor.visible = false;
        Time.timeScale = 1f;
    }
    public void HandleReport()
    {
        pausePanel.SetActive(false);
        reportPanel.SetActive(true);
    }
    public void handleSubmitReport()
    {
        // Submit and store function here in future version
        pausePanel.SetActive(true);
        reportPanel.SetActive(false);
    }
    public void handleReturn()
    {
        pausePanel.SetActive(true);
        reportPanel.SetActive(false);
    }

    public void HandleReset()
    {
        // Set to player start position in future version
        // Currently just calls HandleResume
        HandleResume();
    }

    public void HandleQuit()
    {//return to main menu
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);
    }
}

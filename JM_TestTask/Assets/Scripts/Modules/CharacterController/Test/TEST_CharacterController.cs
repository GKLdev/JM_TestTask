using UnityEngine;
using Modules.CharacterController_Public;
using GDTUtils;

namespace Test.CharacterController
{
    public class CharacterControllerTester : MonoBehaviour
    {
        [SerializeField]
        private SerializedInterface<ICharacterController> characterController = new();

        [SerializeField]
        private Transform targetTransform;  // Target for MoveToTarget

        [SerializeField]
        private float mouseSensitivity = 2f;  // Mouse sensitivity for look direction

        private bool isNavmeshMode = false;  // Current navigation mode state

        // *****************************
        // Start
        // *****************************
        private void Start()
        {
            // Initialize the character controller module
            if (characterController.Value != null)
            {
                characterController.Value.InitModule();
                characterController.Value.Toggle(true);
            }
            else
            {
                Debug.LogError("CharacterController is not assigned in the inspector.");
            }

            // Lock cursor for mouse look
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // *****************************
        // Update
        // *****************************
        private void Update()
        {
            if (characterController.Value == null)
            {
                return;
            }

            // Update the character controller module
            characterController.Value.OnUpdate();

            // Handle keyboard and mouse input
            HandleMovementInput();
            HandleLookInput();
            HandleNavigationModeToggle();
            HandleMoveToTarget();
        }

        // *****************************
        // HandleMovementInput
        // *****************************
        private void HandleMovementInput()
        {
            // Get movement input from keyboard (WASD or arrow keys)
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            Vector2 movementInput = new Vector2(horizontalInput, verticalInput).normalized;

            // Send movement input to the character controller
            characterController.Value.Move(movementInput);
        }

        // *****************************
        // HandleLookInput
        // *****************************
        private void HandleLookInput()
        {
            // Get mouse input for look direction (relative angles)
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            // Calculate relative look direction based on mouse input
            Vector2 relativeLookDir = new Vector2(mouseX, -mouseY);
            characterController.Value.LookDirectionRelative(relativeLookDir);
        }

        // *****************************
        // HandleNavigationModeToggle
        // *****************************
        private void HandleNavigationModeToggle()
        {
            // Toggle navigation mode with a key (e.g., N key)
            if (Input.GetKeyDown(KeyCode.N))
            {
                isNavmeshMode = !isNavmeshMode;
                NavigationMode newMode = isNavmeshMode ? NavigationMode.Navmesh : NavigationMode.DirectControl;
                characterController.Value.ToggleNavigationMode(newMode);
                Debug.Log($"Navigation mode switched to: {newMode}");
            }
        }

        // *****************************
        // HandleMoveToTarget
        // *****************************
        private void HandleMoveToTarget()
        {
            // Set target position with a key (e.g., T key)
            if (Input.GetKeyDown(KeyCode.T) && targetTransform != null)
            {
                characterController.Value.MoveToTarget(targetTransform.position);
                Debug.Log($"Moving to target at position: {targetTransform.position}");
            }
        }
    }
}
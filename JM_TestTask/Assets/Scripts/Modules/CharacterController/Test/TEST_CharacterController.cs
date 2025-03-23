using UnityEngine;
using Modules.CharacterController_Public;
using GDTUtils;
using Zenject;
using Modules.TimeManager_Public;
using Modules.CharacterManager_Public;
using Modules.CharacterFacade_Public;
using Modules.CharacterController;
using Modules.CharacterControllerView_Public;
using Modules.PlayerProgression_Public;
using Modules.ModuleManager_Public;
using Modules.PlayerWeapon_Public;

namespace Test.CharacterController
{
    public class CharacterControllerTester : MonoBehaviour
    {
        [Inject]
        IModuleManager moduleMgr;

        [Inject]
        ICharacterManager characterMgr;

        [Inject]
        IPlayerProgression plrProgression;

        [SerializeField]
        private SerializedInterface<ICharacterFacade> characterController;

        [SerializeField]
        private SerializedInterface<ICharacterControllerView> characterView;

        [SerializeField]
        private Transform targetTransform;  // Target for MoveToTarget

        [SerializeField]
        private float mouseSensitivity = 2f;  // Mouse sensitivity for look direction

        private bool isNavmeshMode = false;  // Current navigation mode state
        private bool charManagerMode = false;

        private IPlayerWeapon weapon;

        // *****************************
        // Start
        // *****************************
        private void Start()
        {
            if (characterController.Value == null)
            {
                characterController.Value = characterMgr.GetPlayer();
                charManagerMode = true;
            }
            else
            {
                characterController.Value.InitModule(characterView.Value);
                characterController.Value.P_Controller.InitModule();
                characterController.Value.P_Controller.Toggle(true);
            }

            // Lock cursor for mouse look
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            weapon = moduleMgr.Container.Resolve<IPlayerWeapon>();
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
            if (!charManagerMode)
            {
                characterController.Value.OnUpdate();
            }

            // Handle keyboard and mouse input
            HandleMovementInput();
            HandleLookInput();
            HandleNavigationModeToggle();
            HandleMoveToTarget();

            HandleStatInput();
            HandleWeaponInput();
        }

        // *****************************
        // HandleMovementInput
        // *****************************
        private void HandleStatInput()
        {
            if (Input.GetKeyDown("1"))
            {
                plrProgression.SetStatValue(10, PlayerProgressionAliases.speed);
                plrProgression.SetStatValue(10, PlayerProgressionAliases.health);
                plrProgression.SetStatValue(10, PlayerProgressionAliases.damage);
            }
        }

        // *****************************
        // HandleWeaponInput
        // *****************************
        private void HandleWeaponInput()
        {
            if (Input.GetButton("Fire1"))
            {
                weapon.Shoot();
            }
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
            characterController.Value.P_Controller.Move(movementInput);
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
            characterController.Value.P_Controller.LookDirectionRelative(relativeLookDir);
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
                characterController.Value.P_Controller.SetNavigationMode(newMode);
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
                characterController.Value.P_Controller.MoveToTarget(targetTransform.position);
                Debug.Log($"Moving to target at position: {targetTransform.position}");
            }
        }
    }
}
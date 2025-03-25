using GDTUtils;
using Modules.CharacterController;
using Modules.CharacterController_Public;
using Modules.CharacterFacade_Public;
using Modules.CharacterManager_Public;
using Modules.DamageManager_Public;
using Modules.InputManager_Public;
using Modules.PlayerProgression_Public;
using Modules.PlayerWeapon_Public;
using Modules.ReferenceDb_Public;
using Modules.TimeManager_Public;
using Modules.UI.UIController_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Windows;
using Zenject;
using static UnityEngine.GraphicsBuffer;

namespace Modules.GameDirector
{
    public class SimpleGameplayTest : GDTUtils.StateMachine.StateMachineNodeBase<GameDirectorNodeType, State>
    {
        ICharacterManager   characterMgr;
        ITimeManager        timeMgr;
        IPlayerProgression  progression;
        IUIController       ui;
        IInputManager       input;

        ICharacterFacade player;

        private float   aiSpawnOffset           = 10f;
        private float   playerSpawnOffset       = 5f;
        private int     aiCharactersCount       = 10;
        private int     upgradePointsPerKill    = 1;

        // *****************************
        // OnStart
        // *****************************
        protected override void OnStart()
        {
            base.OnStart();

            Debug.Log("Starting game...");

            characterMgr    = stateMachine.P_ExternalReference.dynamicData.container.Resolve<ICharacterManager>();
            timeMgr         = stateMachine.P_ExternalReference.dynamicData.container.Resolve<ITimeManager>();
            progression     = stateMachine.P_ExternalReference.dynamicData.container.Resolve<IPlayerProgression>();
            ui              = stateMachine.P_ExternalReference.dynamicData.container.Resolve<IUIController>();
            input           = stateMachine.P_ExternalReference.dynamicData.container.Resolve<IInputManager>();

            // Spawn player
            player          = SpawnPlayer(Vector3.zero + -Vector3.right * playerSpawnOffset, Vector3.right);

            // Spawn ai characters
            for (int i = 0; i < aiCharactersCount; i++) 
            {
                SpawnAIRandomized();
            }

            // Prepare game systems
            timeMgr.ToggleTimeEvaluation(true);
            progression.ResetUpgradePoints();
            ui.ShowScreen(ScreenType.HUD, true);
            input.SetContext(InputContext.World);
        }

        // *****************************
        // SpawnAI
        // *****************************
        private ICharacterFacade SpawnAI(Vector3 _pos, Vector3 _orientation)
        {
            ICharacterFacade result = characterMgr.CreateCharacter(CATEGORY_CHARACTERS.Character_Test_AI);
            
            result.P_GameObjectAccess.transform.position = _pos;
            result.P_GameObjectAccess.transform.forward = _orientation;

            result.MakeAIControlled();
            result.P_Controller.Toggle(true);
            result.P_Damageable.OnDamageApplied += OnAIDamaged;

            return result;
        }

        // *****************************
        // ICharacterFacade
        // *****************************
        private ICharacterFacade SpawnPlayer(Vector3 _pos, Vector3 _orientation)
        {
            ICharacterFacade result = characterMgr.CreateCharacter(CATEGORY_CHARACTERS.Character_Player);

            result.P_GameObjectAccess.transform.position = _pos;
            result.P_GameObjectAccess.transform.forward = _orientation;

            result.MakePlayerControlled();
            result.P_Controller.Toggle(true);
            result.P_StatsSystem.Toggle(true);

            return result;
        }

        // *****************************
        // OnAIDamaged
        // *****************************
        private void OnAIDamaged(bool _isDead, IDamageable _damageable)
        {
            bool ignore = !_isDead;
            if (ignore)
            {
                return;
            }

            progression.AddUpgradePoints(upgradePointsPerKill);
            _damageable.OnDamageApplied -= OnAIDamaged;

            SpawnAIRandomized();
        }

        // *****************************
        // SpawnAIRandomized
        // *****************************
        private void SpawnAIRandomized()
        {
            float rng = (float)GDTRandom.generalRng.NextDouble(0f, 1f);
            SpawnAI(new Vector3(0 + aiSpawnOffset * rng, 0f, 0f), Vector3.forward);
        }
    }
}

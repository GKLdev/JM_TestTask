using GDTUtils;
using Modules.CharacterFacade_Public;
using Modules.CharacterManager_Public;
using Modules.DamageManager_Public;
using Modules.PlayerProgression_Public;
using Modules.ReferenceDb_Public;
using Modules.TimeManager_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using Zenject;
using static UnityEngine.GraphicsBuffer;

namespace Modules.GameDirector
{
    public class SimpleGameplayTest : GDTUtils.StateMachine.StateMachineNodeBase<GameDirectorNodeType, State>
    {
        ICharacterManager   characterMgr;
        ITimeManager        timeMgr;
        IPlayerProgression  progression;

        List<ICharacterFacade> characters = new();

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

            // Spawn player
            var player = SpawnPlayer(Vector3.zero, Vector3.forward);

            // Spawn ai characters
            for (int i = 0; i < 10; i++) 
            {
                int rng = GDTRandom.generalRng.Next(1, 10);
                SpawnAI(new Vector3(0 + rng, 0f, 0f), Vector3.forward);
            }

            // Prepare game systems
            timeMgr.ToggleTimeEvaluation(true);
            progression.ResetUpgradePoints();
        }

        // *****************************
        // OnUpdate
        // *****************************
        protected override void OnUpdate()
        {
            base.OnUpdate();
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

            Debug.Log("Adding progression point");
            progression.AddUpgradePoints(1);
        }
    }
}

using GDTUtils;
using Modules.CharacterFacade_Public;
using Modules.CharacterManager_Public;
using Modules.ReferenceDb_Public;
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
        [Inject]
        ICharacterManager characterMgr;

        List<ICharacterFacade> characters = new();

        // *****************************
        // OnStart
        // *****************************
        protected override void OnStart()
        {
            base.OnStart();

            Debug.Log("Starting game...");

            characterMgr = stateMachine.P_ExternalReference.dynamicData.container.Resolve<ICharacterManager>();

            // spawn ai characters:

            characterMgr.CreateCharacter(CATEGORY_CHARACTERS.Character_Player);


            for (int i = 0; i < 10; i++) 
            {
                int rng = GDTRandom.generalRng.Next(1, 10);
                SpawnAI(new Vector3(0 + rng, 0f, 0f), Vector3.forward);
            }
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


            return result;
        }
    }
}

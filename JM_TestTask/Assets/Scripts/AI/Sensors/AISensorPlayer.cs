using Modules.AISensors_Public;
using Modules.CharacterFacade_Public;
using Modules.CharacterManager_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.AISensors.Player
{
    public interface IAISensorPlayer : IAISensor
    {
        bool    PlayerExists();
        Vector3 GetOrientation();
        Vector3 GetPosition();
    }

    public class AISensorPlayer : AISensorBase, IAISensorPlayer
    {
        ICharacterManager   characterMgr;

        // *****************************
        // OnInit
        // *****************************
        protected override void OnInit()
        {
            base.OnInit();

            characterMgr = moduleMgr.Container.TryResolve<ICharacterManager>();
        }

        // *****************************
        // PlayerExists
        // *****************************
        public bool PlayerExists()
        {
            return characterMgr.GetPlayer() != null;
        }

        // *****************************
        // GetOrientation
        // *****************************
        public Vector3 GetOrientation() {
            var player = characterMgr.GetPlayer();
            if (player == null)
            {
                return Vector3.zero;
            }

            return player.P_Controller.P_Orientation;
        }

        // *****************************
        // GetPosition
        // *****************************
        public Vector3 GetPosition() {
            var player = characterMgr.GetPlayer();
            if (player == null)
            {
                return Vector3.zero;
            }

            return player.P_Controller.P_Position;
        }
    }
}
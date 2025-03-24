using GDTUtils.Common;
using Modules.CharacterController_Public;
using Modules.CharacterFacade_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.CharacterControllerView_Public
{
    public interface ICharacterControllerView : IModuleInit, IModuleUpdate, ICharacterFacadeCallbacks, IGameObjectAccess, ILifeState
    {
        /// <summary>
        /// Sets the visual state of the character (e.g., Idle or Dead).
        /// </summary>
        void SetVisualState(VisualState _type);

        /// <summary>
        /// Sets up the character controller view with the provided data.
        /// </summary>
        void Setup(CharacterControllerViewSetupData _data);
    }

    public enum VisualState
    {
        Idle,
        Dead
    }

    [System.Serializable]
    public class CharacterControllerViewSetupData
    {
        public ICharacterController characterController;
    }
}

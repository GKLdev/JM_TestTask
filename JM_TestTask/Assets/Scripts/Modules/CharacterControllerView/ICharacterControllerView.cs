using Modules.CharacterController_Public;
using Modules.CharacterFacade_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.CharacterControllerView_Public
{
    public interface ICharacterControllerView : IModuleInit, IModuleUpdate, ICharacterFacadeCallbacks
    {
        /// <summary>
        /// Sets the visual state of the character (e.g., Idle or Dead).
        /// </summary>
        /// <param name="_type">The visual state to set.</param>
        void SetVisualState(VisualState _type);

        /// <summary>
        /// Sets up the character controller view with the provided data.
        /// </summary>
        /// <param name="_data">Setup data containing character controller reference.</param>
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

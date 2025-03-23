using GDTUtils.Common;
using Modules.CharacterControllerView_Public;
using Modules.CharacterFacade_Public;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.CharacterController_Public
{
    public interface ICharacterController : 
        IModuleInit, 
        IModuleUpdate, 
        ITransformData, 
        IDisposable,
        ICharacterFacadeCallbacks
    {
        /// <summary>
        /// Sets the movement direction for the character.
        /// </summary>
        void Move(Vector2 _dir);

        /// <summary>
        /// Sets the target position for the character to move towards.
        /// </summary>
        void MoveToTarget(Vector3 _pos);

        /// <summary>
        /// Enables or disables the module's update.
        /// </summary>
        void Toggle(bool _val);

        /// <summary>
        /// Sets the navigation mode for the character.
        /// </summary>
        void SetNavigationMode(NavigationMode _mode);

        /// <summary>
        /// Sets the absolute look direction (world space vector) for the character to smoothly rotate towards.
        /// </summary>
        void LookDirection(Vector3 _dir);

        /// <summary>
        /// Sets the relative look direction (Euler angles in degrees) for the character to rotate by.
        /// </summary>
        void LookDirectionRelative(Vector2 _angles);

        /// <summary>
        /// Assign view to controller.
        /// </summary>
        void AttachVisual(ICharacterControllerView _view);

        /// <summary>
        /// Assign config. Should only be called before controller is initialized.
        /// </summary>
        void SetupConfig(ConfigCharacterController _config);

        /// <summary>
        /// Returns visual part of character controller if assigned.
        /// </summary>
        ICharacterControllerView GetView();
    }

    public enum NavigationMode
    {
        DirectControl = 0,
        Navmesh
    }
}
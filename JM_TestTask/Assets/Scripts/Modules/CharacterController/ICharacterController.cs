using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.CharacterController_Public
{
    public interface ICharacterController : IModuleInit, IModuleUpdate
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
        void ToggleNavigationMode(NavigationMode _mode);

        /// <summary>
        /// Sets the look direction for the character to smoothly rotate towards.
        /// </summary>
        void LookDirection(Vector3 _dir);
    }

    public enum NavigationMode
    {
        DirectControl = 0,
        Navmesh
    }

    public class CharacterControllerException : System.Exception
    {
        public CharacterControllerException(string _message) : base(_message) { }
    }
}
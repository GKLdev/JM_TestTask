using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.InputManager_Public
{
    public interface IInputManager : IModuleInit, IModuleUpdate
    {
        void SetContext(InputContext _context);
    }

    public enum InputContext
    {
        Undef = 0,
        World,
        UI
    }
}
using Modules.CharacterFacade_Public;
using Modules.CharacterManager_Public;
using Modules.InputManager_Public;
using Modules.ModuleManager_Public;
using Modules.PlayerWeapon_Public;
using Modules.UI.UIController_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Modules.InputManager
{
    public class InputManager : LogicBase, IInputManager
    {
        [Inject]
        private IModuleManager moduleMgr;

        [SerializeField]
        private State state = new();

        // *****************************
        // InitModule
        // *****************************
        public void InitModule()
        {
            state.dynamicData.characterMgr  = moduleMgr.Container.Resolve<ICharacterManager>();
            state.dynamicData.ui            = moduleMgr.Container.Resolve<IUIController>();
            state.dynamicData.moduleMgr     = moduleMgr;
            CompInit.Init(state);
        }

        // *****************************
        // OnUpdate
        // *****************************
        public void OnUpdate()
        {
            if (!state.dynamicData.isInitialized)
            {
                return;
            }

            CompUpdate.OnUpdate(state);
        }

        // *****************************
        // SetContext
        // *****************************
        public void SetContext(InputContext _context)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.isInitialized);

            CompContextHandler.SetContext(state, _context);
        }
    }

    [System.Serializable]
    public class State
    {
        public float mouseSensitivity = 2f;

        public DynamicData dynamicData = new();

        public class DynamicData
        {
            public bool isInitialized = false;
            public InputContext currentContext = InputContext.Undef;
            public ICharacterFacade player;
            public IPlayerWeapon    playerWeapon;

            public IModuleManager       moduleMgr;
            public ICharacterManager    characterMgr;
            public IUIController        ui;
        }
    }
}

using GDTUtils;
using Modules.AISensors_Public;
using Modules.CharacterFacade_Public;
using Modules.ModuleManager_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Modules.AISensors_Public
{
    public abstract class AISensorBase : LogicBase, IAISensor
    {
        [Inject]
        protected IModuleManager        moduleMgr;
        protected ICharacterFacade      characterFacade;

        private bool initialized    = false;
        private bool isDisposed     = false;

        // *****************************
        // InitModule
        // *****************************
        public void InitModule(ICharacterFacade _facade)
        {
            if (initialized)
            {
                return;
            }

            OnInit();
            initialized = true;
        }

        // *****************************
        // OnUpdate
        // *****************************
        public void OnUpdate(float _float)
        {
            if (initialized)
            {
                return;
            }

            OnSensorUpdate();
        }

        // *****************************
        // ResetSensor
        // *****************************
        public void ResetSensor()
        {
            OnReset();
        }

        // *****************************
        // OnInit
        // *****************************
        protected virtual void OnInit()
        {

        }

        // *****************************
        // OnSensorUpdate
        // *****************************
        protected virtual void OnSensorUpdate()
        {

        }

        // *****************************
        // OnReset
        // *****************************
        protected virtual void OnReset()
        {

        }

        // *****************************
        // Dispose
        // *****************************
        public void Dispose()
        {
            if (isDisposed)
            {
                return;
            }

            isDisposed = true;
            OnDispose();
        }

        // *****************************
        // OnDispose
        // *****************************
        protected virtual void OnDispose()
        {

        }
    }
}
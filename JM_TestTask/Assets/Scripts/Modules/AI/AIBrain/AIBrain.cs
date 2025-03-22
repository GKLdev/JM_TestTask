using GDTUtils;
using Modules.AIBrain_Public;
using Modules.AIManager;
using Modules.AIManager_Public;
using Modules.AISensors_Public;
using Modules.AITemplate_Public;
using Modules.CharacterFacade_Public;
using Modules.CharacterManager;
using Modules.CharacterManager_Public;
using Modules.Controllable_Public;
using Modules.ModuleManager_Public;
using Modules.ReferenceDb_Public;
using Modules.TimeManager_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static GDTUtils.Collision.CollisionResolveData;

namespace Modules.AIBrain
{
    public class AIBrain : LogicBase, IAIBrain
    {
        public SerializedInterface<IControllable>       controllable;
        public ScriptableObject                         template;

        [SerializeField]
        List<SensorContainer> sensors = new();

        [Inject]
        IModuleManager  moduleMgr;

        [SerializeField]
        State state;

        private bool disposed = false;

        // *****************************
        // InitModule
        // *****************************
        // TODO: support runtime template setup( register at manager e t c)
        public void InitModule(ICharacterFacade _facade)
        {
            if (state.initialized)
            {
                return;
            }

            state.dynamic.timeMgr       = moduleMgr.Container.TryResolve<ITimeManager>();
            state.dynamic.aiManager     = moduleMgr.Container.TryResolve<IAIManager>();
            //state.dynamic.aiManager.Register(this);

            foreach (var sensorContainer in sensors)
            {
                state.dynamic.aliasToSensor.Add(sensorContainer.alias, sensorContainer.sensor.Value);
                sensorContainer.sensor.Value.InitModule(_facade);
            }

            state.dynamic.facade    = _facade;

            if (template == null)
            {
                state.dynamic.emptyTemplateMode = true;
            }
            else
            {
                template                        = ScriptableObject.Instantiate(template);
                state.dynamic.templateCasted    = (template as IAITemplate)?.Clone();
                state.dynamic.templateCasted.Init(this, state.dynamic.facade, moduleMgr.Container);
            }

            state.initialized       = true;
        }

        // *****************************
        // SetupTemplate
        // *****************************
        public void SetupTemplate(IAITemplate _template)
        {
            if (state.initialized)
            {
                Debug.Assert(false, "Cant assign AI Template to AIBrain when its already initialized");
            }

            template = _template as ScriptableObject;
        }

        // *****************************
        // OnUpdate
        // *****************************
        public void OnUpdate()
        {
            bool ignore = !state.initialized || !state.dynamic.isActive || state.dynamic.emptyTemplateMode;
            if (ignore)
            {
                return;
            }

            UpdateDeltaTime();

            foreach (var pair in state.dynamic.aliasToSensor)
            {
                pair.Value.OnUpdate(state.dynamic.deltaTime);
            }

            state.dynamic.templateCasted.UpdateTemplate(state.dynamic.deltaTime);
        }

        // *****************************
        // ToggleAIBrain
        // *****************************
        public void ToggleAIBrain(bool _val)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.initialized);

            if (state.dynamic.emptyTemplateMode)
            {
                return;
            }

            state.dynamic.isActive = _val;
            state.dynamic.templateCasted.ToggleActive(_val);
        }

        // *****************************
        // UpdateDeltaTime
        // *****************************
        private void UpdateDeltaTime()
        {
            state.dynamic.deltaTime = state.dynamic.timeMgr.GetDeltaTime(state.timeLayerType);
        }

        // *****************************
        // GetSensor
        // *****************************
        public bool GetSensor<T>(string _alias, out T sensor)
            where T : class, IAISensor
        {
            bool result = false;

            result = state.dynamic.aliasToSensor.TryGetValue(_alias, out IAISensor tempSensor);
            sensor = tempSensor as T;

            if (sensor == null)
            {
                Debug.Assert(false, $"AIBrain={gameObject} failed to get sensor of type={typeof(T)} from type={tempSensor.GetType()}");
            }

            return result;
        }

        // *****************************
        // OnAdded
        // *****************************
        public void OnAdded()
        {
        }

        // *****************************
        // OnAwake
        // *****************************
        public void OnAwake()
        {
            if (state.dynamic.emptyTemplateMode)
            {
                return;
            }

            state.dynamic.aiManager.Register(this);
        }

        // *****************************
        // OnSlept
        // *****************************
        public void OnSlept()
        {
            if (state.dynamic.emptyTemplateMode)
            {
                return;
            }

            state.dynamic.aiManager.Unregister(this);
            ToggleAIBrain(false);
            state.dynamic.templateCasted.ResetTemplate();
        }

        // *****************************
        // Dispose
        // *****************************
        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            disposed = true;

            if (state.dynamic.emptyTemplateMode)
            {
                return;
            }

            state.dynamic.aiManager.Unregister(this);
            state.dynamic.templateCasted = null;
        }

        [System.Serializable]
        public class SensorContainer
        {
            public string                           alias;
            public SerializedInterface<IAISensor>   sensor;
        }
    }

    // *****************************
    // State
    // *****************************
    [System.Serializable]
    public class State
    {
        public bool             initialized = false;
        public TimeLayerType    timeLayerType;

        public DynamicData  dynamic     = new();

        // *****************************
        // DynamicData
        // *****************************
        [System.Serializable]
        public class DynamicData
        {
            public bool     isActive    = false;
            public float    deltaTime   = 0f;

            public Dictionary<string, IAISensor> aliasToSensor = new();
            public ICharacterFacade facade;

            public ITimeManager timeMgr;
            public IAIManager   aiManager;

            public IAITemplate templateCasted;
            public bool emptyTemplateMode = false;
        }
    }
}
using GDTUtils;
using Modules.AISensors_Public;
using Modules.AITemplate_Public;
using Modules.CharacterFacade_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Modules.AIBrain_Public
{
    public interface IAIBrain : IModuleUpdate, ICharacterFacadeCallbacks
    {
        void SetupTemplate(IAITemplate _template);
        void InitModule(ICharacterFacade _facade);
        bool GetSensor<T>(string _alias, out T sensor) where T : class, IAISensor;
        void ToggleAIBrain(bool _val);
    }
}
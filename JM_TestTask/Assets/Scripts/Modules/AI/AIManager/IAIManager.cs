using Modules.AIBrain_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.AIManager_Public
{
    public interface IAIManager : IModuleInit, IModuleUpdate
    {
        void Register(IAIBrain _brain);
        void Unregister(IAIBrain _brain);
    }
}
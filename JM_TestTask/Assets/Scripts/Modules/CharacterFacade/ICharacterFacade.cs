using GDTUtils;
using GDTUtils.Common;
using GDTUtils.Patterns.Factory;
using Modules.AITemplate_Public;
using Modules.CharacterController_Public;
using Modules.CharacterControllerView_Public;
using Modules.DamageManager_Public;
using Modules.ReferenceDb_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.CharacterFacade_Public
{
    public interface ICharacterFacade : 
        IModuleUpdate, 
        IFactoryProduct,
        IPoolable,
        IElementTypeAccess<CATEGORY_CHARACTERS>,
        IGameObjectAccess
    {
        ICharacterController        P_Controller { get; }

        void SetupCharacter(ConfigCharacterController _configController, ConfigDamageable _configDamageable, IAITemplate _template = null);
        void InitModule(ICharacterControllerView _visual);
        DamageSource GetDamageSource();     
        void MakeAIControlled();
        void MakePlayerControlled();
        bool IsDead();
    }
}
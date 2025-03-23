using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.PlayerWeapon
{
    public static class CompInit
    {
        // *****************************
        // Init
        // *****************************
        public static void Init(State _state, PlayerWeapon _self)
        {
            // Validate essential references
            if (_state.shootPointTransform == null || _state.config == null)
            {
                Debug.Assert(false, "PlayerWeapon: shootPointTransform or config is not assigned.");
                return;
            }

            // Damage
            _state.dynamicData.damageSource.faction = _state.config.P_Faction;
            _state.dynamicData.damageSource.obj     = _self;
            _state.dynamicData.currentDamageValue   = _state.config.P_Damage; 

            // Cooldown
            _state.dynamicData.shotCooldown = _state.dynamicData.timeMgr.AddCooldown(_state.config.P_DelayBetweenShots, _state.config.P_TimeLayer);
            _state.statsSystem.Value.InitModule();
            _state.statsSystem.Value.Toggle(true);
            _state.dynamicData.isInitialized = true;
        }
    }
}

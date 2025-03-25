using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.PlayerWeapon
{
    public static class CompShoot
    {
        // *****************************
        // OnUpdate
        // *****************************
        public static void OnUpdate(State _state)
        {
            // No update logic required for now
        }

        // *****************************
        // Shoot
        // *****************************
        public static void Shoot(State _state)
        {
            bool ignore = _state.dynamicData.timeMgr.CheckCooldownIsRunning(_state.dynamicData.shotCooldown);
            if (ignore)
            {
                return;
            }

            // Start cooldown
            _state.dynamicData.timeMgr.ResetCooldown(_state.dynamicData.shotCooldown);

            // Perform raycast from the shoot point
            RaycastHit hit;
            bool isHit = Physics.Raycast(
                _state.shootPointTransform.position,
                _state.shootPointTransform.forward,
                out hit,
                _state.config.P_MaxDistance,
                _state.config.P_TargetLayerMask
            );

            // Hit logic
            if (!isHit)
            {
                return;
            }

            var cdt         = hit.collider;
            var damageable  = _state.dynamicData.damageMgr.GetDamageableByCollision(cdt);
            if (damageable == null)
            {
                return;
            }

            Debug.DrawLine(_state.shootPointTransform.position, hit.point, Color.green, 0.5f);

            _state.dynamicData.currentDamageValue = _state.config.P_Damage + _state.dynamicData.currentDamageMod;
            damageable.OnDamage(_state.dynamicData.damageSource, DamageManager_Public.DamageType.Physical, _state.dynamicData.currentDamageValue);
        }
    }
}

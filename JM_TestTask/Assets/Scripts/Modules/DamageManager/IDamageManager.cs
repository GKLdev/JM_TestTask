using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.DamageManager_Public
{
    public interface IDamageManager : IModuleInit, IModuleUpdate, IDisposable
    {
        /// <summary>
        /// Register to damage manager.
        /// </summary>
        /// <param name="_component"></param>
        /// <param name="_collider"></param>
        void RegisterDamageable(IDamageable _component, Collider _collider);

        /// <summary>
        /// Unbregister from manager.
        /// </summary>
        /// <param name="_component"></param>
        void UnregisterDamageable(IDamageable _component);

        /// <summary>
        /// Get registered damageble object by a collider its tied to.
        /// </summary>
        /// <param name="_collider"></param>
        /// <returns></returns>
        IDamageable GetDamageableByCollision(Collider _collider);
    }
}
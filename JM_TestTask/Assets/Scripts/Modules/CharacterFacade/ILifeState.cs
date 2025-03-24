using Modules.DamageManager_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.CharacterFacade_Public
{
    public interface ILifeState
    {
        void OnDeath();
        void OnDamage(IDamageable _damageable);
    }
}
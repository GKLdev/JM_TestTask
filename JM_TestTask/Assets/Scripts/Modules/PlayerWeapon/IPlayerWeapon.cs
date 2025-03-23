using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.PlayerWeapon_Public
{
    public interface IPlayerWeapon : IModuleInit, IModuleUpdate
    {
        void Shoot();
    }
}
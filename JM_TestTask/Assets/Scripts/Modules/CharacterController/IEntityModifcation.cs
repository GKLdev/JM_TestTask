using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.CharacterController_Public
{
    public interface IEntityModifcation
    {
        void ModifyEntity(EntityModifcationType _stat, float _value);
    }

    public enum EntityModifcationType
    {
        Health,
        Speed,
        Damage
    }
}
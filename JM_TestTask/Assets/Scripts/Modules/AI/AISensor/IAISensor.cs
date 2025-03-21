using Modules.CharacterFacade_Public;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.AISensors_Public
{
    public interface IAISensor : IDisposable
    {
        void InitModule(ICharacterFacade _facade);
        void OnUpdate(float _float);
    }
}
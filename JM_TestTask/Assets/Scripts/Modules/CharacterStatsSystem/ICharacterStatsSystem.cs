using Modules.CharacterFacade_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterStatsSystem : IModuleInit, ICharacterFacadeCallbacks
{
    void Toggle(bool _val);
}

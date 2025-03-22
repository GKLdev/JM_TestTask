using System.Collections;
using System.Collections.Generic;
using Modules.UI.UIController_Public;
using UnityEngine;
using UnityEngine.Serialization;

namespace Modules.UI.UIController
{
    [CreateAssetMenu(menuName = "Configs/UI/Base/ConfigUIScreens", fileName = "ConfigUIScreens")]
    public class ConfigUIScreens : ScriptableObject
    {
        public List<ScreenConfigContainer> screens = new();

        [System.Serializable]
        public class ScreenConfigContainer
        {
            public string    screenTypeName;
            public string    presenterInterfaceType;
            public string    presenterType;
            public LogicBase screenPrefab;
        }
    }
}
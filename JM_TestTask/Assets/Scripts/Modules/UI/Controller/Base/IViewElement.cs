using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.UI.UIController_Public
{ 
    public interface IViewElement : IModuleInit
    {
        void Show();
        void Hide();
    }
}
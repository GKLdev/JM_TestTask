using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.UI.UIController_Public
{
    public interface IScreenView : IDisposable
    {
        void Init(IScreenPresenter _presenter);
        void Show();
        void Hide();
    }
}
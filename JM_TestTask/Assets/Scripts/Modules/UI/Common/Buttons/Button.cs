using System;
using System.Collections;
using System.Collections.Generic;
using Modules.UI.Button_Public;
using UnityEngine;

namespace Modules.UI.Common
{
    public class Button<TResponse, TInput> : LogicBase 
        where TResponse : ButtonResponseDataBase
        where TInput : ButtonInputDataBase
    {
        public GameObject P_GameObjectAccess => gameObject;
        
        public Action<TResponse> P_ButtonCallback { get; set; }
        public bool P_ButtonIsLocked { get; set; }
        
        protected TResponse responseData;
        
        // *****************************
        // SetResponseData
        // *****************************
        public void SetResponseData(TResponse _responseData)
        {
            responseData = _responseData;
        }
        
        // *****************************
        // SetData
        // *****************************
        public virtual void SetData(TInput _data)
        {
            
        }
        
        // *****************************
        // OnClicked
        // *****************************
        public void OnClicked()
        {
            bool skip = P_ButtonIsLocked;
            if (skip)
            {
                return;
            }
            
            P_ButtonCallback?.Invoke(responseData);
        }
    }
}

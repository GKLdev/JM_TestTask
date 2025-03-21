using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.AIManager
{
    public static class CompUpdate
    {
        // *****************************
        // Update 
        // *****************************
        public static void Update(State _state)
        {
            UpdateBrains(_state);
            HandleQueues(_state);
        }

        // *****************************
        // UpdateBrains 
        // *****************************
        static void UpdateBrains(State _state)
        {
            foreach (var item in _state.dynamic.updateList)
            {
                item.OnUpdate();
            }
        }

        // *****************************
        // HandleQueues 
        // *****************************
        static void HandleQueues(State _state)
        {
            if (_state.dynamic.addQueueTriggered)
            {
                _state.dynamic.addQueueTriggered = false;

                foreach (var item in _state.dynamic.addQueue)
                {
                    _state.dynamic.updateList.Add(item);
                }

                _state.dynamic.addQueue.Clear();
            }

            if (_state.dynamic.removeQueueTriggered)
            {
                _state.dynamic.removeQueueTriggered = false;

                foreach (var item in _state.dynamic.removeQueue)
                {
                    _state.dynamic.updateList.Remove(item);
                }

                _state.dynamic.removeQueue.Clear();
            }
        }
    }
}
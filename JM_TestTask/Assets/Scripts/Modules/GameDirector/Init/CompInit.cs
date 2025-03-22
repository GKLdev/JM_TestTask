using GDTUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Modules.GameDirector
{
    public static class CompInit
    {
        // *****************************
        // Init
        // *****************************
        public static void Init(State _state, DiContainer _container)
        {
            // Create state machine
             StateMachineFactory<GameDirectorNodeType, State> stateMachineFactory = new();
            _state.dynamicData.stateMachine = stateMachineFactory.Produce();

            // Specify external reference
            _state.dynamicData.stateMachine.SpecifyReference(_state);
            _state.dynamicData.container = _container;

            // Add default node
            _state.dynamicData.stateMachine.AddNode(GameDirectorNodeType.Null, new SM_NullState<GameDirectorNodeType, State>());
            _state.dynamicData.stateMachine.AddNode(GameDirectorNodeType.SimpleGameplayTest, new SimpleGameplayTest());

            // Transition to the default state
            _state.dynamicData.stateMachine.TransitionTo(GameDirectorNodeType.SimpleGameplayTest);

            // Mark module as initialized
            _state.dynamicData.isInitialized = true;
        }
    }
}

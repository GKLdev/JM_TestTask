using System;
using System.Collections;
using System.Collections.Generic;
using GDTUtils.Extensions;
using Modules.UI.UIController_Public;
using UnityEngine;
using static Modules.UI.UIController.State.DynamicData;

namespace Modules.UI.UIController
{
    public static class CompScreenVisibility
    {
        // *****************************
        // ShowScreen
        // *****************************
        public static void ShowScreen(State _state, ScreenType _screenType, bool _hideOthers)
        {
            bool error = !_state.dynamicData.dictScreens.TryGetValue(_screenType, out State.DynamicData.ScreenContainer screenContainer);
            if (error)
            {
                throw new System.Exception($"ShowScreen: screen={_screenType} not found!");
            }

            // try open existing screen
            bool screenInstantiated = screenContainer.ScreenIsUsable;
            if (screenInstantiated)
            {
                QueueShowScreen(screenContainer);
                TryCloseOthers();
                return;
            }

            // instantiate screen
            bool initPresenter = !screenContainer.PresenterLoaded;
            bool initView      = !screenContainer.ViewLoaded;

            SpawnScreen(_state, screenContainer, initPresenter, initView);
            
            // open screen
            QueueShowScreen(screenContainer);
            TryCloseOthers();

            void TryCloseOthers()
            {
                bool skip = !_hideOthers;
                if (skip)
                {
                    return;
                }

                HideAll(_state, false, _screenType);
            }
        }

        // *****************************
        // HideAll
        // *****************************
        public static void HideAll(State _state, bool _despawnViews = false, ScreenType _except = ScreenType.UNDEFINED)
        {
            _state.dynamicData.dictScreens.ForEach(x =>
            {
                bool skip = x.Key == _except;
                if (skip)
                {
                    return;
                }

                HideScreen(_state, x.Value.screenType, _despawnViews);
            });
        }

        // *****************************
        // SpawnScreen
        // *****************************
        public static void SpawnScreen(State _state, State.DynamicData.ScreenContainer _screenContainer, bool _instantiatePresenter, bool _instantiateView)
        {
            if (_instantiatePresenter)
            {
                InstantiatePresenter();
            }

            if (_instantiateView)
            {
                InstantiateView();
            }

            void InstantiatePresenter()
            {
                System.Type presenterType = Type.GetType(_screenContainer.config.presenterType);

                bool error = presenterType == null;
                if (error)
                {
                    throw new System.Exception($"Failed to init UI screen: No presenter type={_screenContainer.config.screenTypeName} found!");
                }

                var presenterInstance = _state.dynamicData.moduleManager.Container.Instantiate(presenterType);
                
                error = (presenterInstance as IScreenPresenter) == null;
                if (error)
                {
                    throw new System.Exception($"Failed to init UI screen: Presenter={presenterType} not found!");
                }

                _screenContainer.presenter = presenterInstance as IScreenPresenter;

                // bind to zenject
                System.Type presenterIfaceType = Type.GetType(_screenContainer.config.presenterInterfaceType);
                _state.dynamicData.moduleManager.Container.Bind(presenterIfaceType).FromInstance(presenterInstance).AsSingle().NonLazy();
            }

            void InstantiateView()
            {
                GameObject viewInstance =  _state.dynamicData.moduleManager.Container.InstantiatePrefab(_screenContainer.config.screenPrefab);
                viewInstance.transform.SetParent(_state.screensParent);
                
                viewInstance.TryGetComponent(typeof(IScreenView), out Component component);

                bool error = component == null;
                if (error)
                {
                    throw new System.Exception($"Failed to show UI screen: Component={Type.GetType(_screenContainer.config.presenterType)} not found!");
                }
                
                _screenContainer.screenView = component as IScreenView;
                _screenContainer.screenView.Init(_screenContainer.presenter);
                _screenContainer.presenter.Init(_screenContainer.screenView, _screenContainer.screenType);
            }
        }

        // *****************************
        // DespawnScreen
        // *****************************
        public static void DespawnScreen(State _state, State.DynamicData.ScreenContainer _screenContainer)
        {
            
        }

        // *****************************
        // HideScreen
        // *****************************
        public static void HideScreen(State _state, ScreenType _screenType, bool _despawnView = false)
        {
            bool error = !_state.dynamicData.dictScreens.TryGetValue(_screenType, out State.DynamicData.ScreenContainer screenContainer);
            if (error)
            {
                throw new System.Exception($"ShowScreen: screen={_screenType} not found!");
            }

            bool screenInstantiated = screenContainer.ScreenIsUsable;
            if (screenInstantiated)
            {
                if (_despawnView)
                {
                    screenContainer.viewMarkedForDespawn = true;
                }

                QueueHideScreen(screenContainer);
                return;
            }
        }

        // *****************************
        // QueueShowScreen
        // *****************************
        static void QueueShowScreen(State.DynamicData.ScreenContainer _screenContainer)
        {
            bool skip = _screenContainer.screenState == State.DynamicData.ScreenState.Shown || _screenContainer.screenState == State.DynamicData.ScreenState.Transition;
            if (skip)
            {
                Debug.LogWarning($"Trying to show screen={_screenContainer.screenType} while in transition");
                return;
            }
            
            _screenContainer.screenState = State.DynamicData.ScreenState.Transition;
            _screenContainer.presenter.Show();
        }

        // *****************************
        // QueueHideScreen
        // *****************************
        static void QueueHideScreen(State.DynamicData.ScreenContainer _screenContainer)
        {
            bool skip = _screenContainer.screenState == State.DynamicData.ScreenState.Hidden || _screenContainer.screenState == State.DynamicData.ScreenState.Transition;
            if (skip)
            {
                Debug.LogWarning($"Trying to hide screen={_screenContainer.screenType} while hidden or in transition");
                return;
            }
            
            _screenContainer.screenState = State.DynamicData.ScreenState.Transition;
            _screenContainer.presenter.Hide();
        }

        // *****************************
        // QueueHideScreen
        // *****************************
        public static void OnScreenShown(State _state, ScreenType _screenType)
        {
            _state.dynamicData.dictScreens[_screenType].screenState = State.DynamicData.ScreenState.Shown;
        }

        // *****************************
        // OnScreenHidden
        // *****************************
        public static void OnScreenHidden(State _state, ScreenType _screenType)
        {
            var container = _state.dynamicData.dictScreens[_screenType];

            bool markedForDespawn = container.viewMarkedForDespawn;
            if (markedForDespawn)
            {
                container.viewMarkedForDespawn = false;
                container.screenView.Dispose();
                container.screenView = null;
                container.screenState = State.DynamicData.ScreenState.Undefined;
                return;
            }
            
            container.screenState = State.DynamicData.ScreenState.Hidden;
        }
    }
}
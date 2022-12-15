﻿using System;
using UniRx;
using UnityEngine;

namespace MornLib.Scenes
{
    public abstract class MornSceneMonoBase<TEnum> : MonoBehaviour where TEnum : Enum
    {
        [SerializeField] private TEnum _sceneType;
        private readonly Subject<TEnum> _loadSceneSubject = new();
        private readonly Subject<TEnum> _addSceneSubject = new();
        private readonly Subject<TEnum> _removeSceneSubject = new();
        public TEnum SceneType => _sceneType;
        public IObservable<TEnum> OnLoadScene => _loadSceneSubject;
        public IObservable<TEnum> OnAddScene => _addSceneSubject;
        public IObservable<TEnum> OnRemoveScene => _removeSceneSubject;
        public abstract void MyAwake();

        protected void LoadScene(TEnum sceneType)
        {
            _loadSceneSubject.OnNext(sceneType);
        }

        protected void AddScene(TEnum sceneType)
        {
            _addSceneSubject.OnNext(sceneType);
        }

        protected void RemoveScene(TEnum sceneType)
        {
            _removeSceneSubject.OnNext(sceneType);
        }

        public abstract void OnEnterScene();
        public abstract void SceneUpdate();
        public abstract void OnExitScene();
    }
}

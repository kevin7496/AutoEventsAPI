using Exiled.API.Features;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AutoEvents.Features
{
    public class CoroutineEvent : MonoBehaviour
    {
        public bool IsRunning => TargetEvent != null;
        public AutoEvent TargetEvent { get; private set; }

        private MethodInfo _coroutineMethod;

        public delegate void OnEmergencyStopEvent();
        public OnEmergencyStopEvent EmergencyStopEvent;

        private void Awake()
        {
            Log.Info($"{SceneManager.GetActiveScene().name} | {SceneManager.GetActiveScene().buildIndex}");
        }

        private void Update()
        {
            if (!IsRunning) return;

            try
            {
                _coroutineMethod?.Invoke(TargetEvent, null);
            }catch (Exception ex)
            {
                AutoEventManager.StopEvent();
                Log.Error($"Получена ошибка в авто ивенте \"{TargetEvent.GetType().Name}\": \n {ex}");
            }
            

        }

        public void SetTargetEvent(AutoEvent autoEvent)
        {
            if (autoEvent == null) return;
            if (TargetEvent == null)
            {
                TargetEvent = autoEvent;
                _coroutineMethod = TargetEvent.GetType().GetMethod("Update", BindingFlags.Instance | BindingFlags.NonPublic);
            }
        }

        public void RemoveTargetEvent()
        {
            TargetEvent = null;
            _coroutineMethod = null;
        }
    }
}

using Exiled.API.Features;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AutoEvents.Features
{
    public static class AutoEventManager
    {
        private static Type[] _typeEvents;
        private static GameObject _gameObject;
        private static CoroutineEvent _coroutineEvent;

        internal static void Awake()
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            _typeEvents = types.Where(x => x.IsSubclassOf(typeof(AutoEvent))).ToArray();

            for (int i = 0; i < _typeEvents.Length; i++)
            {
                AutoEvent autoEvent = Activator.CreateInstance(_typeEvents[i]) as AutoEvent;
                Commands.List.EventsList += $"\n[{i}] Название: {autoEvent.Name}\n\tОписание: {autoEvent.Description}";
            }

            Exiled.Events.Handlers.Server.WaitingForPlayers += RoundStarted;
        }

        private static void RoundStarted()
        {
            _gameObject = new GameObject("CoroutineEvent");
            _gameObject.AddComponent<CoroutineEvent>();
            _coroutineEvent = _gameObject.GetComponent<CoroutineEvent>();

            Exiled.Events.Handlers.Server.RoundStarted -= RoundStarted;
        }

        public static void StartEvent(int id)
        {
            if (!_typeEvents.TryGet(id, out Type type))
            {
                return;
            }

            if (_coroutineEvent.IsRunning)
            {
                return;
            }

            AutoEvent aevent = Activator.CreateInstance(type) as AutoEvent;

            if (aevent.MinPlayers > Player.List.Count)
            {
                return;
            }

            aevent.Start();
            _coroutineEvent.SetTargetEvent(aevent);
        }

        public static void StopEvent()
        {
            if (!_coroutineEvent.IsRunning)
            {
                return;
            }

            _coroutineEvent.TargetEvent.Stop();
            _coroutineEvent.RemoveTargetEvent();
        }



        public static bool StartEvent(int id, out string response)
        {
            if (!_typeEvents.TryGet(id, out Type type))
            {
                response = "Ивент с таким айди не найден!";
                return false;
            }

            if (_coroutineEvent.IsRunning)
            {
                response = "Ивент уже запущен!";
                return false;
            }

            AutoEvent aevent = Activator.CreateInstance(type) as AutoEvent;

            if (aevent.MinPlayers > Player.List.Count)
            {
                response = "Недостаточно игроков";
                return false;
            }

            aevent.Start();
            _coroutineEvent.SetTargetEvent(aevent);

            response = "Ивент запущен.";
            return true;
        }

        public static bool StopEvent(out string response)
        {
            if (!_coroutineEvent.IsRunning)
            {
                response = "Сейчас ивент не запущен!";
                return false;
            }

            _coroutineEvent.TargetEvent.Stop();
            _coroutineEvent.RemoveTargetEvent();

            response = "Ивент остановлен!";
            return true;
        }

    }
}

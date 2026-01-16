using System;

namespace AutoEvents
{
    /// <summary>
    /// Base class for auto events
    /// </summary>
    public abstract class AutoEvent
    {
        /// <summary>
        /// Name event
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Description event
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Minimum number of players to start the event
        /// </summary>
        public abstract uint MinPlayers { get; }

        /// <summary>
        /// Вызов метода при запуске ивента
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Calling a method when an event is stopped
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Calling a method when an event is triggered
        /// </summary>
        protected abstract void Update();
    }
}

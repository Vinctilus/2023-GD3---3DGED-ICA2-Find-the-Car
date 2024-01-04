using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GD
{
    public enum GameEventOrder
    {
        First = 80,
        Early = 90,
        Normal = 100, //0 based would have been nicer, but enum get compared as unsigned
        Late = 110,
        Last = 120
    }
    public abstract class BaseGameEventListener<T> : MonoBehaviour
    {
        [SerializeField]
        private string description;

        [SerializeField]
        [Tooltip("This specifies the relative order in which multiple event listeners on one event will be called")]
        public GameEventOrder order = GameEventOrder.Normal;
        
        [SerializeField]
        [Tooltip("Specify the game event (scriptable object) which will raise the event")]
        private BaseGameEvent<T> Event;  //read event + delegate -> GDEvent

        [SerializeField]
        private UnityEvent<T> Response;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public virtual void OnEventRaised(T data)
        {
            Response?.Invoke(data);
        }
    }
}
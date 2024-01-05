using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace GD
{
    public abstract class BaseGameEvent<T> : ScriptableObject
    {
        //this uses a special comparer that see's equal keys as being different to enable duplicated GameEventOrder's
        //another possible way would have been to implement a sorted insert into a normal list
        private SortedList<GameEventOrder, BaseGameEventListener<T>> listeners = new(new DuplicateKeyComparer<GameEventOrder>());

        //display all registered listeners in the inspector for debugging events
        [ShowInInspector] [ReadOnly]
        private IList<BaseGameEventListener<T>> ListenerValues => listeners.Values;

        [ShowInInspector] [ReadOnly]
        private IList<GameEventOrder> ListenerOrders => listeners.Keys;
        
        [ContextMenu("Raise Event")]
        public virtual void Raise(T data)
        {
            //getting an immutable list which is sorted because of the underlying data type. It makes sure the listeners don't change in this small timeframe
            var immutableList = listeners.ToImmutableList();
            foreach(KeyValuePair<GameEventOrder, BaseGameEventListener<T>> kv in immutableList)
            {
                kv.Value.OnEventRaised(data);
            }
        }

        public void RegisterListener(BaseGameEventListener<T> listener)
        {
            if (!listeners.ContainsValue(listener))
                listeners.Add(listener.order, listener);
        }

        public void UnregisterListener(BaseGameEventListener<T> listener)
        {
            int indexToRemove = listeners.IndexOfValue(listener);
            if(indexToRemove != -1)
                listeners.RemoveAt(indexToRemove);
        }
    }
}
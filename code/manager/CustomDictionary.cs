﻿/*******************
* Rudolf Chrispens *
*******************/

#region using
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#endregion

namespace manager.ioc
{
    /// <summary>
    /// Dictionary that is Unity serializable.
    /// Simply two lists. Missing some common methods.
    /// </summary>
    /// <typeparam name="Key"></typeparam>
    /// <typeparam name="Value"></typeparam>
    [System.Serializable]
    public class CustomDict<Key, Value> : IEnumerable<Value>
    {
        [SerializeField]
        protected List<Key> keys = new List<Key>();

        [SerializeField]
        protected List<Value> values = new List<Value>();

        public int Count
        {
            get { return count(); }
        }

        public List<Key> Keys
        {
            get { return keys; }
        }

        public List<Value> Values
        {
            get { return values; }
        }

        public IEnumerator<Value> GetEnumerator()
        {
            return values.GetEnumerator(); ;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool ContainsKey(Key key)
        {
            if (keys.Contains(key))
                return true;
            else
                return false;
        }

        public void Add(Key key, Value value)
        {
            if (keys.Contains(key))
            {
                Debug.LogError("Dictionary already contains key! " + key.ToString());
                return;
            }

            keys.Add(key);
            values.Add(value);
        }

        public void AddOrReplace(Key key, Value value)
        {
            if (!keys.Contains(key))
            {
                int index = keys.IndexOf(key);
                keys.RemoveAt(index);
                values.RemoveAt(index);
            }

            keys.Add(key);
            values.Add(value);
        }

        public void Clear()
        {
            keys.Clear();
            values.Clear();
        }

        public void Remove(Key key)
        {
            if (!keys.Contains(key))
                return;

            int index = keys.IndexOf(key);

            keys.RemoveAt(index);
            values.RemoveAt(index);
        }

        public bool TryGetValue(Key key, out Value value)
        {
            if (keys.Count != values.Count)
            {
                Debug.LogError("Dictionary already contains key! " + key.ToString());
                value = default(Value);
                return false;
            }

            if (!keys.Contains(key))
            {
                value = default(Value);
                return false;
            }

            int index = keys.IndexOf(key);
            value = values[index];

            return true;
        }

        protected int count()
        {
            if(keys.Count != values.Count)
                Debug.LogError("Error " + this.ToString() + " does not have the same key and value count! " + keys.Count + " notEquals " + values.Count );

            return values.Count;
        }

        public void set(Key key, Value value)
        {
            if (keys.Contains(key))
            {
                int index = keys.IndexOf(key);
                values[index] = value;
            }
            else
            {
                Add(key, value);
            }
        }
    }
}
//namespace end
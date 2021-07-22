using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class SerializableDictionary { }

[Serializable]
public class SerializableDictionary<TKey, TValue> : SerializableDictionary, ISerializationCallbackReceiver, IDictionary<TKey, TValue>
{
    [Serializable]
    public struct SerializableKeyValuePair
    {
        public TKey Key;
        public TValue Value;

        public SerializableKeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public void SetValue(TValue value)
        {
            Value = value;
        }
    }

    [SerializeField]
    private List<SerializableKeyValuePair> list = new List<SerializableKeyValuePair>();

    public ICollection<TKey> Keys
    {
        get
        {
            return list.Select(kvp => kvp.Key).ToList();
        }
    }

    public ICollection<TValue> Values
    {
        get
        {
            return list.Select(kvp => kvp.Value).ToList();
        }
    }

    public SerializableDictionary()
    {
    }
    
    public void OnBeforeSerialize()
    {

    }

    public void OnAfterDeserialize()
    {
      
    }

    public TValue this[TKey key]
    {
        get => GetValue(key);
        set
        {
            if (ContainsKey(key))
                Remove(key);

            Add(key, value);
        }
    }

    public bool ContainsKey(TKey key)
    {
        return list.Any(kvp => kvp.Key.Equals(key));
    }

    public bool ContainsValue(TValue value)
    {
        var equalityComparer = EqualityComparer<TValue>.Default;
        return list.Any(kvp => equalityComparer.Equals(kvp.Value, value));
    }
    
    public void Add(TKey key, TValue value)
    {
        list.Add(new SerializableKeyValuePair(key, value));
        Keys.Add(key);
    }

    public bool Remove(TKey key)
    {
        var returnValue = ContainsKey(key);
        list = new List<SerializableKeyValuePair>(list.ToArray().Where(x => !x.Key.Equals(key)));
        Keys.Remove(key);
        return returnValue;
    }

    public bool Replace(TKey key, TValue value)
    {
        var returnValue = ContainsKey(key);
        for (int i = 0; i < list.Count; i++)
        {
            var pair = list[i];

            if (!pair.Key.Equals(key)) 
                continue;

            list[i] = new SerializableKeyValuePair(pair.Key, value);
            break;
        }

        return returnValue;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        foreach (var kvp in list.Where(kvp => kvp.Key.Equals(key)))
        {
            value = kvp.Value;
            return true;
        }

        value = default;
        return false;
    }

    public TValue GetValue(TKey key)
    {
        foreach (var kvp in list.Where(kvp => kvp.Key.Equals(key)))
        {
            return kvp.Value;
        }

        return default;
    }

    public int Count => list.Count;
    public bool IsReadOnly => false;

    public void Add(KeyValuePair<TKey, TValue> kvp)
    {
        Add(kvp.Key, kvp.Value);
    }

    public void Clear()
    {
        list.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return (from kvp in list where kvp.Key.Equals(item.Key) select EqualityComparer<TValue>.Default.Equals(kvp.Value, item.Value)).FirstOrDefault();
    }

    public bool Remove(KeyValuePair<TKey, TValue> kvp)
    {
        return Remove(kvp.Key);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        var numKeys = list.Count;

        if (array.Length - arrayIndex < numKeys)
            throw new ArgumentException("arrayIndex");

        for (int i = 0; i < numKeys; i++, arrayIndex++)
        {
            var entry = list[i];
            array[arrayIndex] = new KeyValuePair<TKey, TValue>(entry.Key, entry.Value);
        }
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return list.Select(ToKeyValuePair).GetEnumerator();

        static KeyValuePair<TKey, TValue> ToKeyValuePair(SerializableKeyValuePair skvp)
        {
            return new KeyValuePair<TKey, TValue>(skvp.Key, skvp.Value);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

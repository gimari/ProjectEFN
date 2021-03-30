﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue> : SortedDictionary<TKey, TValue>, ISerializationCallbackReceiver {

    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    /// <summary>
    /// save the dictionary to lists
    /// </summary>
    public void OnBeforeSerialize() {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this) {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    /// <summary>
    /// load dictionary from lists
    /// </summary>
    public void OnAfterDeserialize() {
        this.Clear();

        if (keys.Count != values.Count)
            throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

        for (int i = 0; i < keys.Count; i++)
            this.Add(keys[i], values[i]);
    }
}
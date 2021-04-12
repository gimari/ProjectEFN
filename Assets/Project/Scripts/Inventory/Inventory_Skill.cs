using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {
    public class Inventory_Skill : ISerializationCallbackReceiver {

        /// <summary>
        /// serialize 저장 전용 리스트
        /// </summary>
        [SerializeField] private List<Data_Skill> _serializedList = new List<Data_Skill>();

        [NonSerialized] protected Dictionary<eSkillType, Data_Skill> _inventoryList = new Dictionary<eSkillType, Data_Skill>();

        public virtual string ToJson() {
            return JsonUtility.ToJson(this);
        }

        public IEnumerator GetEnumerator() {
            return _inventoryList.GetEnumerator();
        }

        public int Get(eSkillType idx) {

            if (false == _inventoryList.ContainsKey(idx)) {
                return 0;
            }

            return _inventoryList[idx].Level;
        }

        public void Add(eSkillType idx) {
            if (false == _inventoryList.ContainsKey(idx)) {
                Data_Skill data = new Data_Skill(idx);
                _inventoryList.Add(idx, data);
            }

            _inventoryList[idx].Add();
        }

        /// <summary>
        /// save inventory to list
        /// </summary>
        public void OnBeforeSerialize() {
            _serializedList.Clear();

            foreach (KeyValuePair<eSkillType, Data_Skill> pair in _inventoryList) {
                _serializedList.Add(pair.Value);
            }
        }

        /// <summary>
        /// load inventory from lists
        /// </summary>
        public void OnAfterDeserialize() {
            _inventoryList.Clear();

            foreach (Data_Skill data in _serializedList) {
                _inventoryList.Add(data.SkillType, data);
            }
        }
    }
}
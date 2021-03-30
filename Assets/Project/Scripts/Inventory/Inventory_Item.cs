﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;
using System;

namespace EFN {

	[Serializable]
	public class Inventory_Item : ISerializationCallbackReceiver {

		/// <summary>
		/// 이 인벤토리 max size
		/// </summary>
		[SerializeField] protected int _maxDisplayIndex = 0;
		public int MaxDisplayIndex {
			get { return this._maxDisplayIndex; }
			set { this._maxDisplayIndex = value; }
		}

		/// <summary>
		/// serialize 저장 전용 리스트
		/// </summary>
		[SerializeField] private List<Data_Item> _serializedList = new List<Data_Item>();

        protected SortedDictionary<int, Data_Item> _inventoryList = new SortedDictionary<int, Data_Item>();

		public virtual string ToJson() {
			return JsonUtility.ToJson(this);
		}

		public IEnumerator GetEnumerator() {
			return _inventoryList.GetEnumerator();
		}

		public Data_Item Get(int idx) {

			if (false == _inventoryList.ContainsKey(idx)) {
				return null;
			}

			return _inventoryList[idx];
		}

		/// <summary>
		/// 특정한 인덱스의 녀석을 발사하려 한다.
		/// </summary>
		public virtual eErrorCode TryFire(int idx) {
			return eErrorCode.Fail;
		}

		/// <summary>
		/// 특정한 인덱스의 녀석을 사용하려 한다.
		/// </summary>
		public virtual eErrorCode TryUse(int idx) {
			return eErrorCode.Fail;
		}

        public virtual void AddInventory(Data_Item item) {
            int firstIdx = GetFirstIdx();

            this._inventoryList.Add(firstIdx, item);
            item.SlotIndex = firstIdx;
			item.StoredInventory = this;

		}

		public virtual void Remove(int slotIdx) {
			this._inventoryList.Remove(slotIdx);
		}

        public virtual int GetFirstIdx() {
            int rv = 0;

            while(true == _inventoryList.ContainsKey(rv)) {
                rv++;
            }

            return rv;
        }

		/// <summary>
		/// 같은 인벤토리 내부에서 특정 아이템을 다른 슬롯으로 옮길 때!!
		/// </summary>
        public virtual eErrorCode AddInventory(Data_Item fromItem, int targetIdx) {

			if (null == fromItem) {
				Global_Common.LogError("CANT MOVE EMPTY ITEM.");
				return eErrorCode.Fail;
			}

			// 들어온 애가 혹시나 다른곳에서 왓을수도 잇음
			if (fromItem.StoredInventory != this) {
				return AddInventory(fromItem, targetIdx, fromItem.StoredInventory);
			}

			// 같은 자리끼리는 의미가 없음.
			if (fromItem.SlotIndex == targetIdx) {
				return eErrorCode.Success;
			}

			int fromIdx = fromItem.SlotIndex;

			Data_Item target = null;
            this._inventoryList.TryGetValue(targetIdx, out target);

			// 들어가려 하는 슬롯에 없으면 null 로 처리
            if (null == target) {
                this._inventoryList.Add(targetIdx, fromItem);
                fromItem.SlotIndex = targetIdx;

				// 성공적으로 자리 바꿈. 기존거에서 지우고 나감
				this._inventoryList.Remove(fromIdx);

				// 콜백 ㅎㅎ
				Global_UIEvent.CallUIEvent(eEventType.UpdateUserInventory);
				return eErrorCode.Success;
            }

			// 키가 다르거나, 이미 있는애가 스택이 안되거나, 스택이 꽉차있으면 서로 교체
            if (target.Key != fromItem.Key || target.StatusData.Stackable == false || target.IsFullStack == true) {
				int swapIndex = target.SlotIndex;

				this._inventoryList[fromItem.SlotIndex] = target;
				target.SlotIndex = fromItem.SlotIndex;

				this._inventoryList[swapIndex] = fromItem;
				fromItem.SlotIndex = swapIndex;

				// 교체 성공.
				// 콜백 ㅎㅎ
				Global_UIEvent.CallUIEvent(eEventType.UpdateUserInventory);
				return eErrorCode.Success;
            }
			
            eErrorCode rv = target.AddStack(fromItem);

			// 성공시에만 기존 자리에서 지운다.
            if (rv == eErrorCode.Success) {
				this._inventoryList.Remove(fromIdx);
			}

			// 콜백 ㅎㅎ
			Global_UIEvent.CallUIEvent(eEventType.UpdateUserInventory);
			return rv;
		}

		/// <summary>
		/// 각기 다른 두개의 인벤토리에서 슬롯을 서로 옮길 때!!
		/// </summary>
		public virtual eErrorCode AddInventory(Data_Item fromItem, int targetIdx, Inventory_Item externalInven) {
			if (null == fromItem) {
				Global_Common.LogError("CANT MOVE EMPTY ITEM.");
				return eErrorCode.Fail;
			}

			if (externalInven == this) {
				return AddInventory(fromItem, targetIdx);
			}

			int fromIdx = fromItem.SlotIndex;

			Data_Item target = null;
			this._inventoryList.TryGetValue(targetIdx, out target);

			// 들어가려 하는 슬롯에 없으면 null 로 처리
			if (null == target) {
				this._inventoryList.Add(targetIdx, fromItem);
				fromItem.SlotIndex = targetIdx;
				fromItem.StoredInventory = this;

				// 성공적으로 자리 바꿈. 기존거에서 지우고 나감
				externalInven._inventoryList.Remove(fromIdx);

				// 콜백 ㅎㅎ
				Global_UIEvent.CallUIEvent(eEventType.UpdateUserInventory);
				return eErrorCode.Success;
			}

			// 키가 다르거나, 이미 있는애가 스택이 안되거나, 스택이 꽉차있으면 서로 교체
			if (target.Key != fromItem.Key || target.StatusData.Stackable == false || target.IsFullStack == true) {
				int swapIndex = target.SlotIndex;

				externalInven._inventoryList[fromItem.SlotIndex] = target;
				target.SlotIndex = fromItem.SlotIndex;
				target.StoredInventory = externalInven;

				this._inventoryList[swapIndex] = fromItem;
				fromItem.SlotIndex = swapIndex;
				fromItem.StoredInventory = this;

				// 교체 성공.
				// 콜백 ㅎㅎ
				Global_UIEvent.CallUIEvent(eEventType.UpdateUserInventory);
				return eErrorCode.Success;
			}

			eErrorCode rv = target.AddStack(fromItem);

			// 성공시에만 기존 자리에서 지운다.
			if (rv == eErrorCode.Success) {
				externalInven._inventoryList.Remove(fromIdx);
			}

			// 콜백 ㅎㅎ
			Global_UIEvent.CallUIEvent(eEventType.UpdateUserInventory);
			return rv;
		}

		public virtual void Use(int slotIdx) {
			if (false == this._inventoryList.ContainsKey(slotIdx)) {
				return;
			}

			this._inventoryList[slotIdx].OnUse();
		}

		/// <summary>
		/// save inventory to list
		/// </summary>
		public void OnBeforeSerialize() {
			_serializedList.Clear();

			foreach (KeyValuePair<int, Data_Item> pair in _inventoryList) {
				_serializedList.Add(pair.Value);
			}
		}

		/// <summary>
		/// load inventory from lists
		/// </summary>
		public void OnAfterDeserialize() {
			_inventoryList.Clear();

			foreach (Data_Item data in _serializedList) {
				_inventoryList.Add(data.SlotIndex, data);
				data.StoredInventory = this;
			}
		}
	}
}
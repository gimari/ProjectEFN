using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {

	[Serializable]
	public class Data_Storable : ISerializationCallbackReceiver {

		[SerializeField]
		private int _stackCount = 0;
		public int StackCount {
			get { return this._stackCount; }
			set { this._stackCount = value; }
		}

		[NonSerialized] protected int _key = 0;
		public int Key {
			get { return this._key; }
			set { this._key = value; }
		}

		[NonSerialized] protected Status_Base _statusData = null;
		public Status_Base StatusData {
			get { 
				if (null == this._statusData) {
					Debug.LogError("I HAVE NO STATUS! : " + this.GetType());
				}

				return this._statusData; 
			}
		}

		public Data_Storable() {
			this._stackCount = 1;
		}

		public virtual bool IsFullStack {
			get { return StatusData.MaxStackSize <= this._stackCount; }
		}

		public virtual void OnRemoved() { }
		public virtual void OnStored() { }

		public virtual eErrorCode AddStack(Data_Storable from) {
			if (false == StatusData.Stackable || false == from.StatusData.Stackable) {
				Global_Common.LogError("Cannot stack Instackable.");
				return eErrorCode.Fail;
			}

			if (this._key != from.Key) {
				Global_Common.LogError("Cannot stack different storeable.");
				return eErrorCode.Fail;
			}

			// 최대 스택개수 넘치면
			if (this.StatusData.MaxStackSize < this._stackCount + from.StackCount) {

				// 여기서 들어온놈의 stack count 를 남은애들로 바꿔준다.
				from._stackCount = this._stackCount + from.StackCount - this.StatusData.MaxStackSize;

				// 내거는 꽉차게 해줌.
				this._stackCount = this.StatusData.MaxStackSize;

				return eErrorCode.StackOverflow;
			}

			// 단순 스택 증가.
			this._stackCount += from.StackCount;

			return eErrorCode.Success;
		}

		public virtual void OnBeforeSerialize() { }

		public virtual void OnAfterDeserialize() {
			InitStatusData();
		}

		protected virtual void InitStatusData() { }

		public virtual eErrorCode DecreaseItem() { 
			if (this._stackCount <= 0) {
				return eErrorCode.Fail;
			}

			this._stackCount--;
			if (this._stackCount <= 0) {
				OnDiscard();
			}

			return eErrorCode.Success;
		}

		public virtual eErrorCode DecreaseItem(int count, out int usecount) {
			usecount = 0;

			if (this._stackCount <= 0) {
				return eErrorCode.Fail;
			}

			// 지금 스택보다 많이쓰면?
			usecount = this._stackCount;
			this._stackCount -= count;
			if (this._stackCount <= 0) {
				OnDiscard();
				return eErrorCode.Success;
			}

			// 남은게 있으면?
			usecount = count;
			return eErrorCode.Success;
		}

		public virtual void OnDiscard() { }
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {

	public class Data_Storable {

		private bool _stackable = true;
		public bool Stackable {
			get { return this._stackable; }
		}

		public bool IsFullStack {
			get { return this._maxStackCount <= this._stackCount; }
		}

		private int _maxStackCount = int.MaxValue;
		public int MaxStackCount {
			get { return this._maxStackCount; }
			set { this._maxStackCount = value; }
		}

		private int _stackCount = 0;
		public int StackCount {
			get { return this._stackCount; }
			set { this._stackCount = value; }
		}

		private int _key = 0;
		public int Key {
			get { return this._key; }
			set { this._key = value; }
		}

		public Data_Storable() {
			this._stackCount = 1;
		}

		public virtual void OnRemoved() { }
		public virtual void OnStored() { }

		public virtual eErrorCode AddStack(Data_Storable from) {
			if (false == this._stackable || false == from.Stackable) {
				Global_Common.LogError("Cannot stack Instackable.");
				return eErrorCode.Fail;
			}

			if (this._key != from.Key) {
				Global_Common.LogError("Cannot stack different storeable.");
				return eErrorCode.Fail;
			}

			// 최대 스택개수 넘치면
			if (this.MaxStackCount < this._stackCount + from.StackCount) {

				// 여기서 들어온놈의 stack count 를 남은애들로 바꿔준다.
				from._stackCount = this._stackCount + from.StackCount - this._maxStackCount;

				// 내거는 꽉차게 해줌.
				this._stackCount = this._maxStackCount;

				return eErrorCode.StackOverflow;
			}

			// 단순 스택 증가.
			this._stackCount += from.StackCount;

			return eErrorCode.Success;
		}
	}
}
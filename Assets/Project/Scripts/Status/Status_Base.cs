using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {
    public class Status_Base {

		private static Dictionary<eItemType, Status_Base> _statusList = new Dictionary<eItemType, Status_Base>();

        /// <summary>
        /// 원래 status 는 엑셀같은 외부정보로 받아와야 하지만 간단하게 하기 위해 이런 방식을 취한다.
        /// 게임이 더 커진다면 좋겟지만 안되면 이상태로 냅둔다.
        /// </summary>
        public static Status_Base GetStatus(eItemType item) {

			if (false == _statusList.ContainsKey(item)) {
				Status_Base status;

				switch (item) {
					case eItemType.Weapon_MP443:
						status = new Status_MP443();
						break;

					case eItemType.Armor_6B3TM:
						status = new Status_6B3TM();
						break;

					case eItemType.AMMO_9X19AP:
						status = new Status_9X19AP();
						break;

					case eItemType.WEAPON_ASVAL:
						status = new Status_ASVAL();
						break;

					case eItemType.AMMO_9X39SP5:
						status = new Status_9X39SP5();
						break;

					default:
						status = null;
						break;
				}

				_statusList.Add(item, status);
			}

			return _statusList[item];
		}

		/// <summary>
		/// 스택 관련
		/// </summary>

		// 스택 가능?
		public virtual bool Stackable { get { return false; } }

		// 최대 스택 사이즈?
        public virtual int MaxStackSize { get { return 1; } }

		// stack 표기 해줄거?
        public virtual bool DisplayStack { get { return false; } }

		/// <summary>
		/// 사용 아이템 관련 정보
		/// </summary>
		
		// 사용 할 수 있음?
        public virtual bool Useable { get { return false; } }

		// 사용시 쿨타임, 총의 경우 스왑시간 쿨타임
		public virtual float UseCoolTime { get { return 0; } }

		// 쏘거나 사용할 때 필요한 부가아이템?
		public virtual eItemType[] RequireItem { get { return null; } }

		/// <summary>
		/// 무기 관련 정보
		/// </summary>

		// 쏠 수 있음?
		public virtual bool Fireable { get { return false; } }

		// 연사 가능?
		public virtual bool ContinuousFire { get { return false; } }

		// 발사 시간
		public virtual float FireRate { get { return 0; } }
	}

	/// <summary>
	/// 9x39 탄
	/// </summary>
	public class Status_9X39SP5 : Status_Base {
		public override bool Stackable { get { return true; } }
		public override int MaxStackSize { get { return 20; } }
		public override bool DisplayStack { get { return true; } }
	}

	/// <summary>
	/// 돌격소총 아스발
	/// </summary>
	public class Status_ASVAL : Status_Base {
		public override bool Fireable { get { return true; } }
		public override bool ContinuousFire { get { return true; } }
		public override float FireRate { get { return 0.11f; } }
		public override eItemType[] RequireItem { get { return new eItemType[] { eItemType.AMMO_9X39SP5 }; } }
		public override float UseCoolTime { get { return 1.2f; } }
	}

	/// <summary>
	/// 권총 mp443
	/// </summary>
	public class Status_MP443 : Status_Base {
		public override eItemType[] RequireItem { get { return new eItemType[] { eItemType.AMMO_9X19AP }; } }
		public override bool Fireable { get { return true; } }
		public override float FireRate { get { return 0.2f; } }
		public override float UseCoolTime { get { return 0.3f; } }
	}
}
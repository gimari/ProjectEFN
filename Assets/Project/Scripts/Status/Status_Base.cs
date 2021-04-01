using EFN.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {

	public enum eItemType {
		None = 0,

		Armor_6B3TM,
		Weapon_MP443,
		AMMO_9X19AP,
		WEAPON_ASVAL,
		AMMO_9X39SP5,
		CONS_FIRSTAID,
	}

	public enum eItemCategory {
		None = 0,

		Weapon = 1,
		Ammo = 2,
		Consumable = 3,
	}

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

					case eItemType.CONS_FIRSTAID:
						status = new Status_FIRSTAID();
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
		/// 기본 정보 관련
		/// </summary>
		
		// 아이템의 공통분류?
		public virtual eItemCategory ItemCategory { get { return eItemCategory.None; } }

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

		// 사용 시 취소될 수 있는 액터 행동 조건
		public virtual eBehaviourCondition CancelCondition { get { return eBehaviourCondition.None; } }

		// 이 아이템이 사용되었을 경우
		public virtual void OnEndItemUsed(Actor_Player actor) { }

		// 적을 때렸을 때 대미지
		public virtual float DmgAmount { get { return 0; } }

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
		public override eItemCategory ItemCategory { get { return eItemCategory.Ammo; } }
		public override bool Stackable { get { return true; } }
		public override int MaxStackSize { get { return 20; } }
		public override bool DisplayStack { get { return true; } }
		public override float DmgAmount { get { return 10; } }
	}

	/// <summary>
	/// 9x19 탄
	/// </summary>
	public class Status_9X19AP : Status_Base {
		public override eItemCategory ItemCategory { get { return eItemCategory.Ammo; } }
		public override bool Stackable { get { return true; } }
		public override int MaxStackSize { get { return 11; } }
		public override bool DisplayStack { get { return true; } }
		public override float DmgAmount { get { return 10; } }
	}

	/// <summary>
	/// 돌격소총 아스발
	/// </summary>
	public class Status_ASVAL : Status_Base {
		public override eItemCategory ItemCategory { get { return eItemCategory.Weapon; } }
		public override bool Useable { get { return true; } }
		public override bool Fireable { get { return true; } }
		public override bool ContinuousFire { get { return true; } }
		public override float FireRate { get { return 0.11f; } }
		public override eItemType[] RequireItem { get { return new eItemType[] { eItemType.AMMO_9X39SP5 }; } }
		public override float UseCoolTime { get { return 0.8f; } }
	}

	/// <summary>
	/// 권총 mp443
	/// </summary>
	public class Status_MP443 : Status_Base {
		public override eItemCategory ItemCategory { get { return eItemCategory.Weapon; } }
		public override eItemType[] RequireItem { get { return new eItemType[] { eItemType.AMMO_9X19AP }; } }
		public override bool Useable { get { return true; } }
		public override bool Fireable { get { return true; } }
		public override float FireRate { get { return 0.2f; } }
		public override float UseCoolTime { get { return 0.3f; } }
	}

	/// <summary>
	/// 힐킷
	/// </summary>
	public class Status_FIRSTAID : Status_Base {
		public override eItemCategory ItemCategory { get { return eItemCategory.Consumable; } }
		public override bool Useable { get { return true; } }
		public override float UseCoolTime { get { return 3; } }
		public override eBehaviourCondition CancelCondition { get { return eBehaviourCondition.Running | eBehaviourCondition.Damaging | eBehaviourCondition.Firing; } }

		public override void OnEndItemUsed(Actor_Player actor) {
			Debug.LogError("I USED!!! : " + actor.name);

			// 팔도 맨손으로 바꿔준다.
			if (null != actor.PlayerArmObject) {
				actor.PlayerArmObject.SetBareHand();
			}
		}
	}
}
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
		WEAPON_RECORDER,
		HEAD_STAR,
		WEAPON_KS23M,
		AMMO_23X75,
		AMMO_9X197N,
		WEAPON_MP5,
		WEAPON_VECTOR,
		AMMO_45AP,
		WEAPON_AKM,
		AMMO_762BP,
		AMMO_762US,
		WEAPON_SKS,
		WEAPON_HK416,
		AMMO_556M855,
		WEAPON_M1911,
		HEAD_FEDORA,
		HEAD_RABBIT,

		NONE_COKE0,
		NONE_COKE1,
		NONE_COKE2,
		NONE_COKE3,
		NONE_COKE4,
		NONE_COKE5,
		NONE_COKE6,
		NONE_COKE7,
		NONE_COKE8,
		NONE_COKE9,
		NONE_COKE10,
		NONE_COKE11,
		NONE_COKE12,
		NONE_COKE13,
		NONE_COKE14,
		NONE_TROPHY0,
		NONE_TROPHY1,
		NONE_TROPHY2,
		NONE_TROPHY3,
		NONE_TROPHY4,
		NONE_TROPHY5,

		WEAPON_DAGGER,
		HEAD_DDU,
		HEAD_COLLET,
		HEAD_TEM,
		HEAD_YOOWOO,
	}

	public enum eItemCategory {
		None = 0,

		Weapon = 1,
		Ammo = 2,
		Consumable = 3,
		Head = 4,
	}

	public enum eWeaponCategory {
		Common = 0,
		Knife = 1,
		ShotGun
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
					case eItemType.Weapon_MP443:	{ status = new Status_MP443(); } break;
					case eItemType.Armor_6B3TM:		{ status = new Status_6B3TM(); } break;
					case eItemType.AMMO_9X19AP:		{ status = new Status_9X19AP(); } break;
					case eItemType.WEAPON_ASVAL:	{ status = new Status_ASVAL(); } break;
					case eItemType.AMMO_9X39SP5:	{ status = new Status_9X39SP5(); } break;
					case eItemType.CONS_FIRSTAID:	{ status = new Status_FIRSTAID(); } break;
					case eItemType.WEAPON_RECORDER: { status = new Weapon_RECORDER(); } break;
					case eItemType.HEAD_STAR:		{ status = new Status_HeadStar(); } break;
					case eItemType.HEAD_FEDORA:		{ status = new Status_Fedora(); } break;
					case eItemType.HEAD_RABBIT:		{ status = new Status_Rabbit(); } break;
					case eItemType.WEAPON_KS23M:	{ status = new Status_KS23M(); } break;
					case eItemType.AMMO_23X75:		{ status = new Status_23X75(); } break;
					case eItemType.AMMO_9X197N:		{ status = new Status_9X197N(); } break;
					case eItemType.WEAPON_MP5:		{ status = new Status_MP5(); } break;
					case eItemType.AMMO_45AP:		{ status = new Status_45AP(); } break;
					case eItemType.WEAPON_VECTOR:	{ status = new Status_VECTOR(); } break;
					case eItemType.WEAPON_AKM:		{ status = new Status_AKM(); } break;
					case eItemType.AMMO_762BP:		{ status = new Status_762BP(); } break;
					case eItemType.AMMO_762US:		{ status = new Status_762US(); } break;
					case eItemType.WEAPON_SKS:		{ status = new Status_SKS(); } break;
					case eItemType.WEAPON_HK416:	{ status = new Status_HK416(); } break;
					case eItemType.AMMO_556M855:	{ status = new Status_556M855(); } break;
					case eItemType.WEAPON_M1911:	{ status = new Status_M1911(); } break;
					case eItemType.NONE_COKE0:		{ status = new Status_Coke0(); } break;
					case eItemType.NONE_COKE1:		{ status = new Status_Coke1(); } break;
					case eItemType.NONE_COKE2:		{ status = new Status_Coke2(); } break;
					case eItemType.NONE_COKE3:		{ status = new Status_Coke3(); } break;
					case eItemType.NONE_COKE4:		{ status = new Status_Coke4(); } break;
					case eItemType.NONE_COKE5:		{ status = new Status_Coke5(); } break;
					case eItemType.NONE_COKE6:		{ status = new Status_Coke6(); } break;
					case eItemType.NONE_COKE7:		{ status = new Status_Coke7(); } break;
					case eItemType.NONE_COKE8:		{ status = new Status_Coke8(); } break;
					case eItemType.NONE_COKE9:		{ status = new Status_Coke9(); } break;
					case eItemType.NONE_COKE10:		{ status = new Status_Coke10(); } break;
					case eItemType.NONE_COKE11:		{ status = new Status_Coke11(); } break;
					case eItemType.NONE_COKE12:		{ status = new Status_Coke12(); } break;
					case eItemType.NONE_COKE13:		{ status = new Status_Coke13(); } break;
					case eItemType.NONE_COKE14:		{ status = new Status_Coke14(); } break;
					case eItemType.NONE_TROPHY0:	{ status = new Status_Trophy0(); } break;
					case eItemType.NONE_TROPHY1:	{ status = new Status_Trophy1(); } break;
					case eItemType.NONE_TROPHY2:	{ status = new Status_Trophy2(); } break;
					case eItemType.NONE_TROPHY3:	{ status = new Status_Trophy3(); } break;
					case eItemType.NONE_TROPHY4:	{ status = new Status_Trophy4(); } break;
					case eItemType.NONE_TROPHY5:	{ status = new Status_Trophy5(); } break;
					case eItemType.WEAPON_DAGGER:	{ status = new Weapon_DAGGER(); } break;
					case eItemType.HEAD_DDU:		{ status = new Status_HeadDdu(); } break;
					case eItemType.HEAD_COLLET:		{ status = new Status_HeadCollet(); } break;
					case eItemType.HEAD_TEM:		{ status = new Status_HeadTem(); } break;
					case eItemType.HEAD_YOOWOO:		{ status = new Status_HeadYoowoo(); } break;
					
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

		// 아이템 기본 가격
		public virtual long DefaultPrice { get { return 10; } }

		// 아이템 표기 이름
		public virtual string DisplayName { get { return ""; } }

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
		public virtual void OnEndItemUsed(Actor_Player actor, Data_Item usedItem) { }

		// 적을 때렸을 때 대미지
		public virtual float DmgAmount { get { return 0; } }

		// 장착 가능한 놈이라면 가능한 슬롯.
		public virtual ePlayerSlotType TargetEquipSlot { get { return ePlayerSlotType.None; } }

		/// <summary>
		/// 무기 관련 정보
		/// </summary>

		// 쏠 수 있음?
		public virtual bool Fireable { get { return false; } }

		// 연사 가능?
		public virtual bool ContinuousFire { get { return false; } }

		// 발사 시간
		public virtual float FireRate { get { return 0; } }

		// 근접무기인가?
		public virtual eWeaponCategory WeaponType { get { return eWeaponCategory.Common; } }

		// 재장전 속도
		public virtual float ReloadTime { get { return 0; } }

		// 최대 탄창
		public virtual int MaxRoundAmount { get { return 0; } }

		// 한번에 발사되는 총알개수
		public virtual int FireRoundsInSingle { get { return 1; } }

		// 반동 크기
		public virtual float RecoilRate { get { return 0; } }

		// 소음기임?
		public virtual bool UsingSilence { get { return false; } }
	}

	/// <summary>
	/// 9x39 탄
	/// </summary>
	public class Status_9X39SP5 : Status_Base {
		public override eItemCategory ItemCategory { get { return eItemCategory.Ammo; } }
		public override bool Stackable { get { return true; } }
		public override int MaxStackSize { get { return 30; } }
		public override bool DisplayStack { get { return true; } }
		public override float DmgAmount { get { return 21; } }
		public override long DefaultPrice { get { return 167; } }
		public override string DisplayName { get { return "9x39"; } }
	}

	/// <summary>
	/// 9x19 탄
	/// </summary>
	public class Status_9X19AP : Status_Base {
		public override eItemCategory ItemCategory { get { return eItemCategory.Ammo; } }
		public override bool Stackable { get { return true; } }
		public override int MaxStackSize { get { return 30; } }
		public override bool DisplayStack { get { return true; } }
		public override float DmgAmount { get { return 16; } }
		public override long DefaultPrice { get { return 83; } }
		public override string DisplayName { get { return "9x19 AP"; } }
	}

	/// <summary>
	/// 돌격소총 아스발
	/// </summary>
	public class Status_ASVAL : Status_Base {
		public override float ReloadTime { get { return 0.5f; } }
		public override int MaxRoundAmount { get { return 20; } }
		public override eItemCategory ItemCategory { get { return eItemCategory.Weapon; } }
		public override bool Useable { get { return true; } }
		public override bool Fireable { get { return true; } }
		public override bool ContinuousFire { get { return true; } }
		public override float FireRate { get { return 0.06f; } }
		public override eItemType[] RequireItem { get { return new eItemType[] { eItemType.AMMO_9X39SP5 }; } }
		public override float UseCoolTime { get { return 0.8f; } }
		public override ePlayerSlotType TargetEquipSlot { get { return ePlayerSlotType.PrimeWeapon; } }
		public override long DefaultPrice { get { return 10234; } }
		public override bool UsingSilence { get { return true; } }
		public override float RecoilRate { get { return 22; } }
		public override string DisplayName { get { return "ASVAL"; } }
	}

	/// <summary>
	/// 권총 mp443
	/// </summary>
	public class Status_MP443 : Status_Base {
		public override float ReloadTime { get { return 0.3f; } }
		public override int MaxRoundAmount { get { return 16; } }
		public override eItemCategory ItemCategory { get { return eItemCategory.Weapon; } }
		public override eItemType[] RequireItem { get { return new eItemType[] { eItemType.AMMO_9X19AP, eItemType.AMMO_9X197N }; } }
		public override bool Useable { get { return true; } }
		public override bool Fireable { get { return true; } }
		public override float FireRate { get { return 0.5f; } }
		public override float UseCoolTime { get { return 0.3f; } }
		public override ePlayerSlotType TargetEquipSlot { get { return ePlayerSlotType.Holster; } }
		public override long DefaultPrice { get { return 5913; } }
		public override float RecoilRate { get { return 13; } }
		public override string DisplayName { get { return "MP443"; } }
	}

	/// <summary>
	/// 힐킷
	/// </summary>
	public class Status_FIRSTAID : Status_Base {
		public override bool Stackable { get { return true; } }
		public override int MaxStackSize { get { return 3; } }
		public override bool DisplayStack { get { return true; } }
		public override eItemCategory ItemCategory { get { return eItemCategory.Consumable; } }
		public override bool Useable { get { return true; } }
		public override float UseCoolTime { get { return 3; } }
		public override eBehaviourCondition CancelCondition { get { return eBehaviourCondition.Running | eBehaviourCondition.Damaging | eBehaviourCondition.Firing; } }
		public override long DefaultPrice { get { return 2715; } }

		public override void OnEndItemUsed(Actor_Player actor, Data_Item usedItem) {

			// 팔도 맨손으로 바꿔준다.
			if (null != actor.PlayerArmObject) {
				actor.PlayerArmObject.SetBareHand();
			}

			actor.Dmgable.Heal(45);

			if (null != usedItem) {
				usedItem.DecreaseItem();
			}

			// 통보
			Global_UIEvent.CallUIEvent(eEventType.UpdateUserInventory);
		}
		public override string DisplayName { get { return "First aid"; } }
	}

	/// <summary>
	/// 근접무기 리코더
	/// </summary>
	public class Weapon_RECORDER : Status_Base {
		public override eItemCategory ItemCategory { get { return eItemCategory.Weapon; } }
		public override bool Useable { get { return true; } }
		public override bool Fireable { get { return true; } }
		public override float FireRate { get { return 0.3f; } }
		public override float UseCoolTime { get { return 0.3f; } }
		public override eWeaponCategory WeaponType { get { return eWeaponCategory.Knife; } }
		public override float DmgAmount { get { return 27; } }
		public override ePlayerSlotType TargetEquipSlot { get { return ePlayerSlotType.Knife; } }
		public override long DefaultPrice { get { return 444444; } }
		public override string DisplayName { get { return "Recorder"; } }
	}

	/// <summary>
	/// 근접무기 대거
	/// </summary>
	public class Weapon_DAGGER : Status_Base {
		public override eItemCategory ItemCategory { get { return eItemCategory.Weapon; } }
		public override bool Useable { get { return true; } }
		public override bool Fireable { get { return true; } }
		public override float FireRate { get { return 0.3f; } }
		public override float UseCoolTime { get { return 0.3f; } }
		public override eWeaponCategory WeaponType { get { return eWeaponCategory.Knife; } }
		public override float DmgAmount { get { return 14; } }
		public override ePlayerSlotType TargetEquipSlot { get { return ePlayerSlotType.Knife; } }
		public override long DefaultPrice { get { return 1345; } }
		public override string DisplayName { get { return "Dagger"; } }
	}

	/// <summary>
	/// 갑옷 6B3TM
	/// </summary>
	public class Status_6B3TM : Status_Base {
		public override ePlayerSlotType TargetEquipSlot { get { return ePlayerSlotType.Armor; } }
		public override long DefaultPrice { get { return 67320; } }
	}

	/// <summary>
	/// 별달린모자
	/// </summary>
	public class Status_HeadStar : Status_Base {
		public override eItemCategory ItemCategory { get { return eItemCategory.Head; } }
		public override ePlayerSlotType TargetEquipSlot { get { return ePlayerSlotType.Head; } }
		public override long DefaultPrice { get { return 44444; } }
		public override string DisplayName { get { return "Galaxy Star"; } }
	}

	/// <summary>
	/// 뚜띠모자
	/// </summary>
	public class Status_HeadDdu : Status_Base {
		public override eItemCategory ItemCategory { get { return eItemCategory.Head; } }
		public override ePlayerSlotType TargetEquipSlot { get { return ePlayerSlotType.Head; } }
		public override long DefaultPrice { get { return 960524; } }
		public override string DisplayName { get { return "Banana"; } }
	}

	/// <summary>
	/// 코렛트모자
	/// </summary>
	public class Status_HeadCollet : Status_Base {
		public override eItemCategory ItemCategory { get { return eItemCategory.Head; } }
		public override ePlayerSlotType TargetEquipSlot { get { return ePlayerSlotType.Head; } }
		public override long DefaultPrice { get { return 1000802; } }
		public override string DisplayName { get { return "Beanie"; } }
	}

	/// <summary>
	/// 탬탬모자
	/// </summary>
	public class Status_HeadTem : Status_Base {
		public override eItemCategory ItemCategory { get { return eItemCategory.Head; } }
		public override ePlayerSlotType TargetEquipSlot { get { return ePlayerSlotType.Head; } }
		public override long DefaultPrice { get { return 313506; } }
		public override string DisplayName { get { return "Bell"; } }
	}

	/// <summary>
	/// 유우양모자
	/// </summary>
	public class Status_HeadYoowoo : Status_Base {
		public override eItemCategory ItemCategory { get { return eItemCategory.Head; } }
		public override ePlayerSlotType TargetEquipSlot { get { return ePlayerSlotType.Head; } }
		public override long DefaultPrice { get { return 333536; } }
		public override string DisplayName { get { return "Milk"; } }
	}

	/// <summary>
	/// 지누모자
	/// </summary>
	public class Status_Fedora : Status_Base {
		public override eItemCategory ItemCategory { get { return eItemCategory.Head; } }
		public override ePlayerSlotType TargetEquipSlot { get { return ePlayerSlotType.Head; } }
		public override long DefaultPrice { get { return 950802; } }
		public override string DisplayName { get { return "Fedora"; } }
	}

	/// <summary>
	/// 춘향귀
	/// </summary>
	public class Status_Rabbit : Status_Base {
		public override eItemCategory ItemCategory { get { return eItemCategory.Head; } }
		public override ePlayerSlotType TargetEquipSlot { get { return ePlayerSlotType.Head; } }
		public override long DefaultPrice { get { return 310169; } }
		public override string DisplayName { get { return "Rabbit"; } }
	}

	/// <summary>
	/// 무기 KS23M
	/// </summary>
	public class Status_KS23M : Status_Base {
		public override float ReloadTime { get { return 0.3f; } }
		public override int MaxRoundAmount { get { return 3; } }
		public override eItemCategory ItemCategory { get { return eItemCategory.Weapon; } }
		public override eItemType[] RequireItem { get { return new eItemType[] { eItemType.AMMO_23X75 }; } }
		public override bool Useable { get { return true; } }
		public override bool Fireable { get { return true; } }
		public override float FireRate { get { return 0.5f; } }
		public override float UseCoolTime { get { return 0.65f; } }
		public override ePlayerSlotType TargetEquipSlot { get { return ePlayerSlotType.PrimeWeapon; } }
		public override long DefaultPrice { get { return 42983; } }
		public override int FireRoundsInSingle { get { return 7; } }
		public override eWeaponCategory WeaponType { get { return eWeaponCategory.ShotGun; } }
		public override float RecoilRate { get { return 18; } }
		public override string DisplayName { get { return "KS23M"; } }
	}

	/// <summary>
	/// 23x75 탄
	/// </summary>
	public class Status_23X75 : Status_Base {
		public override eItemCategory ItemCategory { get { return eItemCategory.Ammo; } }
		public override bool Stackable { get { return true; } }
		public override int MaxStackSize { get { return 5; } }
		public override bool DisplayStack { get { return true; } }
		public override float DmgAmount { get { return 22; } }
		public override long DefaultPrice { get { return 826; } }
		public override string DisplayName { get { return "23x75"; } }
	}

	/// <summary>
	/// AMMO_9X197N 탄
	/// </summary>
	public class Status_9X197N : Status_Base {
		public override eItemCategory ItemCategory { get { return eItemCategory.Ammo; } }
		public override bool Stackable { get { return true; } }
		public override int MaxStackSize { get { return 30; } }
		public override bool DisplayStack { get { return true; } }
		public override float DmgAmount { get { return 18; } }
		public override long DefaultPrice { get { return 22; } }
		public override string DisplayName { get { return "9x19 7N"; } }
	}

	/// <summary>
	/// 무기 mp5
	/// </summary>
	public class Status_MP5 : Status_Base {
		public override float ReloadTime { get { return 0.4f; } }
		public override int MaxRoundAmount { get { return 30; } }
		public override eItemCategory ItemCategory { get { return eItemCategory.Weapon; } }
		public override bool Useable { get { return true; } }
		public override bool Fireable { get { return true; } }
		public override bool ContinuousFire { get { return true; } }
		public override float FireRate { get { return 0.07f; } }
		public override eItemType[] RequireItem { get { return new eItemType[] { eItemType.AMMO_9X19AP, eItemType.AMMO_9X197N }; } }
		public override float UseCoolTime { get { return 0.8f; } }
		public override ePlayerSlotType TargetEquipSlot { get { return ePlayerSlotType.PrimeWeapon; } }
		public override long DefaultPrice { get { return 28234; } }
		public override float RecoilRate { get { return 12; } }
		public override string DisplayName { get { return "MP5"; } }
	}

	/// <summary>
	/// 무기 vector
	/// </summary>
	public class Status_VECTOR : Status_Base {
		public override float ReloadTime { get { return 0.4f; } }
		public override int MaxRoundAmount { get { return 40; } }
		public override eItemCategory ItemCategory { get { return eItemCategory.Weapon; } }
		public override bool Useable { get { return true; } }
		public override bool Fireable { get { return true; } }
		public override bool ContinuousFire { get { return true; } }
		public override float FireRate { get { return 0.05f; } }
		public override eItemType[] RequireItem { get { return new eItemType[] { eItemType.AMMO_45AP }; } }
		public override float UseCoolTime { get { return 0.8f; } }
		public override ePlayerSlotType TargetEquipSlot { get { return ePlayerSlotType.PrimeWeapon; } }
		public override long DefaultPrice { get { return 10234; } }
		public override float RecoilRate { get { return 8; } }
		public override bool UsingSilence { get { return true; } }
		public override string DisplayName { get { return "Vector"; } }
	}

	/// <summary>
	/// AMMO_45AP 탄
	/// </summary>
	public class Status_45AP : Status_Base {
		public override eItemCategory ItemCategory { get { return eItemCategory.Ammo; } }
		public override bool Stackable { get { return true; } }
		public override int MaxStackSize { get { return 40; } }
		public override bool DisplayStack { get { return true; } }
		public override float DmgAmount { get { return 19; } }
		public override long DefaultPrice { get { return 173; } }
		public override string DisplayName { get { return "45 AP"; } }
	}

	/// <summary>
	/// 무기 akm
	/// </summary>
	public class Status_AKM : Status_Base {
		public override float ReloadTime { get { return 0.6f; } }
		public override int MaxRoundAmount { get { return 30; } }
		public override eItemCategory ItemCategory { get { return eItemCategory.Weapon; } }
		public override bool Useable { get { return true; } }
		public override bool Fireable { get { return true; } }
		public override bool ContinuousFire { get { return true; } }
		public override float FireRate { get { return 0.1f; } }
		public override eItemType[] RequireItem { get { return new eItemType[] { eItemType.AMMO_762BP, eItemType.AMMO_762US }; } }
		public override float UseCoolTime { get { return 0.8f; } }
		public override ePlayerSlotType TargetEquipSlot { get { return ePlayerSlotType.PrimeWeapon; } }
		public override long DefaultPrice { get { return 76194; } }
		public override float RecoilRate { get { return 30; } }
		public override string DisplayName { get { return "AKM"; } }
	}

	/// <summary>
	/// AMMO_762BP 탄
	/// </summary>
	public class Status_762BP : Status_Base {
		public override eItemCategory ItemCategory { get { return eItemCategory.Ammo; } }
		public override bool Stackable { get { return true; } }
		public override int MaxStackSize { get { return 30; } }
		public override bool DisplayStack { get { return true; } }
		public override float DmgAmount { get { return 24; } }
		public override long DefaultPrice { get { return 134; } }
		public override string DisplayName { get { return "7.62 BP"; } }
	}

	/// <summary>
	/// AMMO_762US 탄
	/// </summary>
	public class Status_762US : Status_Base {
		public override eItemCategory ItemCategory { get { return eItemCategory.Ammo; } }
		public override bool Stackable { get { return true; } }
		public override int MaxStackSize { get { return 30; } }
		public override bool DisplayStack { get { return true; } }
		public override float DmgAmount { get { return 28; } }
		public override long DefaultPrice { get { return 162; } }
		public override string DisplayName { get { return "7.62 US"; } }
	}

	/// <summary>
	/// 무기 sks
	/// </summary>
	public class Status_SKS : Status_Base {
		public override float ReloadTime { get { return 0.6f; } }
		public override int MaxRoundAmount { get { return 20; } }
		public override eItemCategory ItemCategory { get { return eItemCategory.Weapon; } }
		public override bool Useable { get { return true; } }
		public override bool Fireable { get { return true; } }
		public override float FireRate { get { return 1.5f; } }
		public override eItemType[] RequireItem { get { return new eItemType[] { eItemType.AMMO_762BP, eItemType.AMMO_762US }; } }
		public override float UseCoolTime { get { return 0.7f; } }
		public override ePlayerSlotType TargetEquipSlot { get { return ePlayerSlotType.PrimeWeapon; } }
		public override long DefaultPrice { get { return 10234; } }
		public override float RecoilRate { get { return 11; } }
		public override string DisplayName { get { return "SKS"; } }
	}

	/// <summary>
	/// 무기 akm
	/// </summary>
	public class Status_HK416 : Status_Base {
		public override float ReloadTime { get { return 0.6f; } }
		public override int MaxRoundAmount { get { return 30; } }
		public override eItemCategory ItemCategory { get { return eItemCategory.Weapon; } }
		public override bool Useable { get { return true; } }
		public override bool Fireable { get { return true; } }
		public override bool ContinuousFire { get { return true; } }
		public override float FireRate { get { return 0.07f; } }
		public override eItemType[] RequireItem { get { return new eItemType[] { eItemType.AMMO_556M855 }; } }
		public override float UseCoolTime { get { return 0.6f; } }
		public override ePlayerSlotType TargetEquipSlot { get { return ePlayerSlotType.PrimeWeapon; } }
		public override long DefaultPrice { get { return 10234; } }
		public override float RecoilRate { get { return 18; } }
		public override string DisplayName { get { return "HK416"; } }
	}

	/// <summary>
	/// AMMO_556 탄
	/// </summary>
	public class Status_556M855 : Status_Base {
		public override eItemCategory ItemCategory { get { return eItemCategory.Ammo; } }
		public override bool Stackable { get { return true; } }
		public override int MaxStackSize { get { return 30; } }
		public override bool DisplayStack { get { return true; } }
		public override float DmgAmount { get { return 22; } }
		public override long DefaultPrice { get { return 195; } }
		public override string DisplayName { get { return "5.56"; } }
	}

	/// <summary>
	/// 권총 m1911
	/// </summary>
	public class Status_M1911 : Status_Base {
		public override float ReloadTime { get { return 0.3f; } }
		public override int MaxRoundAmount { get { return 11; } }
		public override eItemCategory ItemCategory { get { return eItemCategory.Weapon; } }
		public override eItemType[] RequireItem { get { return new eItemType[] { eItemType.AMMO_45AP }; } }
		public override bool Useable { get { return true; } }
		public override bool Fireable { get { return true; } }
		public override float FireRate { get { return 0.5f; } }
		public override float UseCoolTime { get { return 0.3f; } }
		public override ePlayerSlotType TargetEquipSlot { get { return ePlayerSlotType.Holster; } }
		public override long DefaultPrice { get { return 8230; } }
		public override bool UsingSilence { get { return true; } }
		public override float RecoilRate { get { return 18; } }
		public override string DisplayName { get { return "M1911"; } }
	}

	/// <summary>
	/// 코크 잡템 0~14
	/// </summary>
	public class Status_Coke0 : Status_Base {
		public override bool Stackable { get { return true; } }
		public override int MaxStackSize { get { return 5; } }
		public override bool DisplayStack { get { return true; } }
		public override string DisplayName { get { return "Coke"; } }
	}
	public class Status_Coke1 : Status_Base {
		public override bool Stackable { get { return true; } }
		public override int MaxStackSize { get { return 5; } }
		public override bool DisplayStack { get { return true; } }
		public override string DisplayName { get { return "Coke"; } }
	}
	public class Status_Coke2 : Status_Base {
		public override bool Stackable { get { return true; } }
		public override int MaxStackSize { get { return 5; } }
		public override bool DisplayStack { get { return true; } }
		public override string DisplayName { get { return "Coke"; } }
	}
	public class Status_Coke3 : Status_Base {
		public override bool Stackable { get { return true; } }
		public override int MaxStackSize { get { return 5; } }
		public override bool DisplayStack { get { return true; } }
		public override string DisplayName { get { return "Coke"; } }
	}
	public class Status_Coke4 : Status_Base {
		public override bool Stackable { get { return true; } }
		public override int MaxStackSize { get { return 5; } }
		public override bool DisplayStack { get { return true; } }
		public override string DisplayName { get { return "Coke"; } }
	}
	public class Status_Coke5 : Status_Base {
		public override bool Stackable { get { return true; } }
		public override int MaxStackSize { get { return 5; } }
		public override bool DisplayStack { get { return true; } }
		public override string DisplayName { get { return "Coke"; } }
	}
	public class Status_Coke6 : Status_Base {
		public override bool Stackable { get { return true; } }
		public override int MaxStackSize { get { return 5; } }
		public override bool DisplayStack { get { return true; } }
		public override string DisplayName { get { return "Coke"; } }
	}
	public class Status_Coke7 : Status_Base {
		public override bool Stackable { get { return true; } }
		public override int MaxStackSize { get { return 5; } }
		public override bool DisplayStack { get { return true; } }
		public override string DisplayName { get { return "Coke"; } }
	}
	public class Status_Coke8 : Status_Base {
		public override bool Stackable { get { return true; } }
		public override int MaxStackSize { get { return 5; } }
		public override bool DisplayStack { get { return true; } }
		public override string DisplayName { get { return "Coke"; } }
	}
	public class Status_Coke9 : Status_Base {
		public override bool Stackable { get { return true; } }
		public override int MaxStackSize { get { return 5; } }
		public override bool DisplayStack { get { return true; } }
		public override string DisplayName { get { return "Coke"; } }
	}
	public class Status_Coke10 : Status_Base {
		public override bool Stackable { get { return true; } }
		public override int MaxStackSize { get { return 5; } }
		public override bool DisplayStack { get { return true; } }
		public override string DisplayName { get { return "Coke"; } }
	}
	public class Status_Coke11 : Status_Base {
		public override bool Stackable { get { return true; } }
		public override int MaxStackSize { get { return 5; } }
		public override bool DisplayStack { get { return true; } }
		public override string DisplayName { get { return "Coke"; } }
	}
	public class Status_Coke12 : Status_Base {
		public override bool Stackable { get { return true; } }
		public override int MaxStackSize { get { return 5; } }
		public override bool DisplayStack { get { return true; } }
		public override string DisplayName { get { return "Coke"; } }
	}
	public class Status_Coke13 : Status_Base {
		public override bool Stackable { get { return true; } }
		public override int MaxStackSize { get { return 5; } }
		public override bool DisplayStack { get { return true; } }
		public override string DisplayName { get { return "Coke"; } }
	}
	public class Status_Coke14 : Status_Base {
		public override bool Stackable { get { return true; } }
		public override int MaxStackSize { get { return 5; } }
		public override bool DisplayStack { get { return true; } }
		public override string DisplayName { get { return "Coke"; } }
	}
	public class Status_Trophy0 : Status_Base {
		public override string DisplayName { get { return "Trophy"; } }
	}
	public class Status_Trophy1 : Status_Base {
		public override string DisplayName { get { return "Trophy"; } }
	}
	public class Status_Trophy2 : Status_Base {
		public override string DisplayName { get { return "Trophy"; } }
	}
	public class Status_Trophy3 : Status_Base {
		public override string DisplayName { get { return "Trophy"; } }
	}
	public class Status_Trophy4 : Status_Base {
		public override string DisplayName { get { return "Trophy"; } }
	}
	public class Status_Trophy5 : Status_Base {
		public override string DisplayName { get { return "Trophy"; } }
	}
}
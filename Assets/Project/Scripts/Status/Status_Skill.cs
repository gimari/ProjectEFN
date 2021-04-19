using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {

    public enum eSkillType {
        None = 0,
        Armor = 1,
        Health,
        Speed,
        Sight,
        Melee,
        Silence,
        Recoil,         // 반동
        Critical,       // 치확
        CritDmg,        // 치뎀
        NormalDmg,      // 일반데미지. 상대값으로 더해짐.
        StashSize,      // 창고사이즈
        InvenSize,      // 플레이어 인벤토리사이즈
    }

    public class Status_Skill {

        private static Dictionary<eSkillType, Status_Skill> _statusList = new Dictionary<eSkillType, Status_Skill>();

        /// <summary>
        /// 원래 status 는 엑셀같은 외부정보로 받아와야 하지만 간단하게 하기 위해 이런 방식을 취한다.
        /// 게임이 더 커진다면 좋겟지만 안되면 이상태로 냅둔다.
        /// </summary>
        public static Status_Skill GetStatus(eSkillType skill) {

			if (false == _statusList.ContainsKey(skill)) {
                Status_Skill status;

				switch (skill) {
					case eSkillType.Armor: { status = new Status_SkillArmor(); } break;
					case eSkillType.Health: { status = new Status_SkillHealth(); } break;
					case eSkillType.Speed: { status = new Status_SkillSpeed(); } break;
					case eSkillType.Sight: { status = new Status_SkillSight(); } break;
					case eSkillType.Melee: { status = new Status_SkillMelee(); } break;
					case eSkillType.Silence: { status = new Status_SkillSilence(); } break;
					case eSkillType.Recoil: { status = new Status_SkillRecoil(); } break;
					case eSkillType.Critical: { status = new Status_SkillCritical(); } break;
					case eSkillType.CritDmg: { status = new Status_SkillCritDmg(); } break;
					case eSkillType.NormalDmg: { status = new Status_SkillNormalDmg(); } break;
					case eSkillType.StashSize: { status = new Status_SkillStashSize(); } break;
					case eSkillType.InvenSize: { status = new Status_SkillInvenSize(); } break;
					default:
						status = null;
						break;
				}

				_statusList.Add(skill, status);
			}

			return _statusList[skill];
		}

		/// <summary>
		/// 스킬 최대레벨
		/// </summary>
		public virtual int MaxLevel { get { return 0; } }

        /// <summary>
        /// 인자로 들어온 레벨에서의 효과량
        /// </summary>
        public virtual float EffectAmount(int level) { return 0; }

        /// <summary>
        /// 인자로 들어온 레벨의 다음 단계로 올리는데 들어가는 cost
        /// </summary>
        public virtual long NextLevelCost(int level) { return 0; }

		/// <summary>
		/// 스킬 설명 반환
		/// </summary>
		public virtual string Explain { get { return ""; } }
	}

	internal class Status_SkillStashSize : Status_Skill {
		public override int MaxLevel { get { return 20; } }
		public override float EffectAmount(int level) { return level * 1; }
		public override long NextLevelCost(int level) { return 5231 + (level) * 2487; }
		public override string Explain { get { return "창고 용량이 {0} 만큼 넓어집니다."; } }
	}

	internal class Status_SkillInvenSize : Status_Skill {
		public override int MaxLevel { get { return 10; } }
		public override float EffectAmount(int level) { return level * 1; }
		public override long NextLevelCost(int level) { return 7612 + (level) * 2412; }
		public override string Explain { get { return "전투 가방이 {0} 만큼 커집니다."; } }
	}

	internal class Status_SkillNormalDmg : Status_Skill {
		public override int MaxLevel { get { return 15; } }
		public override float EffectAmount(int level) { return 1 + (level * 0.01f); }
		public override long NextLevelCost(int level) { return 13726 + (level) * 1726; }
		public override string Explain { get { return "{0}배 더 쎄게 방아쇠를 당깁니다!"; } }
	}

	internal class Status_SkillCritDmg : Status_Skill {
		public override int MaxLevel { get { return 50; } }
		public override float EffectAmount(int level) { return 1.5f + (level * 0.01f); }
		public override long NextLevelCost(int level) { return 2018 + (level + 1) * 918; }
		public override string Explain { get { return "치명타 시 데미지가 {0}배 증가합니다."; } }
	}

	internal class Status_SkillCritical : Status_Skill {
		public override int MaxLevel { get { return 30; } }
		public override float EffectAmount(int level) { return level * 1; }
		public override long NextLevelCost(int level) { return 3382 + (level) * 1282; }
		public override string Explain { get { return "치명타 확율이 {0}% 증가합니다."; } }
	}

	internal class Status_SkillRecoil : Status_Skill {
		public override int MaxLevel { get { return 25; } }
		public override float EffectAmount(int level) { return 1 - (level * 0.01f); }
		public override long NextLevelCost(int level) { return 2694 + (level) * 694; }
		public override string Explain { get { return "{0}만큼 손을 덜 떨게 됩니다!"; } }
	}

	internal class Status_SkillSilence : Status_Skill {
		public override int MaxLevel { get { return 50; } }
		public override float EffectAmount(int level) { return 1 - (level * 0.01f); }
		public override long NextLevelCost(int level) { return 2512 + (level) * 812; }
		public override string Explain { get { return "{0}만큼 덜 시끄러워 집니다!"; } }
	}

	internal class Status_SkillMelee : Status_Skill {
		public override int MaxLevel { get { return 10; } }
		public override float EffectAmount(int level) { return 1 + (level * 0.05f); }
		public override long NextLevelCost(int level) { return 3629 + (level) * 1429; }
		public override string Explain { get { return "{0}배 더 쎈 주먹을 가집니다!"; } }
	}

	internal class Status_SkillSight : Status_Skill {
		public override int MaxLevel { get { return 50; } }
		public override float EffectAmount(int level) { return 1 + (level * 0.01f); }
		public override long NextLevelCost(int level) { return 943 + (level + 1) * 343; }
		public override string Explain { get { return "{0}배 더 멀리 볼 수 있습니다!"; } }
	}

	internal class Status_SkillSpeed : Status_Skill {
		public override int MaxLevel { get { return 20; } }
		public override float EffectAmount(int level) { return 1 + (level * 0.01f); }
		public override long NextLevelCost(int level) { return 1542 + (level + 1) * 542; }
		public override string Explain { get { return "이동속도가 {0}배 증가합니다!"; } }
	}

	public class Status_SkillHealth : Status_Skill {
        public override int MaxLevel { get { return 150; } }
        public override float EffectAmount(int level) { return level * 1; }
        public override long NextLevelCost(int level) { return 653 + (level + 1) * 83; }
		public override string Explain { get { return "체력이 {0} 만큼 증가합니다!"; } }
	}

    public class Status_SkillArmor : Status_Skill {
        public override int MaxLevel { get { return 10; } }
        public override float EffectAmount(int level) { return level * 1; }
        public override long NextLevelCost(int level) { return 20000 + (level) * 8311; }
		public override string Explain { get { return "방어도가 {0} 만큼 증가합니다!"; } }
	}

    [Serializable]
    public class Data_Skill {
        [SerializeField] private eSkillType _type;
        public eSkillType SkillType { get { return _type; } }

        [SerializeField] private int _level;
        public int Level { get { return _level; } }

        public Data_Skill(eSkillType type) {
            this._type = type;
            this._level = 0;
        }

        public void Add() {
            _level++;
        }
    }
}
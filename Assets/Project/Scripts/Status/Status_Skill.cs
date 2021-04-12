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

    }

    public class Status_SkillHealth : Status_Skill {
        public override int MaxLevel { get { return 100; } }
        public override float EffectAmount(int level) { return level * 1; }
        public override long NextLevelCost(int level) { return (level + 1) * 1000; }
    }

    public class Status_SkillArmor : Status_Skill {
        public override int MaxLevel { get { return 5; } }
        public override float EffectAmount(int level) { return level * 1; }
        public override long NextLevelCost(int level) { return (level + 1) * 20000; }
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
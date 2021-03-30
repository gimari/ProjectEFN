using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {
    public class Status_Base {

        /// <summary>
        /// 원래 status 는 엑셀같은 외부정보로 받아와야 하지만 간단하게 하기 위해 이런 방식을 취한다.
        /// 게임이 더 커진다면 좋겟지만 안되면 이상태로 냅둔다.
        /// </summary>
        public static Status_Base GetStatus(eItemType item) {
            switch (item) {
                case eItemType.Weapon_MP443:
                    return new Status_MP443();

                case eItemType.Armor_6B3TM:
                    return new Status_6B3TM();

                case eItemType.AMMO_9X19AP:
                    return new Status_9X19AP();

                default:
                    return null;
            }
        }

        public virtual bool Stackable { get { return false; } }

        public virtual int MaxStackSize { get { return 1; } }

        public virtual bool DisplayStack { get { return false; } }

        public virtual bool Useable { get { return false; } }

        public virtual bool Fireable { get { return false; } }

        public virtual eItemType[] RequireItem() { return null; }
    }
}
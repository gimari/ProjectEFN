using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {
    public class Status_MP443 : Status_Base {

        public override eItemType[] RequireItem { get { return new eItemType[] { eItemType.AMMO_9X19AP }; } }

        public override bool Fireable { get { return true; } }
    }
}
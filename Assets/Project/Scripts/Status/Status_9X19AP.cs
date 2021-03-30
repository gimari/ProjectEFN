using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {
    public class Status_9X19AP : Status_Base {

        public override bool Stackable { get { return true; } }

        public override int MaxStackSize { get { return 100; } }

        public override bool DisplayStack { get { return true; } }
    }
}
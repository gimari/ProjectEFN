using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN.Game {
    public class Actor_Box : Actor_Base {
        protected override void OnTriggerEnter2D(Collider2D other) {
            Destroy(this.gameObject);
        }
    }
}
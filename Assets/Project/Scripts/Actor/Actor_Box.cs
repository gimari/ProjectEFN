using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFN.Game {
    public class Actor_Box : Actor_Base {
        protected override void OnTriggerEnter2D(Collider2D other) {
            Global_Actor.Interactable.Add(this.gameObject);
        }

        protected override void OnTriggerExit2D(Collider2D other) {
            Global_Actor.Interactable.Remove(this.gameObject);
        }
    }
}
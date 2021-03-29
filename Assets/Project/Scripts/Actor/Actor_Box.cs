using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFN.Game {
    public class Actor_Box : Actor_Base {
        protected override void OnAwake() {
            base.OnAwake();

            _actorInventory = new Inventory_Item();
            _actorInventory.MaxDisplayIndex = 4;

            Data_Item item1 = new Data_Item(eItemType.Armor_6B3TM);
            item1.Stackable = false;
            item1.StoredInventory = _actorInventory;

            Data_Item item2 = new Data_Item(eItemType.Weapon_MP443);
            item2.Stackable = false;
            item2.StoredInventory = _actorInventory;

            _actorInventory.AddInventory(item1);
            _actorInventory.AddInventory(item2);
        }

        protected override void OnTriggerEnter2D(Collider2D other) {
            Global_Actor.Interactable.Add(this.gameObject);
        }

        protected override void OnTriggerExit2D(Collider2D other) {
            Global_Actor.Interactable.Remove(this.gameObject);
        }
    }
}
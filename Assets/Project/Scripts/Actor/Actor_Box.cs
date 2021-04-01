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

            Data_Item item1 = new Data_Item(eItemType.WEAPON_ASVAL);
            item1.StoredInventory = _actorInventory;

            Data_Item item2 = new Data_Item(eItemType.Weapon_MP443);
            item2.StoredInventory = _actorInventory;

            Data_Item item3 = new Data_Item(eItemType.CONS_FIRSTAID);
            item3.StoredInventory = _actorInventory;

            Data_Item item4 = new Data_Item(eItemType.AMMO_9X39SP5);
            item4.StoredInventory = _actorInventory;
            item4.StackCount = 20;

            _actorInventory.AddInventory(item1);
            _actorInventory.AddInventory(item2);
            _actorInventory.AddInventory(item3);
            _actorInventory.AddInventory(item4);
        }

        protected override void OnTriggerEnter2D(Collider2D other) {
            Global_Actor.Interactable.Add(this.gameObject);
        }

        protected override void OnTriggerExit2D(Collider2D other) {
            Global_Actor.Interactable.Remove(this.gameObject);
        }
    }
}
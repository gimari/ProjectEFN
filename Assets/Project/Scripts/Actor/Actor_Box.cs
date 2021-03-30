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
            //item1.Stackable = false;
            item1.StoredInventory = _actorInventory;

            Data_Item item2 = new Data_Item(eItemType.Weapon_MP443);
            //item2.Stackable = false;
            item2.StoredInventory = _actorInventory;

            Data_Item item3 = new Data_Item(eItemType.AMMO_9X39SP5);
            //item3.Stackable = true;
            item3.StoredInventory = _actorInventory;
            item3.StackCount = 20;
            //item3.MaxStackCount = 100;

            _actorInventory.AddInventory(item1);
            _actorInventory.AddInventory(item2);
            _actorInventory.AddInventory(item3);
        }

        protected override void OnTriggerEnter2D(Collider2D other) {
            Global_Actor.Interactable.Add(this.gameObject);
        }

        protected override void OnTriggerExit2D(Collider2D other) {
            Global_Actor.Interactable.Remove(this.gameObject);
        }
    }
}
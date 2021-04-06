using EFN.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace EFN {
    public class Panel_ItemModify : MonoBehaviour {

        [SerializeField] private Graphic_FadePop _popup = default;
        [SerializeField] private GameObject _btnCollect = default;

        private Graphic_ItemSlot _itemInfo = null;

        public void SetInfo(Graphic_ItemSlot slot) {
            this._popup.Show();
            this._itemInfo = slot;

            if (null != Global_Actor.SelfPlayer) {
                _btnCollect.SetActive(slot.TargetData.StoredInventory != Global_Actor.SelfPlayer.ActorInventory);
            } else {
                _btnCollect.SetActive(slot.TargetData.StoredInventory != Global_SelfPlayerData.SelfInventory);
            }

            this.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        }

        public void EndInfo() {
            this._popup.Hide();
            this._itemInfo = null;
        }

        public void OnClickCollect() {

            if (null != Global_Actor.SelfPlayer) {
                Global_Actor.SelfPlayer.ActorInventory.AddInventory(_itemInfo.TargetData);
            } else {
                Global_SelfPlayerData.SelfInventory.AddInventory(_itemInfo.TargetData);
            }

            EndInfo();
        }

        public void OnModifyPointerClick() {
            EndInfo();
        }
    }
}
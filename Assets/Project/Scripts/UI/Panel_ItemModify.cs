using EFN.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;

namespace EFN {

    public class ModifyPanelData {
        public List<ModifyPanelInfo> InfoList = new List<ModifyPanelInfo>();
    }

    public class ModifyPanelInfo {
        public Action OnClickAction = null;
        public string BtnName = "";
    }

    public class Panel_ItemModify : MonoBehaviour {

        [SerializeField] private Graphic_FadePop _popup = default;
        [SerializeField] private Graphic_LayoutList _modifyBtnList = default;
		[SerializeField] private Transform _modifyPanel = default;

        public void SetInfo(ModifyPanelData panelData) {
            this._popup.Show();
            _modifyBtnList.Init();

            foreach (ModifyPanelInfo btnInfo in panelData.InfoList) {
                Content_ModifyBtn content = _modifyBtnList.AddWith<Content_ModifyBtn>();
                content.OnUpdate(btnInfo);
            }

			_modifyPanel.position = (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        }

        public void EndInfo() {
            this._popup.Hide();
        }

        public void OnModifyPointerClick() {
            EndInfo();
        }
    }
}
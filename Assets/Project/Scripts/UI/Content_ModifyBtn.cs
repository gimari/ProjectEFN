using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EFN {
    public class Content_ModifyBtn : MonoBehaviour, IListContent {

        [SerializeField] private Text[] _btnName = default;

        private Action _onClickAction = null;

        public void OnUpdate(ModifyPanelInfo info) {
            this._onClickAction = info.OnClickAction;
            
            foreach(Text btnTxt in _btnName) {
                btnTxt.text = info.BtnName;
            }
        }

        public void OnClick() {
            _onClickAction?.Invoke();
        }

        public void OnListCleared() {
            _onClickAction = null;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EFN {
    [RequireComponent(typeof(Graphic_NumericText))]
    public class Graphic_CokeCount : MonoBehaviour {

        private Graphic_NumericText _txtcoke = null;

        private void Awake() {
            _txtcoke = GetComponent<Graphic_NumericText>();
            Global_UIEvent.RegisterUIEvent(eEventType.UpdateCoke, UpdateCoke);
        }

        private void OnEnable() {
            _txtcoke.SetNumericTextWithOutAnimation(Global_SelfPlayerData.CokeAmount, MoneyFormat.JustComma);
        }

        public void UpdateCoke() {
			if (true == _txtcoke.gameObject.activeInHierarchy) {
				_txtcoke.NumberTextAnimate(Global_SelfPlayerData.CokeAmount, MoneyFormat.JustComma);
			} else {
				_txtcoke.SetNumericTextWithOutAnimation(Global_SelfPlayerData.CokeAmount, MoneyFormat.JustComma);
			}
		}
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN.Game {
	public class Graphic_PlayerHat : MonoBehaviour {

		[SerializeField] private GameObject _hatObject = default;


		public void SetItem(Data_Item item) {

			if (null != _hatObject) {
				Destroy(_hatObject.gameObject);
			}

			if (null == item) {
				return;
			}

			GameObject targetPrefab = Global_ResourceContainer.GetHatPrefab(item.ItemType.ToString());
			if (null == targetPrefab) {
				return;
			}

			_hatObject = Instantiate(targetPrefab, this.transform);
		}

	}
}
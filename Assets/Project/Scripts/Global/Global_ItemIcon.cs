using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace EFN {
	public class Global_ItemIcon : MonoBehaviour {

		[SerializeField] private Sprite[] _iconSpriteList = default;

		private static Global_ItemIcon _instance = null;

		private void Awake() {
			_instance = this;
		}

		public static Sprite GetSprite(string spriteName) {
			if (null == _instance) {
				EFN.Global_Common.LogError("CANNOT FIND SPRITE : NO INSTANCE");
				return null;
			}

			var rv = from list in _instance._iconSpriteList
					 where list.name == spriteName
					 select list;

			return rv.First();
		}
	}
}
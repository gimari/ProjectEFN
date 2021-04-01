using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace EFN {
	public class Global_ResourceContainer : MonoBehaviour {

		[SerializeField] private Sprite[] _iconSpriteList = default;
		[SerializeField] private GameObject[] _armPrefabList = default;

		private static Global_ResourceContainer _instance = null;

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

			if (rv.Count() <= 0) {
				EFN.Global_Common.LogError("CANNOT FIND SPRITE : NO RESULT");
				return null;
			}

			return rv.First();
		}

		public static GameObject GetGunPrefab(string spriteName) {
			if (null == _instance) {
				EFN.Global_Common.LogError("CANNOT FIND PREFAB : NO INSTANCE");
				return null;
			}

			var rv = from list in _instance._armPrefabList
					 where list.name == spriteName
					 select list;

			if (rv.Count() <= 0) {
				EFN.Global_Common.LogError("CANNOT FIND PREFAB : NO RESULT");
				return null;
			}

			return rv.First();
		}
	}
}
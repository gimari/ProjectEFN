using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {

	/// <summary>
	/// Global_SelfPlayerData 는 DD 에 붙어서 유저 데이터 관련해 많은 일을 해야함.
	/// </summary>
	public class Global_SelfPlayerData : MonoBehaviour, IDontDestroy {

		private Inventory_SelfPlayer _selfInventory = null;
		public static Inventory_SelfPlayer SelfInventory {
			get { return _instance._selfInventory; }
		}

		private Inventory_Item _stashInventory = null;
		public static Inventory_Item StashInventory {
			get { return _instance._stashInventory; }
		}

		private static Global_SelfPlayerData _instance = null;

		public static void Save() {
			if (null == _instance) { return; }

			EFNEncrypt enc = new EFNEncrypt();

			string parsed = JsonUtility.ToJson(_instance._selfInventory);
			string first = enc.Encrypt(parsed);

			PlayerPrefs.SetString("_selfInventory", first);

			parsed = JsonUtility.ToJson(_instance._stashInventory);
			first = enc.Encrypt(parsed);

			PlayerPrefs.SetString("_stashInventory", first);

			PlayerPrefs.Save();
		}

		public void Load() {

			EFNEncrypt enc = new EFNEncrypt();

			string load = PlayerPrefs.GetString("_selfInventory");

			if (false == string.IsNullOrEmpty(load)) {
				string first = enc.Decrypt(load);
				Inventory_SelfPlayer parsed = JsonUtility.FromJson<Inventory_SelfPlayer>(first);
				_selfInventory = parsed;
			}

			load = PlayerPrefs.GetString("_stashInventory");

			if (false == string.IsNullOrEmpty(load)) {
				string first = enc.Decrypt(load);
				Inventory_Item parsed = JsonUtility.FromJson<Inventory_Item>(first);
				_stashInventory = parsed;
			}
		}

		public void Init() {
			_instance = this;

			_selfInventory = new Inventory_SelfPlayer();

			_stashInventory = new Inventory_Item();
			_stashInventory.MaxDisplayIndex = 40;

			Load();
		}
	}
}
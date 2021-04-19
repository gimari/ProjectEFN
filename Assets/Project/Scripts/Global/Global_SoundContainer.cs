using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {

	public enum eSoundClip {
		None = 0,
		Concrete0,
		Concrete1,
		Concrete2,
		Concrete3,
		Concrete4,
		Concrete5,
		AK,
		Light,
		Light_silence,
		Mp5,
		Rifle,
		Rifle_silence,
		Shotgun,
		Knife_Swing,
		Knife_Hit,
		Ricochets0,
		Ricochets1,
		Ricochets2,
		Ricochets3,
	}

	public class Global_SoundContainer : MonoBehaviour {

		[EnumNamedArray(typeof(eSoundClip))]
		[SerializeField] private AudioClip[] _soundClip = default;

		private static Global_SoundContainer _instance;

		private void Awake() {
			_instance = this;
		}

		public static AudioClip GetWalk() {
			return _instance._soundClip[Random.Range((int)eSoundClip.Concrete0, (int)eSoundClip.Concrete5 + 1)];
		}

		public static AudioClip GetRicochet() {
			return _instance._soundClip[Random.Range((int)eSoundClip.Ricochets0, (int)eSoundClip.Ricochets3 + 1)];
		}

		public static AudioClip GetFireSound(eItemType item) {
			switch (item) {
				case eItemType.Weapon_MP443:
					return _instance._soundClip[(int)eSoundClip.Light];

				case eItemType.WEAPON_ASVAL:
					return _instance._soundClip[(int)eSoundClip.Rifle_silence];

				case eItemType.WEAPON_KS23M:
					return _instance._soundClip[(int)eSoundClip.Shotgun];

				case eItemType.WEAPON_MP5:
					return _instance._soundClip[(int)eSoundClip.Mp5];

				case eItemType.WEAPON_AKM:
				case eItemType.WEAPON_SKS:
					return _instance._soundClip[(int)eSoundClip.AK];

				case eItemType.WEAPON_HK416:
					return _instance._soundClip[(int)eSoundClip.Rifle];

				case eItemType.WEAPON_VECTOR:
				case eItemType.WEAPON_M1911:
					return _instance._soundClip[(int)eSoundClip.Light_silence];

				case eItemType.WEAPON_RECORDER:
				case eItemType.WEAPON_DAGGER:
					return _instance._soundClip[(int)eSoundClip.Knife_Swing];

				default:
					return null;
			}
		}

		public static AudioClip GetSound(eSoundClip clip) {
			return _instance._soundClip[(int)clip];
		}
	}
}
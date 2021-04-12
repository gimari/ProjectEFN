using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {

	public class SelfGameEndData {
		public bool IsKIA = true;
	}

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

		private Inventory_Skill _skillInventory = null;
		public static Inventory_Skill SkillInventory {
			get { return _instance._skillInventory; }
		}

		private SelfGameEndData _gameEndData = null;
		public static SelfGameEndData GameEndData {
			get { return _instance._gameEndData; }
		}

		/// <summary>
		/// 특정한 스킬 타입의 스킬레벨을 1단계 올리려고 한다.
		/// coke 소모에 대한 적절한 처리가 동반되어 있다.
		/// </summary>
		public static eErrorCode TryAddSkill(eSkillType skillType) {
			if (null == _instance) {
				return eErrorCode.Fail;
			}

			if (skillType == eSkillType.None) {
				Global_Common.LogError("올바르지 않은 skill type!! : None");
				return eErrorCode.Fail;
			}

			Status_Skill status = Status_Skill.GetStatus(skillType);
			int level = _instance._skillInventory.Get(skillType);

			if (status.MaxLevel <= level) {
				return eErrorCode.MaxSkillLevel;
			}

			long cost = status.NextLevelCost(level);

			eErrorCode rv = ConsumeCoke(cost);
			if (rv != eErrorCode.Success) {
				return rv;
			}

			_instance._skillInventory.Add(skillType);

			return eErrorCode.Success;
		}

		public static float GetSkillAmount(eSkillType skill) {
			Status_Skill status = Status_Skill.GetStatus(skill);
			if (null == status) { return 0; }

			int level = _instance._skillInventory.Get(skill);
			return status.EffectAmount(level);
		}

		/// <summary>
		/// 유저가 죽었을때의 처리.
		/// 인벤토리를 깨끗히 비우고 gameenddata 를 기록한다.
		/// </summary>
		public static void SetKilledInAction() {
			if (null == _instance) {
				return;
			}

			// 인벤토리를 깨끗하게 비워준다.
			SelfInventory.ClearInventoryWithDie();

			SelfGameEndData data = new SelfGameEndData();
			data.IsKIA = true;

			_instance._gameEndData = data;
			Global_UIEvent.CallUIEvent<string>(ePermanetEventType.TryChangeScene, eSceneName.SceneMain.ToString());
		}

		/// <summary>
		/// 유저가 성공적으로 탈출했을때 처리
		/// </summary>
		public static void SetExtract() {
			if (null == _instance) {
				return;
			}

			SelfGameEndData data = new SelfGameEndData();
			data.IsKIA = false;

			_instance._gameEndData = data;
			Global_UIEvent.CallUIEvent<string>(ePermanetEventType.TryChangeScene, eSceneName.SceneMain.ToString());
		}

		/// <summary>
		/// COKE 관련 처리 부분
		/// </summary>
		private long _cokeAmount = 0;
		public static long CokeAmount {
			get { 
				return _instance._cokeAmount; 
			}

			set {
				// 간단하게 바로 써버림
				_instance._cokeAmount = value;

				Global_UIEvent.CallUIEvent(eEventType.UpdateCoke);
				PlayerPrefs.SetString("_cokeAmount", value.ToString());
				PlayerPrefs.Save();
			}
		}

		public static eErrorCode ConsumeCoke(long amount) {
#if EFN_DEBUG
			if (Global_DebugConfig.InfiniteMoney) {
				return eErrorCode.Success;
			}
#endif

			if (_instance._cokeAmount < amount) {
				return eErrorCode.NotenoughCoke;
			}

			CokeAmount = _instance._cokeAmount - amount;
			return eErrorCode.Success;
		}

		private void LoadCoke() {
			long amount = 0;
			if (long.TryParse(PlayerPrefs.GetString("_cokeAmount"), out amount)) {
				_cokeAmount = amount;
			}
		}

		private static Global_SelfPlayerData _instance = null;

		public static void Save() {
			if (null == _instance) { return; }

			EFNEncrypt enc = new EFNEncrypt();

			// self inventory
			string parsed = JsonUtility.ToJson(_instance._selfInventory);
			string first = enc.Encrypt(parsed);

			PlayerPrefs.SetString("_selfInventory", first);

			// stash inventory
			parsed = JsonUtility.ToJson(_instance._stashInventory);
			first = enc.Encrypt(parsed);

			PlayerPrefs.SetString("_stashInventory", first);

			//skill inventory
			parsed = JsonUtility.ToJson(_instance._skillInventory);
			first = enc.Encrypt(parsed);

			PlayerPrefs.SetString("_skillInventory", first);

			PlayerPrefs.Save();
		}

		/// <summary>
		/// 인벤토리 등 정보를 로드한다.
		/// 정보 로드 순서에 특히 유의.
		/// </summary>
		public void LoadInventory() {

			EFNEncrypt enc = new EFNEncrypt();

			// skill inventory. 스킬 인벤토리가 무조건 제일 먼저 로드되어야 한다!
			_skillInventory = new Inventory_Skill();

			string load = PlayerPrefs.GetString("_skillInventory");

			if (false == string.IsNullOrEmpty(load)) {
				string first = enc.Decrypt(load);
				Inventory_Skill parsed = JsonUtility.FromJson<Inventory_Skill>(first);
				_skillInventory = parsed;
			}

			// self inventory
			_selfInventory = new Inventory_SelfPlayer();

			load = PlayerPrefs.GetString("_selfInventory");

			if (false == string.IsNullOrEmpty(load)) {
				string first = enc.Decrypt(load);
				Inventory_SelfPlayer parsed = JsonUtility.FromJson<Inventory_SelfPlayer>(first);
				_selfInventory = parsed;
			}

			_selfInventory.MaxDisplayIndex = (int)ePlayerSlotType.QuickSlotStart + 5;

			// stash inventory
			_stashInventory = new Inventory_Item();

			load = PlayerPrefs.GetString("_stashInventory");

			if (false == string.IsNullOrEmpty(load)) {
				string first = enc.Decrypt(load);
				Inventory_Item parsed = JsonUtility.FromJson<Inventory_Item>(first);
				_stashInventory = parsed;
			}

			_stashInventory.MaxDisplayIndex = 40;

			// dealer inventory (예정)
		}

		public void Init() {
			_instance = this;

			LoadInventory();
			LoadCoke();
		}

		/*
		/// <summary>
		/// 현재 기록되어있는 config 불러오기
		/// 올바르지 않는 config 존재시 null 반환.
		/// </summary>
		private Dictionary<string, string> LoadDownloadConfig() {

			Dictionary<string, string> rv = new Dictionary<string, string>();
			rv.Clear();

			if (false == File.Exists(LNP.Constant.GetDirectory + LNP.Constant.ConfigName)) {
				return rv;
			}

			FileStream fileStream = new FileStream(LNP.Constant.GetDirectory + LNP.Constant.ConfigName, FileMode.Open);
			StreamReader streamReader = new StreamReader(fileStream);

			while (!streamReader.EndOfStream) {

				string line = streamReader.ReadLine();
				string[] array = line.Split(',');

				if (array.Length < 2 || array.Length > 3) {
					break;
				}

				rv.Add(array[0], array[1]);
			}

			streamReader.Close();
			fileStream.Close();

			return rv;
		}

		private void SaveDownloadConfig() {

			FileStream fileStream = new FileStream(LNP.Constant.GetDirectory + LNP.Constant.ConfigName, FileMode.Create, FileAccess.Write);
			StreamWriter streamReader = new StreamWriter(fileStream, System.Text.Encoding.Unicode);

			foreach (string line in this._downloadedConfig) {
				streamReader.WriteLine(line);
			}

			streamReader.Close();
		}*/
	}
}
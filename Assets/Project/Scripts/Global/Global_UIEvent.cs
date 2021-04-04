using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {
	public enum eEventType {
		None = 0,

		OpenStartMenu,
		OpenTradeSelectPanel,
		OpenDealerPanel,

		OpenMainInven,				// 메인 씬에서 인벤 열음

		ToggleIngameInven,			// 인게임 인벤토리 열음
		OpenInvenWithRoot,			// 루팅열면서 인벤 열음

		TryPickSlot,				// 아이템 슬롯 픽킹햇음.
		EndPickSlot,				// 아이템 슬롯 픽 끝
		RegistQuickSlot,			// 퀵슬롯 등록

		TryInteractWith,			// 특정한 액터랑 인터렉트 시도
		UpdateUserInventory,		// 인벤토리 업데이트됨

		ShowFloatingDamage,

		OnEndFocus,
	}

	public enum ePermanetEventType {
		None = 0,
	}

	/// <summary>
	/// delegate 함수 리스트를 들고있는 구조체.
	/// </summary>
	public struct DelegateList {
		private List<Delegate> _funcList;
		public List<Delegate> GetEventList() { return _funcList; }

		public void AddFunction<T>(T func) {
			if (null == _funcList) {
				_funcList = new List<Delegate>();
			}

			_funcList.Add(func as Delegate);
		}

		public void RemoveFunction<T>(T func) {
			if (null == _funcList) {
				return;
			}

			_funcList.Remove(func as Delegate);
		}

		public bool IsEmpty() {
			if (null == _funcList) {
				return true;
			}

			if (_funcList.Count <= 0) {
				return true;
			}

			return false;
		}
	}

	public class Global_UIEvent {

		// delegate 변수로 사용할 것들.
		public delegate void EventFunc();
		public delegate void EventFunc<T>(T param1);
		public delegate void EventFunc<T1, T2>(T1 param1, T2 param2);
		public delegate void EventFunc<T1, T2, T3>(T1 param1, T2 param2, T3 param3);

		// 실제 ui event 함수들을 들고있는 dictionary
		private static Dictionary<int, DelegateList> _uiEventTable = new Dictionary<int, DelegateList>();
		private static Dictionary<int, DelegateList> _permanentEventTable = new Dictionary<int, DelegateList>();

		////////////////////////////////////////////////////////////////////////////////////////////////
		public static void ClearUIEvent() {

			if (null != _uiEventTable) {
				_uiEventTable.Clear();
			}

			Debug.Log("UI Event Cleared");
		}

		// 주의 : 씬이 바뀔 때 permanent list 는 클리어되지 않아야 한다.
		public static void OnChangeScene() {
			if (null != _uiEventTable) {
				_uiEventTable.Clear();
			}
		}

		private static Dictionary<int, DelegateList> TargetTable(Type targetType) {
			if (targetType == typeof(eEventType)) {
				return _uiEventTable;
			} else if (targetType == typeof(ePermanetEventType)) {
				return _permanentEventTable;
			} else {
				return null;
			}
		}

		////////////////////////////////////////////////////////////////////////////////////////////////
		// UI 이벤트 호출 부분. 원하는 이벤트 이름과 인자로 전달할 내용을 전달하면 된다.
		public static void CallUIEvent(System.Enum eventType) {
			int parseType = Convert.ToInt32(eventType);
			Dictionary<int, DelegateList> table = TargetTable(eventType.GetType());

			if (null == table) {
				Debug.LogError("Event table not initialized.");
				return;
			}

			if (false == table.ContainsKey(parseType)) {
				Debug.Log("등록되지 않은 이벤트입니다 : " + eventType);
				return;
			}

			// 해당 이벤트에 달린 모든 함수를 실행.
			List<Delegate> eventList = table[parseType].GetEventList();
			for (int idx = 0; idx < eventList.Count; idx++) {
				(eventList[idx] as EventFunc)();
			}
		}

		public static void CallUIEvent<T>(System.Enum eventType, T param) {
			int parseType = Convert.ToInt32(eventType);
			Dictionary<int, DelegateList> table = TargetTable(eventType.GetType());

			if (null == table) {
				Debug.LogError("Event table not initialized.");
				return;
			}

			if (false == table.ContainsKey(parseType)) {
				Debug.Log("등록되지 않은 이벤트입니다 : " + eventType);
				return;
			}

			// 해당 이벤트에 달린 모든 함수를 실행.
			List<Delegate> eventList = table[parseType].GetEventList();
			foreach (EventFunc<T> func in eventList) {
				func(param);
			}
		}

		public static void CallUIEvent<T1, T2>(System.Enum eventType, T1 param1, T2 param2) {
			int parseType = Convert.ToInt32(eventType);
			Dictionary<int, DelegateList> table = TargetTable(eventType.GetType());

			if (null == table) {
				Debug.LogError("Event table not initialized.");
				return;
			}

			if (false == table.ContainsKey(parseType)) {
				Debug.Log("등록되지 않은 이벤트입니다 : " + eventType);
				return;
			}

			// 해당 이벤트에 달린 모든 함수를 실행.
			List<Delegate> eventList = table[parseType].GetEventList();
			foreach (EventFunc<T1, T2> func in eventList) {
				func(param1, param2);
			}
		}

		public static void CallUIEvent<T1, T2, T3>(System.Enum eventType, T1 param1, T2 param2, T3 param3) {
			int parseType = Convert.ToInt32(eventType);
			Dictionary<int, DelegateList> table = TargetTable(eventType.GetType());

			if (null == table) {
				Debug.LogError("Event table not initialized.");
				return;
			}

			if (false == table.ContainsKey(parseType)) {
				Debug.Log("등록되지 않은 이벤트입니다 : " + eventType);
				return;
			}

			// 해당 이벤트에 달린 모든 함수를 실행.
			List<Delegate> eventList = table[parseType].GetEventList();
			foreach (EventFunc<T1, T2, T3> func in eventList) {
				func(param1, param2, param3);
			}
		}

		////////////////////////////////////////////////////////////////////////////////////////////////
		// UI 이벤트 등록 부분. 인자 개수에 따라서 다른 RegisterUIEvent 함수로 등록하면 된다.
		public static void RegisterUIEvent(System.Enum eventType, EventFunc func) {

			int parseType = Convert.ToInt32(eventType);
			Dictionary<int, DelegateList> table = TargetTable(eventType.GetType());

			if (null == table) {
				Debug.LogError("Event table not initialized.");
				return;
			}

			// 이벤트가 아얘 없었으면 새로 할당해주어야 한다.
			if (false == table.ContainsKey(parseType)) {
				DelegateList list = new DelegateList();
				list.AddFunction<EventFunc>(func);

				table.Add(parseType, list);

			} else {
				// 이벤트에 함수 추가.
				table[parseType].AddFunction<EventFunc>(func);
			}
		}

		public static void RemoveUIEvent(System.Enum eventType, EventFunc func) {
			int parseType = Convert.ToInt32(eventType);
			Dictionary<int, DelegateList> table = TargetTable(eventType.GetType());

			if (null == table) {
				Debug.LogError("Event table not initialized.");
				return;
			}

			// 이벤트가 있어야 지워줄 것도 있다..
			if (true == table.ContainsKey(parseType)) {
				table[parseType].RemoveFunction<EventFunc>(func);
			}
		}

		public static void RegisterUIEvent<T>(System.Enum eventType, EventFunc<T> func) {
			int parseType = Convert.ToInt32(eventType);
			Dictionary<int, DelegateList> table = TargetTable(eventType.GetType());

			if (null == table) {
				Debug.LogError("Event table not initialized.");
				return;
			}

			// 이벤트가 아얘 없었으면 새로 할당해주어야 한다.
			if (false == table.ContainsKey(parseType)) {
				DelegateList list = new DelegateList();
				list.AddFunction<EventFunc<T>>(func);

				table.Add(parseType, list);
			} else {
				// 이벤트에 함수 추가.
				table[parseType].AddFunction<EventFunc<T>>(func);
			}
		}

		public static void RemoveUIEvent<T>(System.Enum eventType, EventFunc<T> func) {
			int parseType = Convert.ToInt32(eventType);
			Dictionary<int, DelegateList> table = TargetTable(eventType.GetType());

			if (null == table) {
				Debug.LogError("Event table not initialized.");
				return;
			}

			// 이벤트가 있어야 지워줄 것도 있다..
			if (true == table.ContainsKey(parseType)) {
				table[parseType].RemoveFunction<EventFunc<T>>(func);
			}
		}

		public static void RegisterUIEvent<T1, T2>(System.Enum eventType, EventFunc<T1, T2> func) {
			int parseType = Convert.ToInt32(eventType);
			Dictionary<int, DelegateList> table = TargetTable(eventType.GetType());

			if (null == table) {
				Debug.LogError("Event table not initialized.");
				return;
			}

			// 이벤트가 아얘 없었으면 새로 할당해주어야 한다.
			if (false == table.ContainsKey(parseType)) {
				DelegateList list = new DelegateList();
				list.AddFunction<EventFunc<T1, T2>>(func);

				table.Add(parseType, list);

			} else {
				// 이벤트에 함수 추가.
				table[parseType].AddFunction<EventFunc<T1, T2>>(func);
			}

		}

		public static void RegisterUIEvent<T1, T2, T3>(System.Enum eventType, EventFunc<T1, T2, T3> func) {
			int parseType = Convert.ToInt32(eventType);
			Dictionary<int, DelegateList> table = TargetTable(eventType.GetType());

			if (null == table) {
				Debug.LogError("Event table not initialized.");
				return;
			}

			// 이벤트가 아얘 없었으면 새로 할당해주어야 한다.
			if (false == table.ContainsKey(parseType)) {
				DelegateList list = new DelegateList();
				list.AddFunction<EventFunc<T1, T2, T3>>(func);

				table.Add(parseType, list);

			} else {
				// 이벤트에 함수 추가.
				table[parseType].AddFunction<EventFunc<T1, T2, T3>>(func);
			}
		}

		public static class Focus {

			private static GameObject _focusTarget = null;

			/// <summary>
			/// 기적의 C# 스크립트 덕분에 댕글링 걱정은 필요 없다.
			/// </summary>
			public static bool IsFocusing {
				get { return _focusTarget != null; }
			}

			/// <summary>
			/// SetFocus 에 target 이 null 이오던말던 신경쓰지 않는다.
			/// </summary>
			public static void SetFocus(GameObject target) {
				_focusTarget = target;
			}

			public static void EndFocus() {
				_focusTarget = null;
			}
		}
	}
}
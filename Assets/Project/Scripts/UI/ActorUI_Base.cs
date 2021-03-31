using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EFN.Global_UIEvent;

namespace EFN.Game {
	public enum eActorUIType {
		None = 0,

		StartBehaviour,
		EndBehaviour,
	}

	public class ActorUI_Base : MonoBehaviour {

		// 실제 ui event 함수들을 들고있는 dictionary
		private Dictionary<int, DelegateList> _uiEventTable = new Dictionary<int, DelegateList>();

		////////////////////////////////////////////////////////////////////////////////////////////////
		public void ClearUIEvent() {

			if (null != _uiEventTable) {
				_uiEventTable.Clear();
			}

			Debug.Log("UI Event Cleared");
		}

		private Dictionary<int, DelegateList> TargetTable(Type targetType) {
			return _uiEventTable;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////
		// UI 이벤트 호출 부분. 원하는 이벤트 이름과 인자로 전달할 내용을 전달하면 된다.
		public void CallUIEvent(System.Enum eventType) {
			int parseType = Convert.ToInt32(eventType);
			Dictionary<int, DelegateList> table = TargetTable(eventType.GetType());

			if (null == _uiEventTable) {
				Debug.LogError("Event table not initialized.");
				return;
			}

			if (false == _uiEventTable.ContainsKey(parseType)) {
				Debug.Log("등록되지 않은 이벤트입니다 : " + eventType);
				return;
			}

			// 해당 이벤트에 달린 모든 함수를 실행.
			List<Delegate> eventList = table[parseType].GetEventList();
			for (int idx = 0; idx < eventList.Count; idx++) {
				(eventList[idx] as EventFunc)();
			}
		}

		public void CallUIEvent<T>(System.Enum eventType, T param) {
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

		public void CallUIEvent<T1, T2>(System.Enum eventType, T1 param1, T2 param2) {
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

		public void CallUIEvent<T1, T2, T3>(System.Enum eventType, T1 param1, T2 param2, T3 param3) {
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
		public void RegisterUIEvent(System.Enum eventType, EventFunc func) {

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

		public void RemoveUIEvent(System.Enum eventType, EventFunc func) {
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

		public void RegisterUIEvent<T>(System.Enum eventType, EventFunc<T> func) {
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

		public void RemoveUIEvent<T>(System.Enum eventType, EventFunc<T> func) {
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

		public void RegisterUIEvent<T1, T2>(System.Enum eventType, EventFunc<T1, T2> func) {
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

		public void RegisterUIEvent<T1, T2, T3>(System.Enum eventType, EventFunc<T1, T2, T3> func) {
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
	}
}
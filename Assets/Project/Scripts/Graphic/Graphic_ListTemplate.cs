using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IListContent {
	void OnListCleared();
}

public class Graphic_ListTemplate : MonoBehaviour, IEnumerable {
	[SerializeField] protected GameObject _targetObject;

	protected int _currentCount = 0;
	public int CurrentCount { get { return _currentCount; } }

	protected Vector2 _startSize = Vector2.zero;
	protected Vector2 _childSize;
	protected bool _isInitialized = false;

	private void Awake() {
		if (true == _isInitialized) { return; }

		OnAwake();
	}

	/// <summary>
	/// 수동으로 init 하고 싶을때.
	/// 수동으로 하면 awake 에서 불리지 않는다.
	/// </summary>
	public virtual void Init() {
		OnAwake();
	}

	protected virtual void OnAwake() {

		// 변수 세팅
		_startSize = GetComponent<RectTransform>().sizeDelta;

		_childSize = _targetObject.GetComponent<RectTransform>().sizeDelta;

		// 시작할 때 클리어.
		Clear();

		_isInitialized = true;
	}

	/// <summary>
	/// 사전등록한 오브젝트를 Add 한다.
	/// </summary>
	/// <returns></returns>
	public GameObject Add() {
		GameObject rv = null;

		if (transform.childCount <= _currentCount) {
			rv = Instantiate(_targetObject, this.transform);
		} else {
			rv = transform.GetChild(_currentCount).gameObject;
		}

		rv.SetActive(true);

		_currentCount++;
		CalculateSize();

		return rv;
	}

	/// <summary>
	/// 사전등록한 오브젝트를 Add 하고 T 컴포넌트를 바로 얻고싶다.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public virtual T AddWith<T>() {
		T rv;
		GameObject go = null;

		if (transform.childCount <= _currentCount) {
			go = Instantiate(_targetObject, this.transform);
		} else {
			go = transform.GetChild(_currentCount).gameObject;
		}

		go.SetActive(true);
		rv = go.GetComponent<T>();

		_currentCount++;
		CalculateSize();

		if (null == rv) { return default(T); } 
		else { return rv; }
	}

	protected virtual void CalculateSize() { }

	public virtual void Remove(int index) {

		if (transform.childCount < index) {
			return;
		}

		Transform child = transform.GetChild(index);

		child.gameObject.SetActive(false);
		child.SetAsLastSibling();

		_currentCount--;
		CalculateSize();
	}

	public virtual T Get<T>(int index) {
		Transform rv = Get(index);
		if (null == rv) { return default; }

		return rv.GetComponent<T>();
	}

	public virtual Transform Get(int index) {

		if (transform.childCount < index) {
			Debug.LogError("Index Out Of Bound!");
			return null;
		}

		int idx = 0;
		foreach(Transform child in transform) {
			if(true == child.gameObject.activeSelf) {
				if (idx == index) {
					return child;
				}

				idx++;
			}
		}

		Debug.LogError("Index Out Of Bound!");
		return null;
	}

	public virtual void Clear() {
		foreach (Transform child in this.transform) {
			child.gameObject.SetActive(false);

			IListContent content = child.GetComponent<IListContent>();
			if (null != content) { content.OnListCleared(); }
		}

		_currentCount = 0;
		CalculateSize();
	}

	public IEnumerator GetEnumerator() {
		for (int idx = 0; idx < transform.childCount; idx++) {
			Transform child = transform.GetChild(idx);

			if (false == child.gameObject.activeSelf) {
				continue;
			}

			yield return child;
		}
	}
}

//Author : WJ Choi
#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//숫자 연출 기능 제공하는 그래픽클래스
public class Graphic_NumericText : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private bool _unscaledTimer = false;

    [Header("Components")]
    [SerializeField]
    protected Text refText;                   //애니메이션 대상 텍스트

    //현재 진행중인 애니메이션의 포맷
    protected MoneyFormat recentFormat;

    //애니메이션 플레이타이머
    private float timer = 0;

    //목표 값. 스킵시 이 값으로 즉시 대입
    protected long targetValue = 0;
    public long TargetValue { get { return targetValue; } }

    //현재 보여지고 있는 값. Lerp를 위해 저장해둔다.
    protected long _currentValue = 0;
    public long CurrentValue { get { return _currentValue; } }

    //현재 애니메이션 돌고있는 코루틴. 스킵시 Stop
    private Coroutine animateRoutine;

    protected bool animationRoutineAlive = false;
    public virtual bool IsAnimationEnd { get { return !animationRoutineAlive; } }

    private string _prefix = "";

    protected virtual void OnDisable() {
        animationRoutineAlive = false;
        _currentValue = targetValue;

        if (refText != null) {
            UpdateText(_currentValue.ToString(recentFormat));
        }
    }

    //이 값이 항상 앞에 붙어서 출력됨
    public void SetPrefix(string prefix) {
        _prefix = prefix;
    }

    #region Interface
    public void SetNumericTextWithOutAnimation(long targetValue, MoneyFormat format) {
        if (animationRoutineAlive) {
            if (animateRoutine != null) {
                StopCoroutine(animateRoutine);
                animateRoutine = null;
            }

            animationRoutineAlive = false;
        }

        this.targetValue = targetValue;
        _currentValue = targetValue;
        UpdateText(_currentValue.ToString(format));
    }

    public void NumberTextAnimate(long to, MoneyFormat format)
    {
        NumberTextAnimate(_currentValue, to, format);
    }

    public void NumberTextAnimate(long from, long to, MoneyFormat format)
    {
        recentFormat = format;
        _currentValue = from;
        targetValue = to;
        timer = 0;
        if (animationRoutineAlive == false) {
            animateRoutine = StartCoroutine(TextAnimationRoutine(format));
        }
    }

    public void LinearAnimate(long from, long to, float time, MoneyFormat format) {
        recentFormat = format;
        _currentValue = from;
        if (animationRoutineAlive == true) {
            StopCoroutine(animateRoutine);
            animationRoutineAlive = false;
        }
        animateRoutine = StartCoroutine(LinearAnimationRoutine(to, time, format));
    }

    public void LinearAnimate(long to, float time, MoneyFormat format) {
        LinearAnimate(_currentValue, to, time, format);
    }

    public virtual void Skip() {
        if (animationRoutineAlive == true) {
            StopCoroutine(animateRoutine);
            animationRoutineAlive = false;
            OnAnimationEnd();
        }
        
        _currentValue = targetValue;
        UpdateText(_currentValue.ToString(recentFormat));
    }
    #endregion

    
    private IEnumerator TextAnimationRoutine(MoneyFormat format)
    {
        animationRoutineAlive = true;
        timer = 0;

        while (true)
        {
            timer += _unscaledTimer ? Time.unscaledDeltaTime : Time.deltaTime;

            if (timer > 1f) { break; }

            _currentValue = Lerp(_currentValue, targetValue, 0.2f);
            UpdateText(_currentValue.ToString(format));
            yield return null;
        }
        _currentValue = targetValue;
        UpdateText(_currentValue.ToString(format));
        animationRoutineAlive = false;

        OnAnimationEnd();
    }

    private IEnumerator LinearAnimationRoutine(long to, float time, MoneyFormat format) {
        animationRoutineAlive = true;

        targetValue = to;
        timer = 0;
        long startValue = _currentValue;

        while (true) {
            timer += _unscaledTimer ? Time.unscaledDeltaTime : Time.deltaTime;

            if (timer > time) { break; }

            _currentValue = Lerp(startValue, to, timer / time);
            UpdateText(_currentValue.ToString(format));
            yield return null;
        }
        _currentValue = targetValue;
        UpdateText(_currentValue.ToString(format));
        animationRoutineAlive = false;

        OnAnimationEnd();
    }

    protected virtual void UpdateText(string value) {
        refText.text = _prefix + value;
    }

    private long Lerp(long a, long b, float value) {
        decimal result;
        decimal da = a;
        decimal db = b;
        result = da + (db - da) * (decimal)value;

        if (a < b) {
            if (result - (long)result > 0) {//올림
                result += 1;
            }
        }

        return (long)result;
    }

    protected virtual void OnAnimationEnd() {

    }
}

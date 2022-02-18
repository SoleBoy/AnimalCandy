using DragonBones;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeelAnimation : MonoBehaviour
{
    private static KeelAnimation instance;
    public static KeelAnimation Instance { get => instance;}

    private UnityArmatureComponent touchscreen;
    private UnityArmatureComponent win;
    private UnityArmatureComponent warning;
    private UnityArmatureComponent fail;
    private UnityArmatureComponent start;
    private UnityArmatureComponent starup;
    AudioSource source;
    private void Awake()
    {
        instance = this;
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        touchscreen = transform.parent.Find("touchscreen").GetComponent<UnityArmatureComponent>();
        win = transform.Find("win").GetComponent<UnityArmatureComponent>();
        warning = transform.Find("warning").GetComponent<UnityArmatureComponent>();
        fail = transform.Find("fail").GetComponent<UnityArmatureComponent>();
        start = transform.Find("start").GetComponent<UnityArmatureComponent>();
        starup = transform.parent.Find("starup").GetComponent<UnityArmatureComponent>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 aimLocalPos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, Input.mousePosition, Camera.main, out aimLocalPos))
            {
                touchscreen.transform.localPosition = aimLocalPos;
            }
            touchscreen.animation.Play("touchscreen", 1);
        }
    }
    //胜利
    public void WinArmature()
    {
        if (!win.gameObject.activeInHierarchy)
        {
            AudioManager.Instance.PlaySource("win_1", source);
            win.gameObject.SetActive(true);
            win.animation.Play("win", 1);
            if (!win.HasEventListener(EventObject.COMPLETE))
            {
                win.AddEventListener(EventObject.COMPLETE, HideWin);
            };
        }
    }
    private void HideWin(string type, EventObject eventObject)
    {
        win.gameObject.SetActive(false);
    }
    //失败
    public void FailArmature()
    {
        if (!fail.gameObject.activeInHierarchy)
        {
            AudioManager.Instance.PlaySource("fail_1", source);
            fail.gameObject.SetActive(true);
            fail.animation.Play("fail", 1);
            if (!fail.HasEventListener(EventObject.COMPLETE))
            {
                fail.AddEventListener(EventObject.COMPLETE, HideFail);
            };
        }
    }
    private void HideFail(string type, EventObject eventObject)
    {
        fail.gameObject.SetActive(false);
    }
    //Boos预警
    public void WarningArmature()
    {
        if (!warning.gameObject.activeInHierarchy)
        {
            warning.gameObject.SetActive(true);
            warning.animation.Play("warning", 1);
            if (!warning.HasEventListener(EventObject.COMPLETE))
            {
                warning.AddEventListener(EventObject.COMPLETE, HideWarning);
            }
            StartCoroutine(WarnSource());
        }
    }
    IEnumerator WarnSource()
    {
        AudioManager.Instance.PlaySource("warning_1", source);
        yield return new WaitForSeconds(source.clip.length);
        AudioManager.Instance.PlaySource("warning_1", source);
        yield return new WaitForSeconds(source.clip.length);
        AudioManager.Instance.PlaySource("warning_1", source);
    }
    private void HideWarning(string type, EventObject eventObject)
    {
        warning.gameObject.SetActive(false);
    }
    //开始
    public void StartArmature()
    {
        if (!start.gameObject.activeInHierarchy)
        {
            start.gameObject.SetActive(true);
            start.animation.GotoAndPlayByTime("start", 0, 1);
            AudioManager.Instance.PlaySource("start_1", source);
            if (!start.HasEventListener(EventObject.COMPLETE))
            {
                start.AddEventListener(EventObject.COMPLETE, HideStart);
            }
        }
    }
    private void HideStart(string type, EventObject eventObject)
    {
        start.animation.Reset();
        start.gameObject.SetActive(false);
    }
    //升星
    public void StarUpArmature()
    {
        if (!starup.gameObject.activeInHierarchy)
        {
            AudioManager.Instance.PlaySource("starup_1", source);
            starup.gameObject.SetActive(true);
            starup.animation.Play("win", 1);
            if (!starup.HasEventListener(EventObject.COMPLETE))
            {
                starup.AddEventListener(EventObject.COMPLETE, HideStarUp);
            }
        }
    }
    private void HideStarUp(string type, EventObject eventObject)
    {
        starup.gameObject.SetActive(false);
    }
    //暂停所有动画并隐藏
    public void Stop()
    {
        if(start)
        {
            start.animation.Reset();
            start.gameObject.SetActive(false);
        }
    }
}

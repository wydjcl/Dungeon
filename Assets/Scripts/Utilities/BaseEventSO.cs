using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseEventSO<T> : ScriptableObject//基础事件类
{
    [TextArea]//扩展文本框，扩展描述的文本框
    public string description;//描述

    public UnityAction<T> OnEventRaised;//事件

    public string lastSender;

    public void RaisEvent(T value, object sender)//sender谁广播了
    {
        OnEventRaised?.Invoke(value);
        lastSender = sender.ToString();
    }
}
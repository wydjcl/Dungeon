using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseEventListener<T> : MonoBehaviour//基础监听类
{
    public BaseEventSO<T> eventSO;

    [Header("test")]
    public UnityEvent<T> response;

    private void OnEnable()
    {
        //当组件启用时自动注册到事件，将本地的 OnEventRaised 方法订阅到事件的 OnEventRaised 委托，并且确保事件源不为空才注册
        if (eventSO != null)
        {
            eventSO.OnEventRaised += OnEventRaised;
        }
    }

    private void OnDisable()
    {
        if (eventSO != null)
        {
            eventSO.OnEventRaised -= OnEventRaised;
        }
    }

    //当事件被触发时调用，接收事件数据 value（类型为泛型 T）调用配置的响应方法并传递数据
    private void OnEventRaised(T value)
    {
        response.Invoke(value);//调用委托
    }
}
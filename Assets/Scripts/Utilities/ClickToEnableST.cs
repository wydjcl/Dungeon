using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickToEnableST : MonoBehaviour, IPointerClickHandler
{
    [Header("使用方法\n点击后挂载的实例enable\n注意必须在UI层")]
    public GameObject gameObjectToEnable;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameObjectToEnable != null)
        {
            gameObjectToEnable.SetActive(true);
        }
        else
        {
            Debug.Log("没有可加载的对象");
        }
    }
}
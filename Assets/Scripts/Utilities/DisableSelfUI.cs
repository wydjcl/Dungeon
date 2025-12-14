using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DisableSelfUI : MonoBehaviour, IPointerClickHandler
{
    [Header("使用方法\n点击后挂载的实例disable掉\n如果有赋值对象,则改为对象\n注意必须在UI层")]
    public GameObject gameObjectToDisable;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameObjectToDisable != null)
        {
            gameObjectToDisable.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}
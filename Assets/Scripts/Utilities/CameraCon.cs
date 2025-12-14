using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraCon : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;   // 虚拟相机
    private Camera cam;
    private Vector3 dragOrigin;

    [Header("缩放参数")]
    public float zoomOutMin = 5f;   // 最小缩放

    public float zoomOutMax = 20f;  // 最大缩放

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        // ===== 手机端：单指拖动 =====
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                dragOrigin = cam.ScreenToWorldPoint(touch.position);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(touch.position);
                vcam.transform.position += difference;
            }
        }

        // ===== 手机端：双指缩放 =====
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            float newSize = vcam.m_Lens.OrthographicSize - difference * 0.01f;
            vcam.m_Lens.OrthographicSize = Mathf.Clamp(newSize, zoomOutMin, zoomOutMax);
        }

        // ===== PC 编辑器：鼠标拖动 =====
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            vcam.transform.position += difference;
        }

        // ===== PC 编辑器：鼠标滚轮缩放 =====
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            float newSize = vcam.m_Lens.OrthographicSize - scroll * 5f;
            vcam.m_Lens.OrthographicSize = Mathf.Clamp(newSize, zoomOutMin, zoomOutMax);
        }
    }
}
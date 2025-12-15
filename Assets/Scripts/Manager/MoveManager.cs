using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MoveManager : MonoBehaviour
{
    public static MoveManager Instance;
    private Camera cam;
    private Vector3 dragOrigin;

    public Vector2 playerPos;
    public bool isMoveMap;
    private bool dragging = false;          // 是否正在拖动
    private List<BoxMono> path = new List<BoxMono>();
    public BoxMono boxStart;

    [Header("缩放参数")]
    public float zoomOutMin = 5f;   // 最小缩放

    public float zoomOutMax = 20f;  // 最大缩放

    public float dragThreshold = 5f;        // 判断点击/拖动的最小位移（像素）

    [Header("需要导入")]
    public CinemachineVirtualCamera vcam;

    public PlayerMono player;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        MoveMap();
    }

    private void Init()
    {
        playerPos = Vector2.zero;
        cam = Camera.main;
    }

    public void PlayerMove(BoxMono box)
    {
        player.path.Clear();
        if (AManager.Instance.FindPath(player.boxStart, box) != null)
        {
            player.path = AManager.Instance.FindPath(player.boxStart, box);//思路是点击后将路径置入path,然后每次playerend阶段走一步
        }

        // StartCoroutine(FollowNull(pos)); // 传参数
    }

    private IEnumerator FollowNull(Vector2 pos)
    {
        yield return new WaitForSeconds(1f); // 延迟 1 秒
        GameManager.Instance.player.transform.position = pos;
        yield return new WaitForSeconds(1f);
        vcam.Follow = null;//TODO连续移动时候follow刚赋值就被置空了
    }

    public void MoveMap()
    {
        isMoveMap = false; // 每帧默认 false，只有拖动/缩放时才置 true

        // ===== 手机端：单指拖动 =====
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                dragOrigin = touch.position;
                dragging = true;
            }
            else if (touch.phase == TouchPhase.Moved && dragging)
            {
                if (Vector3.Distance(touch.position, dragOrigin) > dragThreshold)
                {
                    Vector3 difference = cam.ScreenToWorldPoint(dragOrigin) - cam.ScreenToWorldPoint(touch.position);
                    vcam.transform.position += difference;
                    dragOrigin = touch.position;
                    isMoveMap = true;
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                dragging = false;
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

            isMoveMap = true;
        }

        // ===== PC 编辑器：鼠标拖动 =====
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            dragging = true;
        }

        if (Input.GetMouseButton(0) && dragging)
        {
            if (Vector3.Distance(Input.mousePosition, dragOrigin) > dragThreshold)
            {
                Vector3 difference = cam.ScreenToWorldPoint(dragOrigin) - cam.ScreenToWorldPoint(Input.mousePosition);
                vcam.transform.position += difference;
                dragOrigin = Input.mousePosition;
                isMoveMap = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }

        // ===== PC 编辑器：鼠标滚轮缩放 =====
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            float newSize = vcam.m_Lens.OrthographicSize - scroll * 5f;
            vcam.m_Lens.OrthographicSize = Mathf.Clamp(newSize, zoomOutMin, zoomOutMax);
            isMoveMap = true;
        }
    }
}
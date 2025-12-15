using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpTextManager : MonoBehaviour
{
    public static UpTextManager Instance;
    public GameObject UpText;

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

    public void CreateUpText(Vector3 pos, string text)
    {
        var t = Instantiate(UpText, pos, Quaternion.identity);
        t.GetComponent<TextMeshPro>().text = text;
        t.transform.DOMove(new Vector3(pos.x, pos.y + 0.45f, pos.z), 0.35f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            t.transform.DOKill();
            Destroy(t);
        });
    }
}
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    public bool battleMode;//战争迷雾外有敌人触发
    public GameObject playerObject;
    public PlayerMono player;
    public List<EnemyBase> enemyList = new List<EnemyBase>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Init();
    }

    public void Init()
    {
        playerObject.SetActive(true);
        GameManager.Instance.AManager.SetActive(true);
    }
}
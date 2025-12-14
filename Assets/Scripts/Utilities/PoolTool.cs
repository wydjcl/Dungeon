using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;

[DefaultExecutionOrder(-100)]//越小越先其他代码执行
public class PoolTool : MonoBehaviour
{
    public GameObject objPrefab;
    private ObjectPool<GameObject> pool;

    private void Awake()
    {
        pool = new ObjectPool<GameObject>(
        //初始化对象池子
        createFunc: () => Instantiate(objPrefab, transform),//创建方法,我们调用时候,生成预制体
        actionOnDestroy: (obj) => Destroy(obj),//obj是我们创建的上述对象
        actionOnGet: (obj) => obj.SetActive(true),
        actionOnRelease: (obj) => obj.SetActive(false),
        collectionCheck: false,
        defaultCapacity: 10,//默认大小
        maxSize: 10//池子中最多能保存多少个“未使用”的对象。

            );
        PreFillPool(10);//预先填充,减少加载
    }

    //预先生成对象到池子里
    private void PreFillPool(int count)
    {
        var preFillArray = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            preFillArray[i] = pool.Get();//生成物体,保存在数组
        }
        foreach (var item in preFillArray)
        {
            pool.Release(item);//进入数组后放回池子里
        }
    }

    public GameObject GetObjectFromPool()
    {
        return pool.Get();
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        pool.Release(obj);
    }
}
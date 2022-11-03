using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ӳ���ϵ��
        �ⲿ����ʱGet��Setʱ������Ĳ���name��Resources��Դ�µ�ȫ·����
        ���л�����ʱ����ص���һ��ClearAll()����ֹ��һ������û�л���ض���
        ������еĲ�ͬ�����Կ���ϸ�·��࣬�˴���ʱ����V1.0��
 */


public class PoolManager : Singleton<PoolManager>
{
    //�ֵ�洢��Ӧ���͵���Ϸ����(�����)
    public Dictionary<string, Queue<GameObject>> cachePool = new Dictionary<string, Queue<GameObject>>();

    //����ض���
    private GameObject basePool;
    private GameObject activePool;
    private GameObject inactivePool;

    //�ӻ����ȡ
    public GameObject GetElement(string name,Transform initTrans = null)
    {
        //���ò㼶
        if (basePool == null)
        {
            basePool = new GameObject("CachePool");
            activePool = new GameObject("ActivePool");
            activePool.transform.parent = basePool.transform;
            inactivePool = new GameObject("InactivePool");
            inactivePool.transform.parent = basePool.transform;
        }

        GameObject res = null;
        
        if (cachePool.ContainsKey(name) && cachePool[name].Count > 0)
        {
            //������Ҫ�Ļ���Ԫ��ʱ
            res = cachePool[name].Dequeue();
        }
        else
        {
            //��û����Ҫ��Ԫ��ʱ
            res = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(name));
            res.name = name;
            
        }
        //�ó�������ó������弤��
        res.SetActive(true);
        res.transform.parent = activePool.transform;
        if (initTrans != null)
            res.transform.position = initTrans.position;
        return res;
    }

    //���뻺��
    public void SetElement(string name,GameObject element)
    {
        //���ø�����
        element.transform.parent = inactivePool.transform;

        //���뻺����ó�������ʧ��
        element.SetActive(false);
        if (cachePool.ContainsKey(name))
        {
            //�����ڶ�Ӧ�����ʱ
            cachePool[name].Enqueue(element);
        }
        else
        {
            //�������ڶ�Ӧ�����ʱ
            cachePool.Add(name, new Queue<GameObject>());
        }
    }

    //��ջ���أ���Ҫ���ڳ����л�
    public void ClearAll()
    {
        cachePool.Clear();
        basePool = null;
    }
}

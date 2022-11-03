using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
    ͳһ����֡���£�ʹ���¼���Э��
 */

public class MonoController : MonoBehaviour
{
    //ͳһ����֡���µ��¼�
    private event UnityAction updateEvent;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (updateEvent != null)
            updateEvent.Invoke();
    }

    //���ⲿ�ṩ�����֡���µĺ���
    public void AddUpdateListener(UnityAction fun)
    {
        updateEvent += fun;
    }

    //���ⲿ�ṩ���Ƴ�֡���µĺ���
    public void RemoveUpdateListener(UnityAction fun)
    {
        updateEvent -= fun;
    }
}

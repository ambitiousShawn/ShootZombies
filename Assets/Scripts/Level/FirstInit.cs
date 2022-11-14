using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
    ��һ�ؿ��ֹؿ��ĳ�ʼ��
 */
public class FirstInit : MonoBehaviour
{
    void Awake()
    {
        //���ֳ�ʼ��������Ϣ
        ResourcesManager.Instance.LoadAsync<GameObject>("Role/Gunner_ǹ��", (obj) =>
         {
             //�޸��������
             obj.name = "Player";
             obj.transform.position = transform.position;
             obj.transform.rotation = Quaternion.identity;
         });

        //��ʼ��Ѫ��UI������
        UIManager.Instance.ShowPanel<GamePanel>("GamePanel");


        //��ʼ��BGM
        AudioManager.Instance.PlayBGM("�ϳ�");
    }
}

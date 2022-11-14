using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
    第一关开局关卡的初始化
 */
public class FirstInit : MonoBehaviour
{
    void Awake()
    {
        //开局初始化人物信息
        ResourcesManager.Instance.LoadAsync<GameObject>("Role/Gunner_枪手", (obj) =>
         {
             //修改玩家姓名
             obj.name = "Player";
             obj.transform.position = transform.position;
             obj.transform.rotation = Quaternion.identity;
         });

        //初始化血条UI并更新
        UIManager.Instance.ShowPanel<GamePanel>("GamePanel");


        //初始化BGM
        AudioManager.Instance.PlayBGM("废城");
    }
}

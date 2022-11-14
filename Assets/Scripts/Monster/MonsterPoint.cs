using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoint : MonoBehaviour
{
    //生成的怪物id(用于属性赋值与模型创建)
    public List<int> monsterIDs;

    //存储生成的僵尸
    private GameObject Zombies;
    

    #region 刷怪点的设置
    [Header("总共生成多少只丧尸")]
    public int monsterNumOneWave = 15;
    [Header("生成单只丧尸的时间间隔")]
    public float createOneInterval = 3f;
    [Header("生成第一只丧尸的时间")]
    public float firstDelayTime = 0;
    #endregion

    #region 刷怪点的数据记录
    //当前丧尸潮的丧尸类型
    private int currId;
    //当前创建了多少只
    private int currMonsterNum;
    #endregion

    void Start()
    {
        Invoke("CreateWave", firstDelayTime);
        Zombies = GameObject.Find("Zombies");
    }

    //开始创建一波尸潮
    private void CreateWave()
    {
        //当前波的丧尸类型
        currId = monsterIDs[Random.Range(0, monsterIDs.Count)];
        //当前波有多少只
        currMonsterNum = 0;
        //创建丧尸
        CreateZombie();
    }

    private void CreateZombie()
    {
        //得到创建数据
        MonsterInfo info = DataManager.Instance.monsterInfoList[currId - 1];

        //创建怪物预设体
        GameObject zombieObj = ResourcesManager.Instance.Load<GameObject>(info.res);
        //调整信息
        zombieObj.transform.position = transform.position;
        zombieObj.transform.rotation = Random.rotation ;
        zombieObj.transform.parent = Zombies.transform;

        //怪物脚本
        ZombiesInGame zombie = zombieObj.AddComponent<ZombiesInGame>();
        zombie.InitZombie(info,transform);

        ++currMonsterNum;

        if (currMonsterNum >= monsterNumOneWave)
        {
            //None
        }
        else
        {
            Invoke("CreateZombie", createOneInterval);
        }
    }
}

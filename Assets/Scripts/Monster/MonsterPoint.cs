using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoint : MonoBehaviour
{
    //生成的怪物id(用于属性赋值与模型创建)
    public List<int> monsterIDs;
    //当前丧尸潮的丧尸类型
    private int currId;

    //生成丧尸潮的周期
    public float monstersInterval;

    //一波丧尸潮有多少丧尸
    public int monsterNumOneWave;
    //当前创建了多少只
    private int currMonsterNum;

    

    //单只丧尸的创建时间
    public float createOneInterval;

    //开局玩家的准备时间
    public float firstDelayTime;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("CreateWave", firstDelayTime);
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
        zombieObj.transform.rotation = Quaternion.identity;

        //怪物脚本
        ZombiesInGame zombie = zombieObj.AddComponent<ZombiesInGame>();
        zombie.InitZombie(info);

        ++currMonsterNum;

        if (currMonsterNum >= monsterNumOneWave)
        {
            //该波丧尸数量创建完成,准备创建下波丧尸
            if (1 == 0)
            {

            }

        }
        else
        {
            Invoke("CreateZombie", createOneInterval);
        }
    }
}

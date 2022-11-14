using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoint : MonoBehaviour
{
    //���ɵĹ���id(�������Ը�ֵ��ģ�ʹ���)
    public List<int> monsterIDs;

    //�洢���ɵĽ�ʬ
    private GameObject Zombies;
    

    #region ˢ�ֵ������
    [Header("�ܹ����ɶ���ֻɥʬ")]
    public int monsterNumOneWave = 15;
    [Header("���ɵ�ֻɥʬ��ʱ����")]
    public float createOneInterval = 3f;
    [Header("���ɵ�һֻɥʬ��ʱ��")]
    public float firstDelayTime = 0;
    #endregion

    #region ˢ�ֵ�����ݼ�¼
    //��ǰɥʬ����ɥʬ����
    private int currId;
    //��ǰ�����˶���ֻ
    private int currMonsterNum;
    #endregion

    void Start()
    {
        Invoke("CreateWave", firstDelayTime);
        Zombies = GameObject.Find("Zombies");
    }

    //��ʼ����һ��ʬ��
    private void CreateWave()
    {
        //��ǰ����ɥʬ����
        currId = monsterIDs[Random.Range(0, monsterIDs.Count)];
        //��ǰ���ж���ֻ
        currMonsterNum = 0;
        //����ɥʬ
        CreateZombie();
    }

    private void CreateZombie()
    {
        //�õ���������
        MonsterInfo info = DataManager.Instance.monsterInfoList[currId - 1];

        //��������Ԥ����
        GameObject zombieObj = ResourcesManager.Instance.Load<GameObject>(info.res);
        //������Ϣ
        zombieObj.transform.position = transform.position;
        zombieObj.transform.rotation = Random.rotation ;
        zombieObj.transform.parent = Zombies.transform;

        //����ű�
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

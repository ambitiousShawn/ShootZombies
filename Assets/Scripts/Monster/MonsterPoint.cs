using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoint : MonoBehaviour
{
    //���ɵĹ���id(�������Ը�ֵ��ģ�ʹ���)
    public List<int> monsterIDs;
    //��ǰɥʬ����ɥʬ����
    private int currId;

    //����ɥʬ��������
    public float monstersInterval;

    //һ��ɥʬ���ж���ɥʬ
    public int monsterNumOneWave;
    //��ǰ�����˶���ֻ
    private int currMonsterNum;

    

    //��ֻɥʬ�Ĵ���ʱ��
    public float createOneInterval;

    //������ҵ�׼��ʱ��
    public float firstDelayTime;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("CreateWave", firstDelayTime);
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
        zombieObj.transform.rotation = Quaternion.identity;

        //����ű�
        ZombiesInGame zombie = zombieObj.AddComponent<ZombiesInGame>();
        zombie.InitZombie(info);

        ++currMonsterNum;

        if (currMonsterNum >= monsterNumOneWave)
        {
            //�ò�ɥʬ�����������,׼�������²�ɥʬ
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

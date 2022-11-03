using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ���ݹ�������
        ����������Ϸȫ�ֵ����ݡ�
    
 */
public class DataManager 
{
    private static DataManager instance = new DataManager();

    public static DataManager Instance => instance;
    

    //������Ч����
    public MusicData musicData;

    //��ɫ����
    public List<RoleInfo> roleInfoList;

    private DataManager()
    {
        //��ʼ����Ч����
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");
        //��ʼ����ɫ����
        roleInfoList = JsonMgr.Instance.LoadData<List<RoleInfo>>("RoleInfo");
    }

    //��������������Ϣ
    public void SaveMusicData()
    {
        JsonMgr.Instance.SaveData(musicData, "MusicData");
    }
    

    //�����ɫ������Ϣ
    public void SaveRoleData()
    {
        JsonMgr.Instance.SaveData(roleInfoList, "RoleInfoList");
    }
}

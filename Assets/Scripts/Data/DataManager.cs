using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ���ݹ�������
        ����������Ϸȫ�ֵ����ݡ�
    
 */
public class DataManager : Singleton<DataManager>
{
    //������Ч����
    public MusicData musicData;

    public DataManager()
    {
        //��ʼ����Ч����
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");
    }

    public void SaveMusicData()
    {
        JsonMgr.Instance.SaveData(musicData, "MusicData");
    }
    
}

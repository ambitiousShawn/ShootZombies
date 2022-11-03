using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    数据管理器：
        用来管理游戏全局的数据。
    
 */
public class DataManager : Singleton<DataManager>
{
    //音乐音效数据
    public MusicData musicData;

    public DataManager()
    {
        //初始化音效数据
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");
    }

    public void SaveMusicData()
    {
        JsonMgr.Instance.SaveData(musicData, "MusicData");
    }
    
}

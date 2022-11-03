using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    数据管理器：
        用来管理游戏全局的数据。
    
 */
public class DataManager 
{
    private static DataManager instance = new DataManager();

    public static DataManager Instance => instance;
    

    //音乐音效数据
    public MusicData musicData;

    //角色数据
    public List<RoleInfo> roleInfoList;

    private DataManager()
    {
        //初始化音效数据
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");
        //初始化角色数据
        roleInfoList = JsonMgr.Instance.LoadData<List<RoleInfo>>("RoleInfo");
    }

    //保存音乐数据信息
    public void SaveMusicData()
    {
        JsonMgr.Instance.SaveData(musicData, "MusicData");
    }
    

    //保存角色数据信息
    public void SaveRoleData()
    {
        JsonMgr.Instance.SaveData(roleInfoList, "RoleInfoList");
    }
}

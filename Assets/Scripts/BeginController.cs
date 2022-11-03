using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginController : MonoBehaviour
{
    void Start()
    {
        //初始化开始界面
        UIManager.Instance.ShowPanel<BeginPanel>("BeginPanel");
        //读取音乐信息并播放
        MusicData musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");
        AudioManager.Instance.PlayBGM("BKMusic");
    }
}

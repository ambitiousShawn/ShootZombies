using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginController : MonoBehaviour
{
    void Start()
    {
        //��ʼ����ʼ����
        UIManager.Instance.ShowPanel<BeginPanel>("BeginPanel");
        //��ȡ������Ϣ������
        MusicData musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");
        AudioManager.Instance.PlayBGM("BKMusic");
    }
}

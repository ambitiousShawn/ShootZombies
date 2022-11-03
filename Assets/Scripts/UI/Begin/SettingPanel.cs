using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    //组件
    private Toggle toggle_Music;
    private Toggle toggle_Sound;
    private Button btn_Quit;
    private Slider slider_Music;
    private Slider slider_Sound;

    protected override void Init()
    {
        toggle_Music = GetControl<Toggle>("Toggle_Music");
        toggle_Sound = GetControl<Toggle>("Toggle_Sound");
        btn_Quit = GetControl<Button>("Btn_Quit");
        slider_Music = GetControl<Slider>("Slider_Music");
        slider_Sound = GetControl<Slider>("Slider_Sound");

        //初始化面板显示数值
        MusicData musicData = DataManager.Instance.musicData;
        toggle_Music.isOn = musicData.isOpenBGM;
        toggle_Sound.isOn = musicData.isOpenSound;
        slider_Music.value = musicData.BGMVolume;
        slider_Sound.value = musicData.soundVolume;

        //退出设置页面
        btn_Quit.onClick.AddListener(() =>
        {
            //当面板关闭时，保存数据到磁盘
            DataManager.Instance.SaveMusicData();
            UIManager.Instance.HidePanel("SettingPanel", false);
        });

        toggle_Music.onValueChanged.AddListener((value) =>
        {
            //开关BGM
            if (!value)
            {
                AudioManager.Instance.PauseBGM();
                //设置全局变量值
                DataManager.Instance.musicData.isOpenBGM = false;
            }

            else
            {
                //FIXED:BGM关闭后，调整音量大小，再开启BGM时，会出现音量并没有被修改的BUG
                AudioManager.Instance.PlayBGM("BKMusic",true,DataManager.Instance.musicData.BGMVolume);
                DataManager.Instance.musicData.isOpenBGM = true;
            }
        });

        toggle_Sound.onValueChanged.AddListener((value) =>
        {
            //TODO:开关音效

        });

        slider_Music.onValueChanged.AddListener((value) =>
        {
            //改变BGM音量
            AudioManager.Instance.SwitchVolume(value);
            DataManager.Instance.musicData.BGMVolume = value;
        });

        slider_Sound.onValueChanged.AddListener((value) =>
        {
            //TODO:开关音效的逻辑
        });
    }
}

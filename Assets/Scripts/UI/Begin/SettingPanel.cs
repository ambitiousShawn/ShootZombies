using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    //���
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

        //��ʼ�������ʾ��ֵ
        MusicData musicData = DataManager.Instance.musicData;
        toggle_Music.isOn = musicData.isOpenBGM;
        toggle_Sound.isOn = musicData.isOpenSound;
        slider_Music.value = musicData.BGMVolume;
        slider_Sound.value = musicData.soundVolume;

        //�˳�����ҳ��
        btn_Quit.onClick.AddListener(() =>
        {
            //�����ر�ʱ���������ݵ�����
            DataManager.Instance.SaveMusicData();
            UIManager.Instance.HidePanel("SettingPanel", false);
        });

        toggle_Music.onValueChanged.AddListener((value) =>
        {
            //����BGM
            if (!value)
            {
                AudioManager.Instance.PauseBGM();
                //����ȫ�ֱ���ֵ
                DataManager.Instance.musicData.isOpenBGM = false;
            }

            else
            {
                //FIXED:BGM�رպ󣬵���������С���ٿ���BGMʱ�������������û�б��޸ĵ�BUG
                AudioManager.Instance.PlayBGM("BKMusic",true,DataManager.Instance.musicData.BGMVolume);
                DataManager.Instance.musicData.isOpenBGM = true;
            }
        });

        toggle_Sound.onValueChanged.AddListener((value) =>
        {
            //TODO:������Ч

        });

        slider_Music.onValueChanged.AddListener((value) =>
        {
            //�ı�BGM����
            AudioManager.Instance.SwitchVolume(value);
            DataManager.Instance.musicData.BGMVolume = value;
        });

        slider_Sound.onValueChanged.AddListener((value) =>
        {
            //TODO:������Ч���߼�
        });
    }
}

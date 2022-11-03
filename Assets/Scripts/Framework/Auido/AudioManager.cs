using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioManager : Singleton<AudioManager>
{
    //BGM���
    private AudioSource BGM_Audio = GameObject.Find("Audio").GetComponent<AudioSource>();

    //��Ч���
    private GameObject Sound_Obj = GameObject.Find("Sound");
    private List<AudioSource> soundList = new List<AudioSource>();

    //������Ч����������Ƴ�
    public AudioManager()
    {
        //����֡���²��ԣ������Ч���������ɾ�����
        MonoManager.Instance.AddUpdateListener(() =>
        {
            for (int i = soundList.Count - 1;i >= 0; --i)
            {
                if (!soundList[i].isPlaying)
                {
                    GameObject.Destroy(soundList[i]);
                    soundList.Remove(soundList[i]);
                }   
            }
        });
    }

    #region BGM���
    //��ʼ����BGM
    /*
        Param1:bgm��ResourcesĿ¼��"Audio/BGM/"���ļ���
        Param2:bgm�Ƿ�ѭ������
        Param3:bgm����������С
        Param4:�Ƿ񿪾ֲ���
    */
    public void PlayBGM(string bgmName, bool isLoop = true, float volume = 0.8f)
    {
        if (BGM_Audio == null)
            BGM_Audio = new GameObject("Audio").AddComponent<AudioSource>();
        //�첽����
        ResourcesManager.Instance.LoadAsync<AudioClip>("Audio/BGM/" + bgmName, (clip) =>
       {
           BGM_Audio.clip = clip;
           BGM_Audio.loop = isLoop;
           BGM_Audio.volume = volume;
           BGM_Audio.Play();
       });
    }

    //ֹͣ���ֲ���
    public void StopBGM()
    {
        if (BGM_Audio == null) return;
        BGM_Audio.Stop();
    }

    //��ͣ��������
    public void PauseBGM()
    {
        if (BGM_Audio == null) return;
        BGM_Audio.Pause();
    }

    //�ı䱳����������
    public void SwitchVolume(float volume)
    {
        if (BGM_Audio == null) return;
        //��������С��������(0~1)
        if (volume > 1 || volume < 0) return; 
        BGM_Audio.volume = volume;
    }
    #endregion

    #region ��Ч���
    //������Ч
    public void PlaySound(string soundName,UnityAction<AudioSource> callback = null ,bool isLoop = false, float volume = 0.8f)
    {
        if (Sound_Obj == null)
            Sound_Obj = new GameObject("Sound");
        //�첽������Դ
        ResourcesManager.Instance.LoadAsync<AudioClip>("Audio/Sound/" + soundName, (clip) =>
         {
             AudioSource sound_Audio = Sound_Obj.AddComponent<AudioSource>();
             sound_Audio.clip = clip;
             sound_Audio.loop = isLoop;
             sound_Audio.volume = volume;
             sound_Audio.Play();
             //�����б�
             soundList.Add(sound_Audio);
             if (callback != null)
             {
                 callback(sound_Audio);
             }
         });
    }

    //ֹͣ��Ч
    public void StopSound(AudioSource source)
    {
        if (soundList.Contains(source))
        {
            source.Stop();
            soundList.Remove(source);
            GameObject.Destroy(source);
        }
    }
    #endregion
}

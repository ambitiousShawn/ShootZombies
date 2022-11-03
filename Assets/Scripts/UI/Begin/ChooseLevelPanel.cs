using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseLevelPanel : BasePanel
{
    //UI���
    private Button Btn_back;
    private Button Btn_start;
    private Button Btn_last;
    private Button Btn_next;
    private Image Img_map;
    private Text Txt_info;

    //��ǰ��Ļ�ϳ���ͼƬ��Ϣ
    private SceneInfo currSceneInfo;
    //��ǰͼƬ������
    private int curIndex;

    protected override void Init()
    {
        Btn_back = GetControl<Button>("Btn_back");
        Btn_start = GetControl<Button>("Btn_start");
        Btn_last = GetControl<Button>("Btn_last");
        Btn_next = GetControl<Button>("Btn_next");
        Img_map = GetControl<Image>("Img_map");
        Txt_info = GetControl<Text>("Txt_info");

        //����ѡ��Ӣ�����
        Btn_back.onClick.AddListener(() =>
       {
           UIManager.Instance.ShowPanel<ChooseHeroPanel>("ChooseHeroPanel");
           UIManager.Instance.HidePanel("ChooseLevelPanel",false);
       });

        //��ʼ��Ϸ�߼�
        Btn_start.onClick.AddListener(() =>
        {
            //TODO:��ʼ��Ϸ
        });

        //��һ�ŵ�ͼ
        Btn_last.onClick.AddListener(() =>
        {
            curIndex--;
            if (curIndex < 0)
                curIndex = DataManager.Instance.sceneInfoList.Count - 1;

            SwitchSceneImage();
        });

        Btn_next.onClick.AddListener(() =>
        {
            curIndex++;
            if (curIndex > DataManager.Instance.sceneInfoList.Count - 1)
                curIndex = 0;

            SwitchSceneImage();
        });

        SwitchSceneImage();
    }

    //�л�����
    private void SwitchSceneImage()
    {
        currSceneInfo = DataManager.Instance.sceneInfoList[curIndex];
        //�õ���ǰ��Ϣ��ֵ�����ֺ�ͼƬ
        Img_map.sprite = Resources.Load<Sprite>(currSceneInfo.imgRes);
        Txt_info.text = "��ͼ���֣�" + currSceneInfo.name + "\n"
                      + "��ͼ��飺\n" + currSceneInfo.tips;
    }

}

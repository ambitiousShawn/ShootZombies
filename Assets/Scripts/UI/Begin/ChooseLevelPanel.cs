using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseLevelPanel : BasePanel
{
    //UI组件
    private Button Btn_back;
    private Button Btn_start;
    private Button Btn_last;
    private Button Btn_next;
    private Image Img_map;
    private Text Txt_info;

    //当前屏幕上场景图片信息
    private SceneInfo currSceneInfo;
    //当前图片的索引
    private int curIndex;

    protected override void Init()
    {
        Btn_back = GetControl<Button>("Btn_back");
        Btn_start = GetControl<Button>("Btn_start");
        Btn_last = GetControl<Button>("Btn_last");
        Btn_next = GetControl<Button>("Btn_next");
        Img_map = GetControl<Image>("Img_map");
        Txt_info = GetControl<Text>("Txt_info");

        //返回选择英雄面板
        Btn_back.onClick.AddListener(() =>
       {
           UIManager.Instance.ShowPanel<ChooseHeroPanel>("ChooseHeroPanel");
           UIManager.Instance.HidePanel("ChooseLevelPanel",false);
       });

        //开始游戏逻辑
        Btn_start.onClick.AddListener(() =>
        {
            //TODO:开始游戏
        });

        //上一张地图
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

    //切换场景
    private void SwitchSceneImage()
    {
        currSceneInfo = DataManager.Instance.sceneInfoList[curIndex];
        //拿到当前信息赋值给文字和图片
        Img_map.sprite = Resources.Load<Sprite>(currSceneInfo.imgRes);
        Txt_info.text = "地图名字：" + currSceneInfo.name + "\n"
                      + "地图简介：\n" + currSceneInfo.tips;
    }

}

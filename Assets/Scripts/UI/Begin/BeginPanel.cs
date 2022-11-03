using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{
    private Button btn_Start;
    private Button btn_Setting;
    private Button btn_About;
    private Button btn_Exit;

    protected override void Init()
    {
        btn_Start = GetControl<Button>("Btn_Start");
        btn_Setting = GetControl<Button>("Btn_Set");
        btn_About = GetControl<Button>("Btn_About");
        btn_Exit = GetControl<Button>("Btn_Exit");

        btn_Start.onClick.AddListener(() =>
        {
            //TODO:显示选角面板
            print("开始游戏！");
            Camera.main.GetComponent<CameraFollow>().switchSelectAnim = true;
            //隐藏开始面板
            UIManager.Instance.HidePanel("BeginPanel",false);
        });

        btn_Setting.onClick.AddListener(() =>
        {
            //TODO:显示设置界面
            print("设置！");
            UIManager.Instance.ShowPanel<SettingPanel>("SettingPanel");
        });

        btn_About.onClick.AddListener(() =>
        {
            //TODO:显示关于面板
            print("关于！");
        });

        btn_Exit.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}

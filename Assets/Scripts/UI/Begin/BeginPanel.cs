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
            //TODO:��ʾѡ�����
            print("��ʼ��Ϸ��");
            Camera.main.GetComponent<CameraFollow>().switchSelectAnim = true;
            //���ؿ�ʼ���
            UIManager.Instance.HidePanel("BeginPanel",false);
        });

        btn_Setting.onClick.AddListener(() =>
        {
            //TODO:��ʾ���ý���
            print("���ã�");
            UIManager.Instance.ShowPanel<SettingPanel>("SettingPanel");
        });

        btn_About.onClick.AddListener(() =>
        {
            //TODO:��ʾ�������
            print("���ڣ�");
        });

        btn_Exit.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}

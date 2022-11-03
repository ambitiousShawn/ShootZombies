using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseHeroPanel : BasePanel
{
    //���
    private Button Btn_back;
    private Button Btn_start;
    private Button Btn_last;
    private Button Btn_next;
    private Text Txt_tips;

    //ģ�͵ĳ���λ��
    private Transform heroTrans;
    //��ǰ����ϳ��ֵ�ģ��
    private GameObject heroObj;
    //��ǰģ�͵�����
    private RoleInfo currRoleInfo;
    //��ǰģ�͵�����
    private int heroIndex;


    protected override void Init()
    {
        heroTrans = GameObject.Find("Points/HeroPoint").transform;

        Btn_back = GetControl<Button>("Btn_back");
        Btn_start = GetControl<Button>("Btn_start");
        Btn_last = GetControl<Button>("Btn_last");
        Btn_next = GetControl<Button>("Btn_next");
        Txt_tips = GetControl<Text>("Txt_tips");

        //������һ���
        Btn_back.onClick.AddListener(() =>
        {
            
            UIManager.Instance.ShowPanel<BeginPanel>("BeginPanel");
            UIManager.Instance.HidePanel("ChooseHeroPanel", false);
            //������ָ�Ѳ��ģʽ
            Camera.main.GetComponent<CameraFollow>().MoveToBegin();
            //�Ƴ���ǰģ��
            Destroy(heroObj.gameObject);
        });

        //��ʼ��Ϸ
        Btn_start.onClick.AddListener(() =>
        {
            //�л���ѡ���ͼ���
            UIManager.Instance.ShowPanel<ChooseLevelPanel>("ChooseLevelPanel");
            UIManager.Instance.HidePanel("ChooseHeroPanel",false);
            //�Ƴ���ǰģ��
            Destroy(heroObj.gameObject);
            
        });

        //�л���ɫ
        Btn_last.onClick.AddListener(() =>
        {
            //��һ����ɫ
            //�������ģ�ͣ���ɾ��ģ��
            if (heroObj != null)
                Destroy(heroObj.gameObject);

            //ģ������+1
            heroIndex--;
            if (heroIndex < 0)
                heroIndex = DataManager.Instance.roleInfoList.Count - 1;

            //ʵ����ģ��
            SwitchRoleModel();
        });

        //��һ����ɫ
        Btn_next.onClick.AddListener(() =>
        {
            //�������ģ�ͣ���ɾ��ģ��
            if (heroObj != null)
                Destroy(heroObj.gameObject);

            //ģ������+1
            heroIndex++;
            if (heroIndex > DataManager.Instance.roleInfoList.Count - 1)
                heroIndex = 0;

            SwitchRoleModel();
        });

        SwitchRoleModel();
    }

    //ʵ�������ɵ�ǰģ��
    private void SwitchRoleModel()
    {
        currRoleInfo = DataManager.Instance.roleInfoList[heroIndex];
        heroObj = Instantiate(Resources.Load<GameObject>(currRoleInfo.res), heroTrans.position, heroTrans.rotation);
        //������ʾ��Ϣ
        Txt_tips.text = currRoleInfo.tips;
    }
}

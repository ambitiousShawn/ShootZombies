using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseHeroPanel : BasePanel
{
    //组件
    private Button Btn_back;
    private Button Btn_start;
    private Button Btn_last;
    private Button Btn_next;
    private Text Txt_tips;

    //模型的出现位置
    private Transform heroTrans;
    //当前面板上出现的模型
    private GameObject heroObj;
    //当前模型的数据
    private RoleInfo currRoleInfo;
    //当前模型的索引
    private int heroIndex;


    protected override void Init()
    {
        heroTrans = GameObject.Find("Points/HeroPoint").transform;

        Btn_back = GetControl<Button>("Btn_back");
        Btn_start = GetControl<Button>("Btn_start");
        Btn_last = GetControl<Button>("Btn_last");
        Btn_next = GetControl<Button>("Btn_next");
        Txt_tips = GetControl<Text>("Txt_tips");

        //返回上一面板
        Btn_back.onClick.AddListener(() =>
        {
            
            UIManager.Instance.ShowPanel<BeginPanel>("BeginPanel");
            UIManager.Instance.HidePanel("ChooseHeroPanel", false);
            //摄像机恢复巡游模式
            Camera.main.GetComponent<CameraFollow>().MoveToBegin();
            //移除当前模型
            Destroy(heroObj.gameObject);
        });

        //开始游戏
        Btn_start.onClick.AddListener(() =>
        {
            //切换到选择地图面板
            UIManager.Instance.ShowPanel<ChooseLevelPanel>("ChooseLevelPanel");
            UIManager.Instance.HidePanel("ChooseHeroPanel",false);
            //移除当前模型
            Destroy(heroObj.gameObject);
            
        });

        //切换角色
        Btn_last.onClick.AddListener(() =>
        {
            //上一个角色
            //如果存在模型，先删除模型
            if (heroObj != null)
                Destroy(heroObj.gameObject);

            //模型索引+1
            heroIndex--;
            if (heroIndex < 0)
                heroIndex = DataManager.Instance.roleInfoList.Count - 1;

            //实例化模型
            SwitchRoleModel();
        });

        //下一个角色
        Btn_next.onClick.AddListener(() =>
        {
            //如果存在模型，先删除模型
            if (heroObj != null)
                Destroy(heroObj.gameObject);

            //模型索引+1
            heroIndex++;
            if (heroIndex > DataManager.Instance.roleInfoList.Count - 1)
                heroIndex = 0;

            SwitchRoleModel();
        });

        SwitchRoleModel();
    }

    //实例化生成当前模型
    private void SwitchRoleModel()
    {
        currRoleInfo = DataManager.Instance.roleInfoList[heroIndex];
        heroObj = Instantiate(Resources.Load<GameObject>(currRoleInfo.res), heroTrans.position, heroTrans.rotation);
        //更新提示信息
        Txt_tips.text = currRoleInfo.tips;
    }
}

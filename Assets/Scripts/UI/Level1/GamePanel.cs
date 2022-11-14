using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    private Button Btn_back;
    private Scrollbar ScrollBar_hp;
    private Text Txt_hpValue;
    private Text Txt_BulletNum;
    private Text Txt_Item1Num;


    protected override void Init()
    {
        Btn_back = GetControl<Button>("Btn_back");
        ScrollBar_hp = GetControl<Scrollbar>("ScrollBar_hp");
        Txt_hpValue = GetControl<Text>("Txt_hpValue");
        Txt_BulletNum = GetControl<Text>("Txt_BulletNum");
        Txt_Item1Num = GetControl<Text>("Txt_Item1Num");

        //返回主界面
        Btn_back.onClick.AddListener(() =>
        {
            //TODO:切换至主界面场景
        });
    }

    //更新血条方法
    public void UpdateHpBar(int currHp,int maxHp)
    {
        float currHp1 = (float)currHp;
        float maxHp1 = (float)maxHp; 
        //更新血条长度
        ScrollBar_hp.size = currHp1 / maxHp1;
        //更新文字内容
        Txt_hpValue.text = currHp1 + "/" + maxHp1;
    }

    //更新子弹的方法
    public void UpdateBulletNum(int currNum,int maxNum)
    {
        Txt_BulletNum.text = currNum + " / " + maxNum;
    }

    //更新收集物数量的方法
    public void UpdateItemNum(int num)
    {
        Txt_Item1Num.text = num.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    private Button Btn_back;
    private Scrollbar ScrollBar_hp;
    private Text Txt_hpValue;


    protected override void Init()
    {
        Btn_back = GetControl<Button>("Btn_back");
        ScrollBar_hp = GetControl<Scrollbar>("ScrollBar_hp");
        Txt_hpValue = GetControl<Text>("Txt_hpValue");

        //返回主界面
        Btn_back.onClick.AddListener(() =>
        {
            //TODO:切换至主界面场景
        });
    }

    //更新血条方法
    public void UpdateHpBar(int currHp,int maxHp)
    {
        //更新血条长度
        ScrollBar_hp.size = currHp / maxHp;
        //更新文字内容
        Txt_hpValue.text = currHp + "/" + maxHp;
    }
}

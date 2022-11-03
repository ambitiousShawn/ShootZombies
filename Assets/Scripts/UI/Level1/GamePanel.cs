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

        //����������
        Btn_back.onClick.AddListener(() =>
        {
            //TODO:�л��������泡��
        });
    }

    //����Ѫ������
    public void UpdateHpBar(int currHp,int maxHp)
    {
        //����Ѫ������
        ScrollBar_hp.size = currHp / maxHp;
        //������������
        Txt_hpValue.text = currHp + "/" + maxHp;
    }
}

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

        //����������
        Btn_back.onClick.AddListener(() =>
        {
            //TODO:�л��������泡��
        });
    }

    //����Ѫ������
    public void UpdateHpBar(int currHp,int maxHp)
    {
        float currHp1 = (float)currHp;
        float maxHp1 = (float)maxHp; 
        //����Ѫ������
        ScrollBar_hp.size = currHp1 / maxHp1;
        //������������
        Txt_hpValue.text = currHp1 + "/" + maxHp1;
    }

    //�����ӵ��ķ���
    public void UpdateBulletNum(int currNum,int maxNum)
    {
        Txt_BulletNum.text = currNum + " / " + maxNum;
    }

    //�����ռ��������ķ���
    public void UpdateItemNum(int num)
    {
        Txt_Item1Num.text = num.ToString();
    }
}

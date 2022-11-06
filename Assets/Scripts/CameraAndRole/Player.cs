using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //引入移动组件
    private CharacterMove characterMove;
    //引入摄像机组件
    private Photographer photographer;
    //动画组件
    private Animator anim;

    //摄像机的根位置
    public Transform followTarget;

    //枪的开火点位置
    public Transform firePoint;

    //Test:玩家信息
    private int currHp;
    private RoleInfo info;
    public GamePanel panel;

    private void Awake()
    {
        //Test:加载出血量UI
        UIManager.Instance.ShowPanel<GamePanel>("GamePanel");
    }

    void Start()
    {
        //Test:玩家信息赋值
        info = DataManager.Instance.roleInfoList[0];
        currHp = info.hp;

        //输入事件
        InputManager.Instance.SwitchState(true);
        EventManager.Instance.AddEventListener<KeyCode>("KeyDown", CheckOtherInputDown);
        EventManager.Instance.AddEventListener<KeyCode>("KeyUp", CheckOtherInputUp);
        EventManager.Instance.AddEventListener<int>("MouseDown", CheckMouseInputDown);

        anim = GetComponent<Animator>();
        characterMove = GetComponent<CharacterMove>();

        photographer = Camera.main.transform.parent.GetComponent<Photographer>();
        photographer.InitCamera(followTarget);

        //Test:
        panel = GameObject.Find("Canvas/GamePanel").GetComponent<GamePanel>();
        //panel.UpdateHpBar(currHp, info.hp);
    }

    void Update()
    {
        UpdateMovementInput();
    }

    //移动输入检测
    private void UpdateMovementInput()
    {
        //获取玩家输入值
        float ad = Input.GetAxis("Horizontal");
        float ws = Input.GetAxis("Vertical");

        Quaternion rot = Quaternion.Euler(0, photographer.Yaw, 0);
        characterMove.SetMovementInput(rot * Vector3.forward * ws +
                                       rot * Vector3.right * ad);
        gameObject.transform.rotation = rot;

        //实时设置动画
        anim.SetFloat("HSpeed", ws);
        anim.SetFloat("VSpeed", ad);
    }

    //键盘按下输入检测
    public void CheckOtherInputDown(KeyCode key)
    {
        //翻滚
        if (key == KeyCode.LeftShift)
        {
            anim.SetTrigger("Roll");
            //print("正在翻滚");
        }
        //跳跃
        if (key == KeyCode.Space)
        {
            //TODO:跳跃逻辑
        }

        //蹲起
        if (key == KeyCode.LeftControl)
        {
            anim.SetLayerWeight(1, 1);
            //print("蹲下");
        }
    }

    //键盘抬起输入检测
    public void CheckOtherInputUp(KeyCode key)
    {
        //蹲起
        if (key == KeyCode.LeftControl)
        {
            anim.SetLayerWeight(1, 0);
        }
    }


    //鼠标输入检测
    public void CheckMouseInputDown(int mouseEvent)
    {
        if (mouseEvent == 0)
        {
            anim.SetTrigger("Attack");
        }

        if (mouseEvent == 1)
        {
            //翻滚逻辑
            anim.SetTrigger("Roll");
        }
    }

    //受伤逻辑
    public void Wound(int damage)
    {
        //扣血
        currHp -= damage;
        //更新血条UI
        panel.UpdateHpBar(currHp, info.hp);
    }

    #region 动画事件
    public void TakeKnifeDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position + Vector3.forward + Vector3.up, 1 , 1 << LayerMask.NameToLayer("Monster"));
        foreach (Collider c in colliders)
        {
            //TODO:遍历到受到伤害的所有碰撞体，对其调用受伤函数
        }
    }


    //拿枪的造成伤害事件
    public void TakeGunDamage()
    {
        RaycastHit[] raycastHits =  Physics.RaycastAll(new Ray(firePoint.position, firePoint.forward), 1000, 1 << LayerMask.NameToLayer("Monster"));
        
        foreach (RaycastHit r in raycastHits)
        {
            //TODO:遍历所有被射线检测到的物体，对其调用受伤函数
        }
    }
    #endregion
}

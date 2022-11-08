using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //引入移动组件
    private CharacterMove characterMove;
    //引入摄像机组件
    private Photographer photographer;
    //动画组件
    private Animator anim;
    //刚体组件
    private Rigidbody rigidbody;

    //摄像机的根位置
    public Transform followTarget;

    //枪的开火点位置
    public Transform firePoint;
    //枪手瞄准准心
    private Transform AimPoint;

    //Test:玩家信息
    private int currHp;
    private RoleInfo info;
    public GamePanel panel;

    //是否可以跳跃
    bool canJump = true;

    void Start()
    {
        //准心赋值
        AimPoint = GameObject.Find("Canvas/AimPoint").transform;

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
        rigidbody = GetComponent<Rigidbody>();

        photographer = Camera.main.transform.parent.GetComponent<Photographer>();
        photographer.InitCamera(followTarget);

        panel = GameObject.Find("Canvas/GamePanel").GetComponent<GamePanel>();
        //panel.UpdateHpBar(currHp, info.hp);

        //anim.SetBool("canJump", true);
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
            print("按下跳跃键");
            
            if (canJump)
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, 5, rigidbody.velocity.z);
            
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = false;
        }
    }

    #region 动画事件
    public void TakeKnifeDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position + Vector3.forward + Vector3.up, 1.5f , 1 << LayerMask.NameToLayer("Monster"));
        foreach (Collider c in colliders)
        {
            //TODO:遍历到受到伤害的所有碰撞体，对其调用受伤函数
            c.GetComponent<ZombiesInGame>().Wound(info.attack);
        }
    }


    //拿枪的造成伤害事件
    public void TakeGunDamage()
    {
        //释放特效
        ResourcesManager.Instance.LoadAsync<GameObject>("Effect/FireImpactSmall", (obj) =>
        {
            obj.transform.position = firePoint.position;
            obj.transform.rotation = Quaternion.AngleAxis(90,firePoint.forward);
            Destroy(obj.gameObject,0.8f);
        });

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        //RaycastHit[] raycastHits =  Physics.RaycastAll(new Ray(firePoint.position, firePoint.forward), 1000, 1 << LayerMask.NameToLayer("Monster"));
        RaycastHit[] raycastHits = Physics.RaycastAll(ray, 1000, 1 << LayerMask.NameToLayer("Monster"));
        foreach (RaycastHit r in raycastHits)
        {
            //修改图标颜色
            AimPoint.GetComponent<Image>().color = Color.red;
            //0.5秒后修改回来
            Invoke("UpdateColor", 0.5f);
            //TODO:遍历所有被射线检测到的物体，对其调用受伤函数
            r.transform.GetComponent<ZombiesInGame>().Wound(info.attack);
        }
    }

    private void UpdateColor()
    {
        AimPoint.GetComponent<Image>().color = Color.white;
    }
    
    public void TakeFireDamage()
    {
        //释放特效
        ResourcesManager.Instance.LoadAsync<GameObject>("Effect/FireProjectileMega", (obj) =>
        {
            obj.transform.position = firePoint.position;
            obj.transform.rotation = Quaternion.AngleAxis(90, firePoint.forward);
            Destroy(obj.gameObject, 0.8f);
        });

        Collider[] colliders = Physics.OverlapSphere(firePoint.position, 1.5f, 1 << LayerMask.NameToLayer("Monster"));
        foreach (Collider c in colliders)
        {
            //TODO:遍历到受到伤害的所有碰撞体，对其调用受伤函数
            c.GetComponent<ZombiesInGame>().Wound(info.attack);
        }
    }
    #endregion
}

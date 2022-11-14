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
    //音效组件(专门播放走路)
    private AudioSource audio;
    //走路音效切片
    private AudioClip walkClip;

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
    //上帝模式(翻滚时设为无敌状态)
    [HideInInspector]
    public bool isGod = false;
    //玩家是否在换弹
    private bool isReload = false;

    //玩家移动信息
    float ad;
    float ws;

    //翻滚力的大小
    public float rollForce = 2;
    //翻滚冷却时间(表格读取)
    private float rollCooldown = 1;
    private float rollTimer = 0;

    //子弹数量(表格读取)
    private int bullet_currNum = 8;
    private int currBullet = 8;
    private int bullet_maxNum = 64;
    private int maxBullet = 64;

    //攻击冷却及其计时器(表格读取)
    private float attackCooldown = 0.5f;
    private float attackTimer = 0;

    //走路音效是否正在播放
    private bool isPlayWalk;

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

        //组件赋值
        anim = GetComponent<Animator>();
        characterMove = GetComponent<CharacterMove>();
        rigidbody = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
        walkClip = ResourcesManager.Instance.Load<AudioClip>("Audio/Sound/脚步声城市乡镇");

        photographer = Camera.main.transform.parent.GetComponent<Photographer>();
        photographer.InitCamera(followTarget);

        panel = GameObject.Find("Canvas/GamePanel").GetComponent<GamePanel>();
        //panel.UpdateHpBar(currHp, info.hp);

        //anim.SetBool("canJump", true);
    }

    void Update()
    {
        UpdateMovementInput();
        RollAndAttack();
    }

    //移动输入检测
    private void UpdateMovementInput()
    {
        //获取玩家输入值
        ad = Input.GetAxis("Horizontal");
        ws = Input.GetAxis("Vertical");

        Quaternion rot = Quaternion.Euler(0, photographer.Yaw, 0);
        characterMove.SetMovementInput(rot * Vector3.forward * ws +
                                       rot * Vector3.right * ad);
        gameObject.transform.rotation = rot;

        //实时设置动画
        anim.SetFloat("HSpeed", ws);
        anim.SetFloat("VSpeed", ad);

        //走路音效
        if (ad != 0 || ws != 0)
        {
            if (!audio.isPlaying)
            {
                audio.PlayOneShot(walkClip, 0.8f);
            }
        }
        else
        {
            if (audio.isPlaying)
            {
                audio.Stop();
            }
        }
    }

    //键盘按下输入检测
    public void CheckOtherInputDown(KeyCode key)
    {
        //翻滚
        if (key == KeyCode.LeftShift)
        {
            //冷却时间判断
            if (rollTimer >= rollCooldown)
            {
                //冷却已完成(进入冷却)
                rollTimer = 0;
                //翻滚逻辑
                anim.SetTrigger("Roll");
            }
        }

        //跳跃
        if (key == KeyCode.Space)
        {       
            if (canJump)
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, 5, rigidbody.velocity.z);
        }

        //蹲起
        if (key == KeyCode.LeftControl)
        {
            anim.SetLayerWeight(1, 1);
        }

        //上子弹
        if (key == KeyCode.R)
        {
            //当所有子弹都打完了
            if (maxBullet == 0)
                AudioManager.Instance.PlaySound("卡膛");
            else if (currBullet < bullet_currNum && !isReload)
            {
                isReload = true;
                anim.SetTrigger("Reload");
                AudioManager.Instance.PlaySound("上子弹");
            }

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
        //射击
        if (mouseEvent == 0)
        {
            if (attackTimer >= attackCooldown)
            {
                //冷却完成
                attackTimer = 0;
                if (currBullet <= 0)
                {
                    //该弹夹没有子弹了
                    currBullet = 0;
                    if (!isReload && maxBullet != 0)
                    {
                        //如果没有正在上弹
                        isReload = true;
                        anim.SetTrigger("Reload");
                        //播放上子弹音效
                        AudioManager.Instance.PlaySound("上子弹");
                    }
                    if (maxBullet == 0)
                    {
                        //子弹打空或者正在上弹，提示音效
                        AudioManager.Instance.PlaySound("卡膛");
                    }

                }
                else
                {
                    anim.SetTrigger("Attack");
                    //减少子弹
                    --currBullet;
                }
            }
            

            panel.UpdateBulletNum(currBullet, maxBullet);
        }
        //翻滚
        if (mouseEvent == 1)
        {
            //TODO:瞄准射击(近战重击)
        }
    }

    //翻滚和攻击冷却限制检测
    private void RollAndAttack()
    {
        rollTimer += Time.deltaTime;

        //攻击冷却
        attackTimer += Time.deltaTime;

        //翻滚中的限制
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Roll"))
        {
            isGod = true;
            //正在翻滚时，关闭输入检测，并施加位移效果(此处注意，角色移动是Input检测的，单纯关闭输入检测对移动无效)
            InputManager.Instance.SwitchState(false);
            ad = 0;
            ws = 0;
            rigidbody.AddForce(transform.forward * rollForce,ForceMode.Impulse);
        }
        else
        {
            isGod = false;
            InputManager.Instance.SwitchState(true);
        }
    }

    //受伤逻辑
    public void Wound(int damage)
    {
        if (isGod) return;
        //扣血
        currHp -= damage;
        //更新血条UI
        panel.UpdateHpBar(currHp, info.hp);
    }

    #region 碰撞检测
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
    #endregion

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
            ResourcesManager.Instance.LoadAsync<GameObject>("Effect/FireImpactSmall", (obj) =>
             {
                 obj.transform.position = r.transform.position + Vector3.up;
                 obj.transform.rotation = Quaternion.identity;
                 Destroy(obj.gameObject, 1.5f);
             });
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

    //上完子弹执行的操作
    public void Reload()
    {
        //换弹夹的计算
        if (maxBullet >= bullet_currNum - currBullet)
        {
            //如果总弹夹子弹足够一发完整弹夹
            maxBullet -= bullet_currNum - currBullet;
            currBullet = bullet_currNum;
        }
        else
        {
            //子弹不够一发完整弹夹
            currBullet = maxBullet;
            maxBullet = 0;
        }
        isReload = false;
        panel.UpdateBulletNum(currBullet, maxBullet);
    }
    #endregion
}

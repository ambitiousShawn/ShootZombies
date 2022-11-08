using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ZombiesInGame : MonoBehaviour
{
    #region 变量
    //动画组件
    private Animator anim;
    //寻路AI组件
    private NavMeshAgent agent;

    //怪物是否死亡
    public bool isDead;
    //怪物剩余生命值
    private int currHp;

    //怪物数据
    private MonsterInfo info;
    #endregion

    //玩家的实时位置
    private Transform targetPos;

    //发现玩家的反应
    private bool isFound;

    #region 生命周期函数
    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        //Test:测试用
        info = DataManager.Instance.monsterInfoList[0];
        InitZombie(info);
    }

    // Update is called once per frame
    void Update()
    {
        IsDead();
        if (isDead) return;
        CheckAttackRangeAndTakeDamage();
    }

    #endregion
    //初始化僵尸(该函数由出怪点调用)
    public void InitZombie(MonsterInfo info)
    {
        //设置状态和血量
        isDead = false;
        currHp = info.hp;
        //初始化速度
        //TODO:待修改
        agent.speed = agent.acceleration = info.moveSpeed/20;
        //设置自动停止距离
        agent.stoppingDistance = 1.5f;

        //添加动画
        anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(info.animator);
        //添加追击目标
        targetPos = GameObject.Find("Player").transform;
    }

    //丧尸的死亡
    private void IsDead()
    {
        if (currHp <= 0)
        {
            //玩家死亡后，将其目标点设置为自身，并关闭跟踪AI。
            anim.SetBool("isDead", true);
            anim.SetBool("canAtk", false);
            agent.SetDestination(this.transform.position);
            agent.isStopped = true;
            
            isDead = true;
        }
        else
        {
            isDead = false;
        }
    }

    //僵尸的受伤(由玩家范围检测后调用)
    public void Wound(int damage)
    {
        //播放受伤动画
        //anim.SetTrigger("Wound");
        //扣除自身血量
        currHp -= damage;
        print("当前剩余血量" + currHp);
    }

    //攻击距离检测以及攻击并造成伤害(核心AI，待优化)
    private void CheckAttackRangeAndTakeDamage()
    {
        //TODO:丧尸出生后，会优先Idle，当发现自己身前存在玩家时，会进行猛烈嘶吼后追逐，近身后开始攻击。当玩家拉开时，会继续跟进追逐。
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Emerge"))
            return;

        if (Vector3.Angle(transform.forward, (targetPos.position - transform.position).normalized) < 90 &&
            Vector3.Angle(transform.forward, (targetPos.position - transform.position).normalized) > -90 &&
            Vector3.Distance(transform.position, targetPos.position) < 15)
        {
            //发现玩家目标
            print("发现玩家");
            
            //爬出来发现玩家后，播放完成爬出动画再进行嘶吼
            if (!isFound)
            {
                anim.SetBool("FoundPlayer",true);
                isFound = true;//保证不会重复播放FoundPlayer嘶吼
            }

            //TODO:攻击距离可写成Json持久化数据
            if (Vector3.Distance(transform.position, targetPos.position) <= 1.5f)
            {
                //停止寻路并开始攻击
                 agent.isStopped = true;
                 //开始攻击之前，先让其面对玩家(注意Y轴的偏移量)
                 transform.LookAt(new Vector3(targetPos.position.x, transform.position.y, targetPos.position.z));
                 anim.SetBool("canAtk", true);
            }
            else
            {
                /*
                    第一种情况：嘶吼动画播放完开始追逐，需注意延迟调用。
                    第二种情况：丧尸攻击距离被拉开，直接开始追逐
                 */
                anim.SetBool("canAtk", false);
                Invoke("SetTargetPos", 1.8f);
                
            }
        }
        else
        {
            //TODO:未发现玩家，后续实现随机选择位置走路
            isFound = false;
            anim.SetBool("FoundPlayer", false);
        }

        //切换动画
        anim.SetBool("isRun", agent.velocity != Vector3.zero);

    }

    //丧尸嘶吼后开始追逐
    private void SetTargetPos()
    {
        //当距离超过攻击距离，继续追赶
        agent.isStopped = false;
        //重新设置目标位置
        agent.SetDestination(targetPos.position);
    }

    #region 动画事件
    //死亡动画播完后 销毁自身
    public void DestroySelf()
    {
        //15s后自动销毁自身
        Destroy(this.gameObject,15f);
    }

    //攻击的动画检测
    public void TakeDamageToPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position + Vector3.up + Vector3.forward, 2, 1 << LayerMask.NameToLayer("Player")) ;

        //遍历碰撞体(此处有些多余，因为玩家只有一个)
        foreach (Collider c in colliders)
        {
            //TODO:调用玩家自身的受伤逻辑即可
            print("玩家被攻击了");
            targetPos.GetComponent<Player>().Wound(info.atk);
        }
    }
    #endregion
}

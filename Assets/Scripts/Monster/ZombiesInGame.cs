using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ZombiesInGame : MonoBehaviour
{
    #region ����
    //�������
    private Animator anim;
    //Ѱ·AI���
    private NavMeshAgent agent;

    //�����Ƿ�����
    public bool isDead;
    //����ʣ������ֵ
    private int currHp;

    //��������
    private MonsterInfo info;
    #endregion

    //��ҵ�ʵʱλ��
    private Transform targetPos;

    //������ҵķ�Ӧ
    private bool isFound;

    #region �������ں���
    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        //Test:������
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
    //��ʼ����ʬ(�ú����ɳ��ֵ����)
    public void InitZombie(MonsterInfo info)
    {
        //����״̬��Ѫ��
        isDead = false;
        currHp = info.hp;
        //��ʼ���ٶ�
        //TODO:���޸�
        agent.speed = agent.acceleration = info.moveSpeed/20;
        //�����Զ�ֹͣ����
        agent.stoppingDistance = 1.5f;

        //��Ӷ���
        anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(info.animator);
        //���׷��Ŀ��
        targetPos = GameObject.Find("Player").transform;
    }

    //ɥʬ������
    private void IsDead()
    {
        if (currHp <= 0)
        {
            //��������󣬽���Ŀ�������Ϊ�������رո���AI��
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

    //��ʬ������(����ҷ�Χ�������)
    public void Wound(int damage)
    {
        //�������˶���
        //anim.SetTrigger("Wound");
        //�۳�����Ѫ��
        currHp -= damage;
        print("��ǰʣ��Ѫ��" + currHp);
    }

    //�����������Լ�����������˺�(����AI�����Ż�)
    private void CheckAttackRangeAndTakeDamage()
    {
        //TODO:ɥʬ�����󣬻�����Idle���������Լ���ǰ�������ʱ�����������˻���׷�𣬽����ʼ���������������ʱ�����������׷��
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Emerge"))
            return;

        if (Vector3.Angle(transform.forward, (targetPos.position - transform.position).normalized) < 90 &&
            Vector3.Angle(transform.forward, (targetPos.position - transform.position).normalized) > -90 &&
            Vector3.Distance(transform.position, targetPos.position) < 15)
        {
            //�������Ŀ��
            print("�������");
            
            //������������Һ󣬲���������������ٽ���˻��
            if (!isFound)
            {
                anim.SetBool("FoundPlayer",true);
                isFound = true;//��֤�����ظ�����FoundPlayer˻��
            }

            //TODO:���������д��Json�־û�����
            if (Vector3.Distance(transform.position, targetPos.position) <= 1.5f)
            {
                //ֹͣѰ·����ʼ����
                 agent.isStopped = true;
                 //��ʼ����֮ǰ��������������(ע��Y���ƫ����)
                 transform.LookAt(new Vector3(targetPos.position.x, transform.position.y, targetPos.position.z));
                 anim.SetBool("canAtk", true);
            }
            else
            {
                /*
                    ��һ�������˻�𶯻������꿪ʼ׷����ע���ӳٵ��á�
                    �ڶ��������ɥʬ�������뱻������ֱ�ӿ�ʼ׷��
                 */
                anim.SetBool("canAtk", false);
                Invoke("SetTargetPos", 1.8f);
                
            }
        }
        else
        {
            //TODO:δ������ң�����ʵ�����ѡ��λ����·
            isFound = false;
            anim.SetBool("FoundPlayer", false);
        }

        //�л�����
        anim.SetBool("isRun", agent.velocity != Vector3.zero);

    }

    //ɥʬ˻���ʼ׷��
    private void SetTargetPos()
    {
        //�����볬���������룬����׷��
        agent.isStopped = false;
        //��������Ŀ��λ��
        agent.SetDestination(targetPos.position);
    }

    #region �����¼�
    //������������� ��������
    public void DestroySelf()
    {
        //15s���Զ���������
        Destroy(this.gameObject,15f);
    }

    //�����Ķ������
    public void TakeDamageToPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position + Vector3.up + Vector3.forward, 2, 1 << LayerMask.NameToLayer("Player")) ;

        //������ײ��(�˴���Щ���࣬��Ϊ���ֻ��һ��)
        foreach (Collider c in colliders)
        {
            //TODO:�����������������߼�����
            print("��ұ�������");
            targetPos.GetComponent<Player>().Wound(info.atk);
        }
    }
    #endregion
}

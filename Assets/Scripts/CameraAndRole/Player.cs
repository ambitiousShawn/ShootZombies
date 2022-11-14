using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //�����ƶ����
    private CharacterMove characterMove;
    //������������
    private Photographer photographer;
    //�������
    private Animator anim;
    //�������
    private Rigidbody rigidbody;
    //��Ч���(ר�Ų�����·)
    private AudioSource audio;
    //��·��Ч��Ƭ
    private AudioClip walkClip;

    //������ĸ�λ��
    public Transform followTarget;

    //ǹ�Ŀ����λ��
    public Transform firePoint;
    //ǹ����׼׼��
    private Transform AimPoint;

    //Test:�����Ϣ
    private int currHp;
    private RoleInfo info;
    public GamePanel panel;

    //�Ƿ������Ծ
    bool canJump = true;
    //�ϵ�ģʽ(����ʱ��Ϊ�޵�״̬)
    [HideInInspector]
    public bool isGod = false;
    //����Ƿ��ڻ���
    private bool isReload = false;

    //����ƶ���Ϣ
    float ad;
    float ws;

    //�������Ĵ�С
    public float rollForce = 2;
    //������ȴʱ��(����ȡ)
    private float rollCooldown = 1;
    private float rollTimer = 0;

    //�ӵ�����(����ȡ)
    private int bullet_currNum = 8;
    private int currBullet = 8;
    private int bullet_maxNum = 64;
    private int maxBullet = 64;

    //������ȴ�����ʱ��(����ȡ)
    private float attackCooldown = 0.5f;
    private float attackTimer = 0;

    //��·��Ч�Ƿ����ڲ���
    private bool isPlayWalk;

    void Start()
    {
        //׼�ĸ�ֵ
        AimPoint = GameObject.Find("Canvas/AimPoint").transform;

        //Test:�����Ϣ��ֵ
        info = DataManager.Instance.roleInfoList[0];
        currHp = info.hp;

        //�����¼�
        InputManager.Instance.SwitchState(true);
        EventManager.Instance.AddEventListener<KeyCode>("KeyDown", CheckOtherInputDown);
        EventManager.Instance.AddEventListener<KeyCode>("KeyUp", CheckOtherInputUp);
        EventManager.Instance.AddEventListener<int>("MouseDown", CheckMouseInputDown);

        //�����ֵ
        anim = GetComponent<Animator>();
        characterMove = GetComponent<CharacterMove>();
        rigidbody = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
        walkClip = ResourcesManager.Instance.Load<AudioClip>("Audio/Sound/�Ų�����������");

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

    //�ƶ�������
    private void UpdateMovementInput()
    {
        //��ȡ�������ֵ
        ad = Input.GetAxis("Horizontal");
        ws = Input.GetAxis("Vertical");

        Quaternion rot = Quaternion.Euler(0, photographer.Yaw, 0);
        characterMove.SetMovementInput(rot * Vector3.forward * ws +
                                       rot * Vector3.right * ad);
        gameObject.transform.rotation = rot;

        //ʵʱ���ö���
        anim.SetFloat("HSpeed", ws);
        anim.SetFloat("VSpeed", ad);

        //��·��Ч
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

    //���̰���������
    public void CheckOtherInputDown(KeyCode key)
    {
        //����
        if (key == KeyCode.LeftShift)
        {
            //��ȴʱ���ж�
            if (rollTimer >= rollCooldown)
            {
                //��ȴ�����(������ȴ)
                rollTimer = 0;
                //�����߼�
                anim.SetTrigger("Roll");
            }
        }

        //��Ծ
        if (key == KeyCode.Space)
        {       
            if (canJump)
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, 5, rigidbody.velocity.z);
        }

        //����
        if (key == KeyCode.LeftControl)
        {
            anim.SetLayerWeight(1, 1);
        }

        //���ӵ�
        if (key == KeyCode.R)
        {
            //�������ӵ���������
            if (maxBullet == 0)
                AudioManager.Instance.PlaySound("����");
            else if (currBullet < bullet_currNum && !isReload)
            {
                isReload = true;
                anim.SetTrigger("Reload");
                AudioManager.Instance.PlaySound("���ӵ�");
            }

        }
    }

    //����̧��������
    public void CheckOtherInputUp(KeyCode key)
    {
        //����
        if (key == KeyCode.LeftControl)
        {
            anim.SetLayerWeight(1, 0);
        }
    }


    //���������
    public void CheckMouseInputDown(int mouseEvent)
    {
        //���
        if (mouseEvent == 0)
        {
            if (attackTimer >= attackCooldown)
            {
                //��ȴ���
                attackTimer = 0;
                if (currBullet <= 0)
                {
                    //�õ���û���ӵ���
                    currBullet = 0;
                    if (!isReload && maxBullet != 0)
                    {
                        //���û�������ϵ�
                        isReload = true;
                        anim.SetTrigger("Reload");
                        //�������ӵ���Ч
                        AudioManager.Instance.PlaySound("���ӵ�");
                    }
                    if (maxBullet == 0)
                    {
                        //�ӵ���ջ��������ϵ�����ʾ��Ч
                        AudioManager.Instance.PlaySound("����");
                    }

                }
                else
                {
                    anim.SetTrigger("Attack");
                    //�����ӵ�
                    --currBullet;
                }
            }
            

            panel.UpdateBulletNum(currBullet, maxBullet);
        }
        //����
        if (mouseEvent == 1)
        {
            //TODO:��׼���(��ս�ػ�)
        }
    }

    //�����͹�����ȴ���Ƽ��
    private void RollAndAttack()
    {
        rollTimer += Time.deltaTime;

        //������ȴ
        attackTimer += Time.deltaTime;

        //�����е�����
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Roll"))
        {
            isGod = true;
            //���ڷ���ʱ���ر������⣬��ʩ��λ��Ч��(�˴�ע�⣬��ɫ�ƶ���Input���ģ������ر���������ƶ���Ч)
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

    //�����߼�
    public void Wound(int damage)
    {
        if (isGod) return;
        //��Ѫ
        currHp -= damage;
        //����Ѫ��UI
        panel.UpdateHpBar(currHp, info.hp);
    }

    #region ��ײ���
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

    #region �����¼�
    public void TakeKnifeDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position + Vector3.forward + Vector3.up, 1.5f , 1 << LayerMask.NameToLayer("Monster"));
        foreach (Collider c in colliders)
        {
            //TODO:�������ܵ��˺���������ײ�壬����������˺���
            c.GetComponent<ZombiesInGame>().Wound(info.attack);
        }
    }


    //��ǹ������˺��¼�
    public void TakeGunDamage()
    {
        //�ͷ���Ч
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
            //�޸�ͼ����ɫ
            AimPoint.GetComponent<Image>().color = Color.red;
            //0.5����޸Ļ���
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
        //�ͷ���Ч
        ResourcesManager.Instance.LoadAsync<GameObject>("Effect/FireProjectileMega", (obj) =>
        {
            obj.transform.position = firePoint.position;
            obj.transform.rotation = Quaternion.AngleAxis(90, firePoint.forward);
            Destroy(obj.gameObject, 0.8f);
        });

        Collider[] colliders = Physics.OverlapSphere(firePoint.position, 1.5f, 1 << LayerMask.NameToLayer("Monster"));
        foreach (Collider c in colliders)
        {
            //TODO:�������ܵ��˺���������ײ�壬����������˺���
            c.GetComponent<ZombiesInGame>().Wound(info.attack);
        }
    }

    //�����ӵ�ִ�еĲ���
    public void Reload()
    {
        //�����еļ���
        if (maxBullet >= bullet_currNum - currBullet)
        {
            //����ܵ����ӵ��㹻һ����������
            maxBullet -= bullet_currNum - currBullet;
            currBullet = bullet_currNum;
        }
        else
        {
            //�ӵ�����һ����������
            currBullet = maxBullet;
            maxBullet = 0;
        }
        isReload = false;
        panel.UpdateBulletNum(currBullet, maxBullet);
    }
    #endregion
}

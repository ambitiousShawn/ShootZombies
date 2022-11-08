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

    //�ƶ�������
    private void UpdateMovementInput()
    {
        //��ȡ�������ֵ
        float ad = Input.GetAxis("Horizontal");
        float ws = Input.GetAxis("Vertical");

        Quaternion rot = Quaternion.Euler(0, photographer.Yaw, 0);
        characterMove.SetMovementInput(rot * Vector3.forward * ws +
                                       rot * Vector3.right * ad);
        gameObject.transform.rotation = rot;

        //ʵʱ���ö���
        anim.SetFloat("HSpeed", ws);
        anim.SetFloat("VSpeed", ad);
    }

    //���̰���������
    public void CheckOtherInputDown(KeyCode key)
    {
        //����
        if (key == KeyCode.LeftShift)
        {
            anim.SetTrigger("Roll");
            //print("���ڷ���");
        }
        //��Ծ
        if (key == KeyCode.Space)
        {
            //TODO:��Ծ�߼�
            print("������Ծ��");
            
            if (canJump)
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, 5, rigidbody.velocity.z);
            
        }

        //����
        if (key == KeyCode.LeftControl)
        {
            anim.SetLayerWeight(1, 1);
            //print("����");
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
        if (mouseEvent == 0)
        {
            anim.SetTrigger("Attack");
        }

        if (mouseEvent == 1)
        {
            //�����߼�
            anim.SetTrigger("Roll");
        }
    }

    //�����߼�
    public void Wound(int damage)
    {
        //��Ѫ
        currHp -= damage;
        //����Ѫ��UI
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
            //TODO:�������б����߼�⵽�����壬����������˺���
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
    #endregion
}

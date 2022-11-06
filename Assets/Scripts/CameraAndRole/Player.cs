using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //�����ƶ����
    private CharacterMove characterMove;
    //������������
    private Photographer photographer;
    //�������
    private Animator anim;

    //������ĸ�λ��
    public Transform followTarget;

    //ǹ�Ŀ����λ��
    public Transform firePoint;

    //Test:�����Ϣ
    private int currHp;
    private RoleInfo info;
    public GamePanel panel;

    private void Awake()
    {
        //Test:���س�Ѫ��UI
        UIManager.Instance.ShowPanel<GamePanel>("GamePanel");
    }

    void Start()
    {
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

    #region �����¼�
    public void TakeKnifeDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position + Vector3.forward + Vector3.up, 1 , 1 << LayerMask.NameToLayer("Monster"));
        foreach (Collider c in colliders)
        {
            //TODO:�������ܵ��˺���������ײ�壬����������˺���
        }
    }


    //��ǹ������˺��¼�
    public void TakeGunDamage()
    {
        RaycastHit[] raycastHits =  Physics.RaycastAll(new Ray(firePoint.position, firePoint.forward), 1000, 1 << LayerMask.NameToLayer("Monster"));
        
        foreach (RaycastHit r in raycastHits)
        {
            //TODO:�������б����߼�⵽�����壬����������˺���
        }
    }
    #endregion
}

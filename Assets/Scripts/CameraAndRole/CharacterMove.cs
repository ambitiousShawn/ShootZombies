using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ����һ���ƶ����ܽű����������а�����ң����˵ȶ�����ƶ�
 */

[RequireComponent(typeof(Rigidbody))]
public class CharacterMove : MonoBehaviour
{
    //�������
    private Rigidbody rigidbody;

    //�����ƶ����
    public float MaxWalkSpeed = 5;
    //������ҵ�����
    public Vector3 CurrentInput { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //print(CurrentInput);
        rigidbody.MovePosition(rigidbody.position + CurrentInput * MaxWalkSpeed * Time.fixedDeltaTime);
    }

    //�����ƶ�����ƫ��ֵ
    public void SetMovementInput(Vector3 input)
    {
        CurrentInput = Vector3.ClampMagnitude(input, 1);
    }
}

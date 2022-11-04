using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    这是一个移动功能脚本。管理所有包括玩家，敌人等对象的移动
 */

[RequireComponent(typeof(Rigidbody))]
public class CharacterMove : MonoBehaviour
{
    //刚体组件
    private Rigidbody rigidbody;

    //人物移动相关
    public float MaxWalkSpeed = 5;
    //接收玩家的输入
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

    //设置移动输入偏移值
    public void SetMovementInput(Vector3 input)
    {
        CurrentInput = Vector3.ClampMagnitude(input, 1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //主摄像机
    private Camera mainCam;
    //选择切换角色后需要移动到的位置
    public Transform selectPos;

    #region 布尔变量
    public bool switchSelectAnim = false;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        //当玩家点击开始游戏时，切换至选择角色页面
        if (switchSelectAnim)
            MoveToSelect();
    }

    //摄像机移动到选择角色面板
    public void MoveToSelect()
    {
        GetComponent<Animator>().enabled = false;
        MoveToTarget(selectPos);
    }


    //摄像机移动到指定位置与角度
    private void MoveToTarget(Transform trans)
    {
        this.transform.position = Vector3.Lerp(transform.position, trans.position, Time.deltaTime);
        this.transform.rotation = Quaternion.Slerp(transform.rotation, trans.rotation, Time.deltaTime);
        if (transform.position.x == trans.position.x)
        {
            switchSelectAnim = false;
        }
    }
}

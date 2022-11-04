using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//游戏中相机跟随角色的脚本
public class Photographer : MonoBehaviour
{
    //相机抬升(绕X轴)
    public float Pitch { get; private set; }
    //相机水平摆动
    public float Yaw { get; private set; }

    //鼠标灵敏度
    public float mouseSensitivity = 5;
    //相机旋转速度
    public float cameraRotationSpeed = 80;
    public float cameraYSpeed = 5;

    //相机跟随目标
    private Transform followTarget;
    //相机臂长的优化设置
    public AnimationCurve camArmLen;
    //相机组件
    private Camera mainCam;

    //初始化相机位置以及跟随
    public void InitCamera(Transform target)
    {
        mainCam = Camera.main;
        followTarget = target;
        transform.position = target.position; 
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRotation();
        UpdatePosition();
        UpdateCamArmLen();
    }

    //检测旋转更新
    private void UpdateRotation()
    {
        Yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        Yaw += Input.GetAxis("CameraRateX") * cameraRotationSpeed * Time.deltaTime;
        //print("水平角度" + Yaw);
        Pitch += Input.GetAxis("Mouse Y") * mouseSensitivity;
        Pitch += Input.GetAxis("CameraRateY") * cameraRotationSpeed * Time.deltaTime;
        //限制抬升角度
        Pitch = Mathf.Clamp(Pitch, -75, 75);
        //print("抬升角度"+Pitch);

        //修改摄像机当前角度
        transform.rotation= Quaternion.Euler(Pitch, Yaw, 0);
    }

    //检测位置更新
    private void UpdatePosition()
    {
        Vector3 position = followTarget.position;
        float newY = Mathf.Lerp(transform.position.y, position.y, Time.deltaTime * cameraYSpeed);
        transform.position = new Vector3(position.x, newY, position.z);
    }

    private void UpdateCamArmLen()
    {
        mainCam.transform.localPosition = new Vector3(0,0,camArmLen.Evaluate(Pitch)* -1);
    }
}

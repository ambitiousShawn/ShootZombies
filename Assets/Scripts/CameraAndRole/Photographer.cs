using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//��Ϸ����������ɫ�Ľű�
public class Photographer : MonoBehaviour
{
    //���̧��(��X��)
    public float Pitch { get; private set; }
    //���ˮƽ�ڶ�
    public float Yaw { get; private set; }

    //���������
    public float mouseSensitivity = 5;
    //�����ת�ٶ�
    public float cameraRotationSpeed = 80;
    public float cameraYSpeed = 5;

    //�������Ŀ��
    private Transform followTarget;
    //����۳����Ż�����
    public AnimationCurve camArmLen;
    //������
    private Camera mainCam;

    //��ʼ�����λ���Լ�����
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

    //�����ת����
    private void UpdateRotation()
    {
        Yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        Yaw += Input.GetAxis("CameraRateX") * cameraRotationSpeed * Time.deltaTime;
        //print("ˮƽ�Ƕ�" + Yaw);
        Pitch += Input.GetAxis("Mouse Y") * mouseSensitivity;
        Pitch += Input.GetAxis("CameraRateY") * cameraRotationSpeed * Time.deltaTime;
        //����̧���Ƕ�
        Pitch = Mathf.Clamp(Pitch, -75, 75);
        //print("̧���Ƕ�"+Pitch);

        //�޸��������ǰ�Ƕ�
        transform.rotation= Quaternion.Euler(Pitch, Yaw, 0);
    }

    //���λ�ø���
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

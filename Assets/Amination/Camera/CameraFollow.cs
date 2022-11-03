using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //�������
    private Camera mainCam;
    //ѡ���л���ɫ����Ҫ�ƶ�����λ��
    public Transform selectPos;

    #region ��������
    public bool switchSelectAnim = false;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        //����ҵ����ʼ��Ϸʱ���л���ѡ���ɫҳ��
        if (switchSelectAnim)
            MoveToSelect();
    }

    //������ƶ���ѡ���ɫ���
    public void MoveToSelect()
    {
        GetComponent<Animator>().enabled = false;
        MoveToTarget(selectPos);
    }


    //������ƶ���ָ��λ����Ƕ�
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

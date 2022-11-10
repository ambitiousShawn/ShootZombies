using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    ��ҵĽ����߼�
 */

public class Interaction : MonoBehaviour
{
    //��ʾ���UI
    private GameObject ActionPanel;
    //��ʾ�ı�����
    private Text info;

    #region ״̬����
    private bool isPollution;
    private bool isCollection;
    #endregion

    //��ǰ�������ʵ�ӵ������
    private int MedicalNum = 0;
    //��ǰ�Ӵ��ľ�������
    private GameObject currObj;

    private void Start()
    {
        ActionPanel = GameObject.Find("Canvas/ActionPanel");
        info = ActionPanel.transform.GetComponentInChildren<Text>();
    }

    private void Update()
    {
        //���߽��ɽ�����������Ҫ������
        if (isPollution && Input.GetKeyDown(KeyCode.E))
        {
            //TODO:��������������ֲ��
            if (MedicalNum > 0)
            {
                //TODO:����ֲ��
                MedicalNum--;
                print("�����ɹ�" + MedicalNum);
            }
            //������������������ʾ
            else
            {
                this.info.text = "��ǰ���߱�����������";
            }
        }

        //���Ӵ�����������
        if (isCollection && Input.GetKeyDown(KeyCode.E))
        {
            MedicalNum++;
            HideTipPanel();
            Destroy(currObj.gameObject);
        }
    }

    //��ʾ��ʾ�������(��ʾ����)
    private void ShowTipPanel(string info)
    {
        //������ʾ����
        this.info.text = info;
        ActionPanel.GetComponent<CanvasGroup>().alpha = 1;
    }

    private void HideTipPanel()
    {
        ActionPanel.GetComponent<CanvasGroup>().alpha = 0;
    }

    //������
    private void OnTriggerEnter(Collider collider)
    {
        switch (collider.tag)
        {
            case "Interactive":
                ShowTipPanel("��E����������");
                print("�Ӵ����ɽ�������");
                isPollution = true;
                break;

            case "Collective":
                ShowTipPanel("��E�ռ���Ʒ");
                print("�Ӵ����ɽ�������");
                isCollection = true;
                currObj = collider.gameObject;
                break;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        switch (collider.tag)
        {
            case "Interactive":
                HideTipPanel();
                print("�뿪���ɽ�������");
                isPollution = false;
                break;
            case "Collective":
                HideTipPanel();
                print("�뿪���ɽ�������");
                isCollection = false;
                currObj = null;
                break;
        }
    }
}

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
    private int medicalNum;
    public int MedicalNum
    {
        get { return medicalNum; }
        set
        {
            medicalNum = value;
            panel.UpdateItemNum(medicalNum);
        }
    }
    //��ǰ�Ӵ��ľ�������
    private GameObject currObj;

    //UI���
    private GamePanel panel;

    private void Start()
    {
        ActionPanel = GameObject.Find("Canvas/ActionPanel");
        info = ActionPanel.transform.GetComponentInChildren<Text>();
        panel = GameObject.Find("Canvas/GamePanel").GetComponent<GamePanel>();
    }

    private void Update()
    {
        //���߽��ɽ�����������Ҫ������
        if (isPollution && Input.GetKeyDown(KeyCode.E))
        {
            //TODO:��������������ֲ��
            if (medicalNum > 0)
            {
                print(--MedicalNum);           
                info.text = "�����ɹ�";
                Destroy(currObj.gameObject);
                isPollution = false;
                currObj = null;
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
            //Bug��ɾ��ʰȡ��󣬲�δִ���߳�������߼���
            MedicalNum++;
            Destroy(currObj.gameObject);
            isCollection = false;
            currObj = null;

        }
    }

    //��ʾ��ʾ�������(��ʾ����)
    private void ShowTipPanel(string info, float time = 999)
    {
        //������ʾ����
        this.info.text = info;
        ActionPanel.GetComponent<CanvasGroup>().alpha = 1;
        Invoke("HideTipPanel", time);
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
                ShowTipPanel("��E����������",5f);
                isPollution = true;
                currObj = collider.gameObject;
                break;

            case "Collective":
                ShowTipPanel("��E�ռ���Ʒ",5f);
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
                isPollution = false;
                currObj = null;
                break;
            case "Collective":
                HideTipPanel();
                isCollection = false;
                currObj = null;
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    玩家的交互逻辑
 */

public class Interaction : MonoBehaviour
{
    //提示面板UI
    private GameObject ActionPanel;
    //提示文本内容
    private Text info;

    #region 状态变量
    private bool isPollution;
    private bool isCollection;
    #endregion

    //当前净化物质的拥有数量
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
    //当前接触的净化物质
    private GameObject currObj;

    //UI面板
    private GamePanel panel;

    private void Start()
    {
        ActionPanel = GameObject.Find("Canvas/ActionPanel");
        info = ActionPanel.transform.GetComponentInChildren<Text>();
        panel = GameObject.Find("Canvas/GamePanel").GetComponent<GamePanel>();
    }

    private void Update()
    {
        //当走进可交互区，按键要做的事
        if (isPollution && Input.GetKeyDown(KeyCode.E))
        {
            //TODO:满足条件：净化植物
            if (medicalNum > 0)
            {
                print(--MedicalNum);           
                info.text = "净化成功";
                Destroy(currObj.gameObject);
                isPollution = false;
                currObj = null;
            }
            //不满足条件：给予提示
            else
            {
                this.info.text = "当前不具备净化的条件";
            }
        }

        //当接触到净化物质
        if (isCollection && Input.GetKeyDown(KeyCode.E))
        {
            //Bug：删除拾取物后，并未执行走出区域的逻辑。
            MedicalNum++;
            Destroy(currObj.gameObject);
            isCollection = false;
            currObj = null;

        }
    }

    //显示提示交互面板(提示内容)
    private void ShowTipPanel(string info, float time = 999)
    {
        //更新提示内容
        this.info.text = info;
        ActionPanel.GetComponent<CanvasGroup>().alpha = 1;
        Invoke("HideTipPanel", time);
    }

    private void HideTipPanel()
    {
        ActionPanel.GetComponent<CanvasGroup>().alpha = 0;
    }

    //触发器
    private void OnTriggerEnter(Collider collider)
    {
        switch (collider.tag)
        {
            case "Interactive":
                ShowTipPanel("按E净化该区域",5f);
                isPollution = true;
                currObj = collider.gameObject;
                break;

            case "Collective":
                ShowTipPanel("按E收集物品",5f);
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

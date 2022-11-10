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
    private int MedicalNum = 0;
    //当前接触的净化物质
    private GameObject currObj;

    private void Start()
    {
        ActionPanel = GameObject.Find("Canvas/ActionPanel");
        info = ActionPanel.transform.GetComponentInChildren<Text>();
    }

    private void Update()
    {
        //当走进可交互区，按键要做的事
        if (isPollution && Input.GetKeyDown(KeyCode.E))
        {
            //TODO:满足条件：净化植物
            if (MedicalNum > 0)
            {
                //TODO:净化植物
                MedicalNum--;
                print("净化成功" + MedicalNum);
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
            MedicalNum++;
            HideTipPanel();
            Destroy(currObj.gameObject);
        }
    }

    //显示提示交互面板(提示内容)
    private void ShowTipPanel(string info)
    {
        //更新提示内容
        this.info.text = info;
        ActionPanel.GetComponent<CanvasGroup>().alpha = 1;
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
                ShowTipPanel("按E净化该区域");
                print("接触到可交互区域");
                isPollution = true;
                break;

            case "Collective":
                ShowTipPanel("按E收集物品");
                print("接触到可交互区域");
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
                print("离开到可交互区域");
                isPollution = false;
                break;
            case "Collective":
                HideTipPanel();
                print("离开到可交互区域");
                isCollection = false;
                currObj = null;
                break;
        }
    }
}

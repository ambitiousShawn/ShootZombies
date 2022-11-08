using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/*
    ʹ��˵������Ҫ��Resources��UIĿ¼�´���CanvasԤ����(���ú���ز�����)
              ��Ҫ����EventSystemԤ���塣

    �ṩ���ܣ�
        1.ͨ���㼶ö�ٵõ���Ӧ�ĸ�����
        2.��ʾĳ���
        3.����ĳ���
        4.�õ�ĳ����������
        5.���ؼ�����Զ����¼�����
 */
public class UIManager : Singleton<UIManager>
{
    public Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    private Transform canvas , eventSystem;

    public UIManager()
    {
        canvas = GameObject.Find("Canvas").transform;
        eventSystem = GameObject.Find("EventSystem").transform;
    }

    //��ʾĳ���
    //Param3:�����Ԥ���崴���ɹ�ʱ����Ҫ���õ��߼�
    public void ShowPanel<T>(string panelName,UnityAction<T> callback = null) where T : BasePanel
    {
        //�������Ѿ����ڣ���ֱ�ӻص���������
        if (panelDic.ContainsKey(panelName))
        {
            if (callback != null)
                callback(panelDic[panelName] as T);

            return;
        }

        //�첽�������
        ResourcesManager.Instance.LoadAsync<GameObject>("UI/" + panelName, (obj) =>
        {
            //�������λ�úʹ�С
            obj.transform.SetParent(canvas);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.name = panelName;

            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;
            

            T panel = obj.GetComponent<T>();
            if (callback != null)
                callback(panel);

            //��ʾ���
            panel.ShowSelf();
            //�洢���
            panelDic.Add(panelName, panel);
        });
    }

    //�Ƿ���Ҫ������Ϻ���ɾ����
    public void HidePanel(string panelName,bool isFade = true)
    {
        if (panelDic.ContainsKey(panelName))
        {
            //����嵭����Ϻ���ɾ�����
            if (isFade)
            {
                panelDic[panelName].HideSelf(() =>
                {
                    //���ع������Ƴ�
                    GameObject.Destroy(panelDic[panelName].gameObject);
                    panelDic.Remove(panelName);
                });
            }
            else
            {
                GameObject.Destroy(panelDic[panelName].gameObject);
                panelDic.Remove(panelName);
            }
        }
    }

    public T GetPanel<T>(string panelName) where T : BasePanel
    {
        if (panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        }
        return null;
    }

    //���ؼ�����Զ����¼�����
    //Param1:�ؼ�����
    //Param2:�¼�����
    //Param3:�¼�����ִ���߼�
    public static void AddCustomEventListener(UIBehaviour control,EventTriggerType type,UnityAction<BaseEventData> callback)
    {
        EventTrigger trigger = control.GetComponent<EventTrigger>();
        if (trigger == null)
           trigger = control.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callback);

        trigger.triggers.Add(entry);
    }
}

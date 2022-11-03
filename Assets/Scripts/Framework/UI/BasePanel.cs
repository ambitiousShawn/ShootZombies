using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 *  ʹ�÷������������̳и��ֱ࣬������������д���Ĵ��뼴�ɣ�������дͨ�õ�ƥ��������߼���
 * 
    ��Ҫ�ҵ��Լ�����µĿؼ�����
    ��Ӧ���ṩ��ʾ �� �����Լ�����Ϊ
 */
public abstract class BasePanel : MonoBehaviour
{
    private Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();

    //�������͸����
    private CanvasGroup canvasGroup;
    //������嵭�뵭�����ٶ�
    private float alphaSpeed = 10;

    #region ״̬����
    private bool isShow = false;
    #endregion

    //���������Ϻ�ִ�е��߼�
    private UnityAction hideCallback = null;

    protected virtual void Awake()
    {
        FindChildrenControl<Button>();
        FindChildrenControl<Image>();
        FindChildrenControl<Text>();
        FindChildrenControl<Toggle>();
        FindChildrenControl<ScrollRect>();
        FindChildrenControl<Slider>();
        FindChildrenControl<Scrollbar>();

        //��ȡ����͸���ȵ����
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    protected virtual void Start()
    {
        Init();
    }

    void Update()
    {
        if (isShow && canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += alphaSpeed;
            if (canvasGroup.alpha >= 1)
                canvasGroup.alpha = 1;
        }

        if (!isShow && canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= alphaSpeed;
            if (canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                hideCallback?.Invoke();
            }
                
        }
    }

    //��ʼ��
    protected abstract void Init();

    //��ʾ�Լ�
    public virtual void ShowSelf()
    {
        //ʵ��˼·�����������͸��������Ϊ0��������Ϊ��ʾ״̬����Update()������ʹ�����͸���Ƚ�����ʾ����
        canvasGroup.alpha = 0;
        isShow = true;
    }

    //�����Լ�
    //����������Ҫ���٣�����Ҫ͸������ȫΪ0ʱ�ſ�ִ��֮���߼���ʹ��ί�к���
    public virtual void HideSelf(UnityAction callback)
    {
        canvasGroup.alpha = 1;
        isShow = false;
        hideCallback = callback;
    }

    //��ť�ĵ���¼�
    //Param1:��ť������Ϸ������
    protected virtual void OnClick(string btnName)
    {

    }

    //��ѡ��ĸ�ֵ�¼�
    //Param1:��ѡ��������Ϸ������
    //Param2:��ѡ��Ĭ��״ֵ̬
    protected virtual void OnValueChanged(string toggleName,bool value)
    {

    }

    //�ҵ��Ӷ���Ķ�Ӧ�ؼ�
    private void FindChildrenControl<T>() where T : UIBehaviour
    {
        T[] controls = GetComponentsInChildren<T>();

        foreach (T control in controls)
        {
            string objName = control.gameObject.name;
            if (controlDic.ContainsKey(objName))
                controlDic[objName].Add(control);
            else
                controlDic.Add(objName, new List<UIBehaviour>() { control });

            //����ǰ�ť������Զ�Ϊ����ӵ������
            if (control is Button)
            {
                (control as Button).onClick.AddListener(() =>
                {
                    OnClick(objName);
                });
            }
            //����ǵ�ѡ��������Զ�Ϊ����Ӹ�ֵ����
            else if (control is Toggle)
            {
                (control as Toggle).onValueChanged.AddListener((value) =>
                {
                    OnValueChanged(objName, value);
                });
            }
        }
    }

    //�õ���Ӧ���ֵĶ�Ӧ�ű�
    protected T GetControl<T>(string controlName) where T : UIBehaviour
    {
        if (controlDic.ContainsKey(controlName))
        {
            //����ֵ��д��ڶ�Ӧ����
            for (int i = 0;i < controlDic[controlName].Count; ++i)
            {
                if (controlDic[controlName][i] is T)
                    return controlDic[controlName][i] as T;
            }
        }
        return null;
    }
}

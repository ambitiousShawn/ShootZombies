using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
    ʹ�÷�����
        ����Ҫ����ĳ�¼�����Ϸ����AddEventListener������Ӧ�¼�
        ����ʱ���¼����Ļ����ζԼ����˸��¼��Ķ���ִ�ж�Ӧ�߼���
        ��д�������ء�
        ���÷��ͺ���ʽת��ԭ������˴�objectת���͵�װ����䣬��ͨ����������
        �����ʱ��������ĵķ����ԡ�
 */
public interface IEventInfo{  };

//���Ͱ�װ��
public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> actions; 

    public EventInfo(UnityAction<T> actions)
    {
        this.actions = actions;
    }
}

public class EventInfo : IEventInfo
{
    public UnityAction actions;

    public EventInfo(UnityAction actions)
    {
        this.actions = actions;
    }
}

public class EventManager : Singleton<EventManager>
{
    //�洢�¼����������¼��� = ����ί��
    private Dictionary<string, IEventInfo> events = new Dictionary<string, IEventInfo>();


    #region ����¼�����
    //����¼�����
    public void AddEventListener<T>(string name,UnityAction<T> action)
    {

        if (events.ContainsKey(name))
        {
            //���ڶ�Ӧ�¼�
            (events[name] as EventInfo<T>).actions += action;
        }
        else
        {
            //�����ڶ�Ӧ�¼�
            events.Add(name, new EventInfo<T>(action));
        }
    }

    public void AddEventListener(string name, UnityAction action)
    {

        if (events.ContainsKey(name))
        {
            //���ڶ�Ӧ�¼�
            (events[name] as EventInfo).actions += action;
        }
        else
        {
            //�����ڶ�Ӧ�¼�
            events.Add(name, new EventInfo(action));
        }
    }
    #endregion


    #region �¼�����
    //�¼�����,���ܴ��ݲ���
    public void EventTrigger<T>(string name,T param)
    {
        if (events.ContainsKey(name))
        {
            //�����ڶ�Ӧ�¼�ʱ
            (events[name] as EventInfo<T>).actions?.Invoke(param);
        }
    }

    public void EventTrigger(string name)
    {
        if (events.ContainsKey(name))
        {
            //�����ڶ�Ӧ�¼�ʱ
            (events[name] as EventInfo).actions?.Invoke();
        }
    }

    #endregion


    #region �Ƴ��¼�����
    //�¼������мӾ��м�
    //�Ƴ��¼�����
    public void RemoveEventListener<T>(string name,UnityAction<T> action)
    {
        if (events.ContainsKey(name))
        {
            (events[name] as EventInfo<T>).actions -= action;
        }
    }

    public void RemoveEventListener(string name, UnityAction action)
    {
        if (events.ContainsKey(name))
        {
            (events[name] as EventInfo).actions -= action;
        }
    }

    #endregion

    //����¼�����
    //��Ҫ���ڳ����л�
    public void ClearAll()
    {
        events.Clear();
    }
}

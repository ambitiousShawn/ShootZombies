using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    //����Ƿ���
    private bool isOpen = true;

    public InputManager()
    {
        //���Update�ļ���
        MonoManager.Instance.AddUpdateListener(InputUpdate);
    }

    private void InputUpdate()
    {
        if (!isOpen) 
            return;

        //����������
        CheckKey(KeyCode.LeftShift);
        CheckKey(KeyCode.LeftControl);
        //���������
        CheckMouse(0);
        CheckMouse(1);
    }

    //�������
    private void CheckKey(KeyCode keycode)
    {
        
        if (Input.GetKeyDown(keycode))
            EventManager.Instance.EventTrigger<KeyCode>("KeyDown", keycode);
        if (Input.GetKeyUp(keycode))
            EventManager.Instance.EventTrigger<KeyCode>("KeyUp", keycode);
        if (Input.GetKey(keycode))
            EventManager.Instance.EventTrigger<KeyCode>("Key", keycode);
    }

    //������
    private void CheckMouse(int mouseEvent)
    {
        if (Input.GetMouseButtonDown(mouseEvent))
            EventManager.Instance.EventTrigger<int>("MouseDown", mouseEvent);
        if (Input.GetMouseButtonUp(mouseEvent))
            EventManager.Instance.EventTrigger<int>("MouseUp", mouseEvent);
        if (Input.GetMouseButton(mouseEvent))
            EventManager.Instance.EventTrigger<int>("Mouse", mouseEvent);
    }

    //�Ƿ���������
    public void SwitchState(bool isOpen)
    {
        this.isOpen = isOpen;
    }
}

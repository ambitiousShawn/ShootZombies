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
        CheckKey(KeyCode.T);
    }

    private void CheckKey(KeyCode keycode)
    {
        if (Input.GetKeyDown(keycode))
            EventManager.Instance.EventTrigger("KeyDown", keycode);
        if (Input.GetKeyUp(keycode))
            EventManager.Instance.EventTrigger("KeyUp", keycode);
        if (Input.GetKey(keycode))
            EventManager.Instance.EventTrigger("Key", keycode);
    }

    //�Ƿ���������
    public void SwitchState(bool isOpen)
    {
        this.isOpen = isOpen;
    }
}

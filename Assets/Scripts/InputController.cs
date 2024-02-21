// Copyright 2024, Fedir Khodchenko, All rights reserved.
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour
{
    public UnityEvent OnTouchBegan;
    public UnityEvent OnTouchEnded;
    public UnityEvent OnTouchCanceled;

    private int currTouchId = -1;

    void Awake()
    {
        currTouchId = -1;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            return;
        }

        if (currTouchId < 0)
        {
            currTouchId = FindBeganTouch();
            if (CurrTouchIsValid())
                OnTouchBegan.Invoke();
            else
                currTouchId = -1;
            return;
        }

        if (!CurrTouchIsValid())
        {
            CancelTouch();
            return;
        }

        Touch touch = Input.GetTouch(currTouchId);
        switch (touch.phase)
        {
            case TouchPhase.Moved:
                break;
            case TouchPhase.Ended:
                OnTouchEnded.Invoke();
                currTouchId = -1;
                break;
            case TouchPhase.Canceled:
                CancelTouch();
                break;
            default: break;
        }
    }

    void CancelTouch()
    {
        if (currTouchId != -1)
            OnTouchCanceled.Invoke();
        currTouchId = -1;
    }

    int FindBeganTouch()
    {
        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                return i;
            }
        }
        return -1;
    }

    bool CurrTouchIsValid()
    {
        return currTouchId != -1 && !TouchOnUI(currTouchId);
    }

    static bool TouchOnUI(int touchId)
    {
        return EventSystem.current.IsPointerOverGameObject(touchId);
    }
}

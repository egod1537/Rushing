using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Platform.Controller;

public class ControllerSystem : Singletone<ControllerSystem>
{
    public class LeftSlideEvent : UnityEvent { }
    public LeftSlideEvent OnLeftSlide = new LeftSlideEvent();
    public class RightSlideEvent : UnityEvent { }
    public RightSlideEvent OnRightSlide = new RightSlideEvent();
    public class UpSlideEvent : UnityEvent { }
    public UpSlideEvent OnUpSlide = new UpSlideEvent();
    public class DownSlideEvent : UnityEvent { }
    public DownSlideEvent OnDownSlide = new DownSlideEvent();

    List<IController> controllers = new List<IController>();
    private void Awake()
    {
#if UNITY_EDITOR_WIN
        controllers.Add(new WindowsController());
#endif
#if UNITY_ANDROID
        controllers.Add(new AndroidController());
#endif
    }
    private void Update()
    {
        foreach(var con in controllers)
        {
            if (con.IsLeftSlide()) OnLeftSlide.Invoke();
            if (con.IsRightSlide()) OnRightSlide.Invoke();
            if (con.IsUpSlide()) OnUpSlide.Invoke();
            if (con.IsDownSlide()) OnDownSlide.Invoke();
        } 
    }
}

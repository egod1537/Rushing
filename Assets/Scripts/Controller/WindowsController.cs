using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Controller
{
    public class WindowsController : IController
    {
        public bool IsDownSlide()
        {
#if UNITY_EDITOR
            return Input.GetKeyDown(KeyCode.S);
#endif
            return false;
        }

        public bool IsLeftSlide()
        {
#if UNITY_EDITOR
            return Input.GetKeyDown(KeyCode.A);
#endif
            return false;
        }

        public bool IsRightSlide()
        {
#if UNITY_EDITOR
            return Input.GetKeyDown(KeyCode.D);
#endif
            return false;
        }

        public bool IsUpSlide()
        {
#if UNITY_EDITOR
            return Input.GetKeyDown(KeyCode.W);
#endif
            return false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Controller
{
    public class AndroidController : IController
    {
        public bool IsDownSlide()
        {
            if (Input.touchCount == 0) return false;
            return Input.GetTouch(0).deltaPosition.y < 0.0f;
        }

        public bool IsLeftSlide()
        {
            if (Input.touchCount == 0) return false;
            return Input.GetTouch(0).deltaPosition.x < 0.0f;
        }

        public bool IsRightSlide()
        {
            if (Input.touchCount == 0) return false;
            return Input.GetTouch(0).deltaPosition.x > 0.0f;
        }

        public bool IsUpSlide()
        {
            if (Input.touchCount == 0) return false;
            return Input.GetTouch(0).deltaPosition.y > 0.0f;
        }
    }
}

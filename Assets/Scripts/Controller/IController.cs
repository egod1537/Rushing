using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Controller
{
    public interface IController
    {
        public abstract bool IsLeftSlide();
        public abstract bool IsRightSlide();
        public abstract bool IsUpSlide();
        public abstract bool IsDownSlide();
    }
}

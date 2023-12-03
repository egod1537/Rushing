using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBtnSkillIcon : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] public Transform icon;
    [SerializeField] public float yOffset;

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 pos = icon.localPosition;
        pos.y += yOffset;
        icon.localPosition = pos;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Vector3 pos = icon.localPosition;
        pos.y -= yOffset;
        icon.localPosition = pos;
    }
}

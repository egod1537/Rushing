using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class UIInformationBar : MonoBehaviour
{
    public int _Level;
    public int Level
    {
        get => _Level;
        set {
            int input = Mathf.Max(value, 0);
            if(_Level != input)AnimationLevel(input);
            _Level = input;
        }
    }

    public int _MaxHp;
    public int MaxHp
    {
        get => _MaxHp;
        set {
            int input = Mathf.Max(value, 0);
            if(input != _MaxHp) AnimationMaxHp(input);
            _MaxHp = input; 
        }
    }

    public int _Hp;
    public int Hp
    {
        get => _Hp;
        set {
            int input = Mathf.Clamp(value, 0, MaxHp);
            if (input != _Hp) AnimationHp(input);
            _Hp = input;
        }
    }

    public int _Energy;
    public int Energy
    {
        get => _Energy;
        set {
            int input = Mathf.Clamp(value, 0, 5);
            if(input != _Energy) AnimationEnergy(input);
            _Energy = input;
        }
    }

    public TextMeshProUGUI TxtLevel;

    public Image ImgHp;
    public TextMeshProUGUI TxtHp;

    public Image ImgEnergy;

    private void AnimationLevel(int value)
    {
        TxtLevel.text = value.ToString();
    }
    private void AnimationMaxHp(int value)
    {
        TxtHp.text = $"{Hp}/{MaxHp}";
    }

    private void AnimationHp(int value)
    {
        DOTween.KillAll(ImgHp);

        TxtHp.text = $"{Hp}/{MaxHp}";

        Color col = ImgHp.color;
        DOTween.Sequence()
            .Append(ImgHp.DOFillAmount(1.0f * value / MaxHp, 0.25f))
            .Join(ImgHp.DOColor(col * 0.75f, 0.25f / 2f))
            .Append(ImgHp.DOColor(col, 0.25f / 2f));

        if (!Application.isPlaying)
            ImgHp.fillAmount = 1.0f * value / MaxHp;
    }

    private void AnimationEnergy(int value)
    {
        DOTween.KillAll(ImgEnergy);

        Color col = ImgEnergy.color;

        DOTween.Sequence()
            .Append(ImgEnergy.DOFillAmount(1.0f * value / 5f, 0.25f))
            .Join(ImgEnergy.DOColor(col * 0.75f, 0.25f / 2f))
            .Append(ImgEnergy.DOColor(col, 0.25f / 2f));

        if (!Application.isPlaying)
            ImgEnergy.fillAmount = 1.0f * value / 5f;
    }
}

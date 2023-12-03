using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInfoBar : MonoBehaviour
{
    public Image imgHpBar;
    public Image imgEnergyBar;
    PlayerModel playerModel { get { return PlayerManager.ins.player; } }

    private void Awake()
    {
        MapManager.ins.OnGenerateEntity.AddListener(() =>
        {
            playerModel.OnEditHp.AddListener(() =>
            {
                SetHp();
            });

            playerModel.OnEditEnergy.AddListener(() =>
            {
                SetEnergy();
            });
        });
    }

    private void Update()
    {
        SetHp();
        SetEnergy();
    }
    public void SetHp()
    {
        imgHpBar.fillAmount = 1.0f*playerModel.hp / playerModel.maxHp;
    }
    public void SetEnergy()
    {
        imgEnergyBar.fillAmount = 1.0f*playerModel.energy / playerModel.maxEnergy;
    }
}

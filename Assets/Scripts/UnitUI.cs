using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitUI : MonoBehaviour
{
    public Unit unit;

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI atkSpeedText;
    [SerializeField] private TextMeshProUGUI rangeText;

    private void Update()
    {
        healthText.text = string.Format("Health: {0} / {1}", unit.CurrentHP, unit.HP);
        damageText.text = string.Format("Damage: {0}", unit.ATK);
        speedText.text = string.Format("Unit Speed: {0}", unit.MovSPD);
        atkSpeedText.text = string.Format("Unit AtkSpeed: {0}", unit.AtkSPD);
        rangeText.text = string.Format("Unit Range: {0}", unit.Range);
    }
}

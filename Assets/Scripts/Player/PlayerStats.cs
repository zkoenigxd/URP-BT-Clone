using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.Port;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] Slider powerSlider;
    [SerializeField] Slider shieldSlider;
    [SerializeField] Slider cargoSlider;
    [SerializeField] Slider healthSlider;

    float totalPower;
    float currentPower;
    float totalShields;
    float currentShields;
    float totalCargo;
    float currentCargo;
    float totalHealth;
    float currentHealth;

    public void AddPower(float power) { totalPower += power; }
    public void AddShield(float shield) {  totalShields += shield; }
    public void AddCargo(float cargo) {  totalCargo += cargo; }
    public void AddHealth(float health) {  totalHealth += health; }

    public void RemovePower(float power) { totalPower -= power; }
    public void RemoveShield(float shield) { totalShields -= shield; }
    public void RemoveCargo(float cargo) { totalCargo -= cargo; }
    public void RemoveHealth(float health) { totalHealth -= health; }

    public void UpdateCurrentShield(float change) { currentShields += change; UpdateShieldVisuals();}
    public void UpdateCurrentCargo(float change) { currentCargo += change;  UpdateCargoVisuals(); }
    public void UpdateCurrentPower(float change) { currentPower += change; UpdatePowerVisuals(); }
    public void UpdateHealth(float change) {  totalHealth += change; }

    public void ResetTotalValues()
    {
        totalPower = 0;
        totalShields = 0;
        totalCargo = 0;
        totalHealth = 0;
    }

    public void InitializeUI()
    {
        SetUpShieldBar();
        SetUpCargoBar();
        SetUpPowerBar();
    }

    void SetUpShieldBar()
    {
        RectTransform rectTransform = shieldSlider.GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 10 * (totalShields + .0000001f));
    }

    void SetUpCargoBar()
    {
        RectTransform rectTransform = cargoSlider.GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 10 * totalCargo);
    }

    void SetUpPowerBar()
    {
        RectTransform rectTransform = powerSlider.GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 10 * totalPower);
    }

    void UpdateShieldVisuals()
    {
        shieldSlider.value = currentShields / (totalShields + .0000001f);
    }

    void UpdateCargoVisuals()
    {
        if (cargoSlider != null)
            cargoSlider.value = currentCargo / ((float)totalCargo + .0000001f);
    }

    void UpdatePowerVisuals()
    {
        if (powerSlider != null)
        {
            powerSlider.value = currentPower / ((float)totalPower + .00000001f);
        }
    }
}

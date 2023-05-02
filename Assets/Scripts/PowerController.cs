using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PowerController : MonoBehaviour
{
    [SerializeField] Slider powerSlider;
    [SerializeField] int capacity;
    [SerializeField] float rechargeRate;
    [SerializeField] float percentPowerToCharge;

    float currentPower;

    public float ChargePercent => currentPower / capacity;

    private void Awake()
    {
        currentPower = capacity;
        if(powerSlider != null)
        {
            SetUpPowerBar();
            UpdateVisuals();
        }
    }

    private void Update()
    {
        Recharge();
    }

    public bool ConsumePower(float powerCost)
    {
        if (currentPower < powerCost)
        {
            return false;
        }
        currentPower -= powerCost;
        UpdateVisuals();
        return true;
    }

    void Recharge()
    {
        if (currentPower < capacity)
            currentPower += rechargeRate * Time.deltaTime;
        UpdateVisuals();
    }

    void SetUpPowerBar()
    {
        RectTransform rectTransform = powerSlider.GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 10 * capacity);
    }

    void UpdateVisuals()
    {
        if (powerSlider != null)
        {
            powerSlider.value = currentPower / (float)capacity;
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PowerController : MonoBehaviour
{
    [SerializeField] Slider powerSlider;
    [SerializeField] PowerUSO powerType;

    int capacity;
    float rechargeRate;

    float currentPower;

    public float ChargePercent => currentPower / (capacity + .0000001f);
    public PowerUSO PowerType => powerType;

    private void Awake()
    {
        capacity = powerType.Capacity;
        rechargeRate = powerType.RechargeRate;
        currentPower = capacity;
        if (powerSlider != null)
        {
            SetUpPowerBar();
            UpdateVisuals();
        }
    }

    public void InstallComponent(PowerUSO powerUSO)
    {
        powerType = powerUSO;
        capacity = powerType.Capacity;
        rechargeRate = powerType.RechargeRate;
        currentPower = capacity;
        SetUpPowerBar();
        UpdateVisuals();
    }

    public void RemoveComponent()
    {
        powerType = null;
        capacity = 0;
        rechargeRate = 0;
        currentPower = 0;
        SetUpPowerBar();
        UpdateVisuals();
    }

    private void Update()
    {
        if (capacity > 0)
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
            powerSlider.value = currentPower / ((float)capacity + .00000001f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldController : MonoBehaviour
{
    [SerializeField] ShieldUSO shieldType;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Slider shieldSlider;
    [SerializeField] Slider beginChargeUISlider;

    PowerController powerController;
    Collider2D shieldCollider;
    Color shieldColor;
    float shieldCapacity;
    float damageReduction;
    float rechargeRate;
    float rechargeDelay;
    float powerConsumptionRate;
    float beginChargePercent;
    float currentShield;
    bool endDelay;

    public ShieldUSO ShieldType => shieldType;

    private void Awake()
    {
        shieldCapacity = shieldType.ShieldCapacity;
        damageReduction = shieldType.DamageReduction;
        rechargeRate = shieldType.RechargeRate;
        rechargeDelay = shieldType.RechargeDelay;
        powerConsumptionRate = shieldType.PowerConsumptionRate;
        beginChargePercent = shieldType.BeginChargePercent;
        shieldColor = shieldType.ShieldColor;

        currentShield = shieldCapacity;

        powerController = GetComponentInParent<PowerController>();
        shieldCollider = GetComponent<Collider2D>();

        SetUpShieldBar();
        UpdateShieldVisuals();

        beginChargeUISlider.value = beginChargePercent;
    }

    public void InstallComponent(ShieldUSO shieldUSO)
    {
        shieldType = shieldUSO;
        shieldCapacity = shieldType.ShieldCapacity;
        damageReduction = shieldType.DamageReduction;
        rechargeRate = shieldType.RechargeRate;
        rechargeDelay = shieldType.RechargeDelay;
        powerConsumptionRate = shieldType.PowerConsumptionRate;
        beginChargePercent = shieldType.BeginChargePercent;
        shieldColor = shieldType.ShieldColor;

        currentShield = shieldCapacity;

        SetUpShieldBar();
        UpdateShieldVisuals();
        Debug.Log(shieldUSO.ToString() + " Charged to: "+ currentShield);
        beginChargeUISlider.value = beginChargePercent;
    }

    public void RemoveComponent()
    {
        shieldType = null;
        shieldCapacity = 0;
        damageReduction = 0;
        rechargeRate = 0;
        rechargeDelay = 0;
        powerConsumptionRate = 0;
        beginChargePercent = 0;
        shieldColor = Color.white;

        currentShield = shieldCapacity;
        SetUpShieldBar();
        UpdateShieldVisuals();
        beginChargeUISlider.value = beginChargePercent;
    }

    private void Update()
    {
        if (endDelay && powerController.ChargePercent > beginChargePercent)
            Recharge();
    }

    public void WeakenSheild(float incomingDamage)
    {
        currentShield -= incomingDamage;
        endDelay = false;
        StopAllCoroutines();
        StartCoroutine(DelayRecharge());
        if (currentShield <= 0)
        {
            shieldCollider.enabled = false;
            spriteRenderer.enabled = false;
        }
        UpdateShieldVisuals();
    }

    public float ReduceDamage(float incomingDamage)
    {
        incomingDamage *= damageReduction;
        return incomingDamage;
    }

    void Recharge()
    {
        if(currentShield < shieldCapacity && powerController.ConsumePower(powerConsumptionRate * Time.deltaTime))
        {
                currentShield += rechargeRate * Time.deltaTime;
        }
        shieldCollider.enabled = true;
        spriteRenderer.enabled = true;
        UpdateShieldVisuals();
    }

    void SetUpShieldBar()
    {
        RectTransform rectTransform = shieldSlider.GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 10 * (shieldCapacity+.0000001f));
    }

    void UpdateShieldVisuals()
    {
        shieldSlider.value = currentShield / (shieldCapacity + .0000001f);
        spriteRenderer.color = new Color(shieldColor.r, shieldColor.g, shieldColor.b, currentShield / (shieldCapacity + .0000001f));
    }

    IEnumerator DelayRecharge()
    {
        yield return new WaitForSeconds(rechargeDelay);
        endDelay = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldController : Controller
{
    [SerializeField] ShieldUSO shieldType;
    [SerializeField] SpriteRenderer spriteRenderer;
    //[SerializeField] Slider shieldSlider;
    //[SerializeField] Slider beginChargeUISlider;
    [SerializeField] Collider2D shieldCollider;

    PlayerStats playerStats;
    PowerController powerController;
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
        playerStats = GetComponentInParent<PlayerStats>();
        shieldCapacity = shieldType.ShieldCapacity;
        damageReduction = shieldType.DamageReduction;
        rechargeRate = shieldType.RechargeRate;
        rechargeDelay = shieldType.RechargeDelay;
        powerConsumptionRate = shieldType.PowerConsumptionRate;
        beginChargePercent = shieldType.BeginChargePercent;
        shieldColor = shieldType.ShieldColor;

        currentShield = shieldCapacity;


        powerController = GetComponent<PowerController>();
        playerStats.AddShield(shieldCapacity);
        playerStats.UpdateCurrentShield(shieldCapacity);
        //beginChargeUISlider.value = beginChargePercent;
    }

    public override void InstallComponent(UpgradeSO upgrade)
    {
        shieldType = (ShieldUSO)upgrade;
        shieldCapacity = shieldType.ShieldCapacity;
        damageReduction = shieldType.DamageReduction;
        rechargeRate = shieldType.RechargeRate;
        rechargeDelay = shieldType.RechargeDelay;
        powerConsumptionRate = shieldType.PowerConsumptionRate;
        beginChargePercent = shieldType.BeginChargePercent;
        shieldColor = shieldType.ShieldColor;

        currentShield = shieldCapacity;

        playerStats.AddShield(shieldCapacity);
        //beginChargeUISlider.value = beginChargePercent;
    }

    public override void RemoveComponent()
    {
        playerStats.RemoveShield(shieldCapacity);
        shieldType = null;
        shieldCapacity = 0;
        damageReduction = 0;
        rechargeRate = 0;
        rechargeDelay = 0;
        powerConsumptionRate = 0;
        beginChargePercent = 0;
        shieldColor = Color.white;

        currentShield = shieldCapacity;

        //beginChargeUISlider.value = beginChargePercent;
    }

    public override bool IsAvailable()
    {
        return shieldType == null;
    }
    public override UpgradeSO GetUpgrade()
    {
        return shieldType;
    }

    public override UpgradeType GetSlotType()
    {
        return UpgradeType.Shield;
    }

    private void Update()
    {
        if (endDelay && powerController.ChargePercent > beginChargePercent)
            Recharge();
    }

    public void WeakenSheild(float incomingDamage)
    {
        if (GetComponentInParent<Player>() != null)
        {
            GetComponentInParent<Player>().PlayShieldHitAnim();
        }
        currentShield -= incomingDamage;
        endDelay = false;
        StopAllCoroutines();
        StartCoroutine(DelayRecharge());
        if (currentShield <= 0)
        {
            if (GetComponentInParent<Player>() != null)
            {
                GetComponentInParent<Player>().PlayShieldDestroyedAnim();
            }
            shieldCollider.enabled = false;
            spriteRenderer.enabled = false;
        }
        playerStats.UpdateCurrentShield(-incomingDamage);
        spriteRenderer.color = new Color(shieldColor.r, shieldColor.g, shieldColor.b, currentShield / (shieldCapacity + .0000001f));
    }

    public float ReduceDamage(float incomingDamage)
    {
        incomingDamage *= damageReduction;
        return incomingDamage;
    }

    void Recharge()
    {
        float recharge = rechargeRate * Time.deltaTime;
        if (currentShield < shieldCapacity && powerController.ConsumePower(powerConsumptionRate * Time.deltaTime))
        {
            currentShield += recharge;
        }
        shieldCollider.enabled = true;
        spriteRenderer.enabled = true;
        playerStats.UpdateCurrentShield(recharge);
        spriteRenderer.color = new Color(shieldColor.r, shieldColor.g, shieldColor.b, currentShield / (shieldCapacity + .0000001f));
    }

    IEnumerator DelayRecharge()
    {
        yield return new WaitForSeconds(rechargeDelay);
        endDelay = true;
    }
}

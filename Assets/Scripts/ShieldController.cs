using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldController : MonoBehaviour
{
    [SerializeField] float shieldCapacity;
    [Tooltip("Percent reduction of incoming damage.")]
    [SerializeField] float damageReduction;
    [SerializeField] float rechargeRate;
    [SerializeField] float rechargeDelay;
    [Tooltip("Amount of power drained for each point of shield recharged.")]
    [SerializeField] float powerConsumptionRate;
    [SerializeField] float beginChargePercent;
    [SerializeField] Slider shieldSlider;
    [SerializeField] Slider beginChargeUISlider;
    [SerializeField] Color shieldColor;

    PowerController powerController;
    SpriteRenderer spriteRenderer;
    Collider2D shieldCollider;
    float currentShield;
    bool endDelay;

    private void Awake()
    {
        powerController = GetComponentInParent<PowerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentShield = shieldCapacity;
        shieldCollider = GetComponent<Collider2D>();
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
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 10 * shieldCapacity);
    }

    void UpdateShieldVisuals()
    {
        shieldSlider.value = currentShield / shieldCapacity;
        spriteRenderer.color = new Color(shieldColor.r, shieldColor.g, shieldColor.b, currentShield / shieldCapacity);
    }

    IEnumerator DelayRecharge()
    {
        yield return new WaitForSeconds(rechargeDelay);
        endDelay = true;
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PowerController : Controller
{
    [SerializeField] PowerUSO powerType;

    int capacity;
    float rechargeRate;

    float currentPower;
    PlayerStats playerStats;
    bool updatePlayerStats = false;

    public float ChargePercent => currentPower / (capacity + .0000001f);
    public PowerUSO PowerType => powerType;

    private void Awake()
    {
        capacity = powerType.Capacity;
        rechargeRate = powerType.RechargeRate;
        currentPower = capacity;
        if(GetComponentInParent<Player>())
        {
            updatePlayerStats = true;
            playerStats = GetComponentInParent<PlayerStats>();
            playerStats.AddPower(capacity);
            playerStats.UpdateCurrentPower(capacity);
        }
    }

    public override void InstallComponent(UpgradeSO upgrade)
    {
        powerType = (PowerUSO)upgrade;
        capacity = powerType.Capacity;
        rechargeRate = powerType.RechargeRate;
        currentPower = capacity;
        playerStats.AddPower(capacity);
    }

    public override void RemoveComponent()
    {
        playerStats.RemovePower(capacity);
        powerType = null;
        capacity = 0;
        rechargeRate = 0;
        currentPower = 0;
    }

    public override bool IsAvailable()
    {
        return powerType == null;
    }

    public override UpgradeSO GetUpgrade()
    {
        return powerType;
    }

    public override UpgradeType GetSlotType()
    {
        return UpgradeType.Power;
    }

    private void Update()
    {
        if (capacity - currentPower > 0)
            Recharge();
    }

    public bool ConsumePower(float powerCost)
    {
        if (currentPower < powerCost)
        {
            return false;
        }
        currentPower -= powerCost;
        if(updatePlayerStats)
            playerStats.UpdateCurrentPower(-powerCost);
        return true;
    }

    void Recharge()
    {
        float recharge = rechargeRate * Time.deltaTime;
        if (currentPower < capacity)
        {
            currentPower += recharge;
            if(updatePlayerStats)
                playerStats.UpdateCurrentPower(recharge);
        }

    }
}

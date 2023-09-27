using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CargoController : Controller
{
    [SerializeField] CargoUSO cargoType;
    int capacity;
    // condisder increasing the "inertia" of ships with increased cargo capacity

    int scrapCollected;
    int rareScrapCollected;
    List<Scrap> rareScrapItems;
    PlayerStats playerStats;

    public CargoUSO CargoType => cargoType;
    public int CurrentCurrency => scrapCollected;

    private void Awake()
    {
        playerStats = GetComponentInParent<PlayerStats>();
        capacity = cargoType.CargoCapacity;
        scrapCollected = 0;
        playerStats.AddCargo(capacity);
        playerStats.UpdateCurrentCargo(scrapCollected);
    }

    public override void InstallComponent(UpgradeSO upgrade)
    {
        cargoType = (CargoUSO)upgrade;
        capacity = cargoType.CargoCapacity;
        scrapCollected = 0;
        playerStats.AddCargo(capacity);
    }

    public override void RemoveComponent()
    {
        cargoType = null;
        capacity = 0;
        scrapCollected = 0;
        playerStats.RemoveCargo(capacity);
    }

    public override UpgradeSO GetUpgrade()
    {
        return cargoType;
    }

    public override UpgradeType GetSlotType()
    {
        return UpgradeType.Cargo;
    }

    public override bool IsAvailable()
    {
        return cargoType == null;
    }

    public bool CollectScrap(Scrap scrap, int value)
    {
        if (!scrap.IsRare)
        {
            if (scrapCollected + value > capacity - rareScrapCollected)
                return false;
            scrapCollected += value;
            playerStats.UpdateCurrentCargo(value);
            return true;
        }
        if (scrapCollected + rareScrapCollected + value > capacity)
        {
            return false;
        }
        rareScrapItems.Add(scrap);
        rareScrapCollected += value;
        playerStats.UpdateCurrentCargo(value);
        return true;
    }

    public void DepositCurrency()
    {
        Debug.Log("Got this far");
        playerStats.UpdateCurrentCargo(-scrapCollected);
        scrapCollected = 0;
    }
}

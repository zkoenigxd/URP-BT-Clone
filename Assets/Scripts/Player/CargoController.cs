using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CargoController : Controller
{
    [SerializeField] Slider scrapSlider;
    [SerializeField] CargoUSO cargoType;
    int capacity;
    // condisder increasing the "inertia" of ships with increased cargo capacity

    int scrapCollected;
    int rareScrapCollected;
    List<Scrap> rareScrapItems;


    public CargoUSO CargoType => cargoType;
    public int CurrentCurrency => scrapCollected;

    private void Awake()
    {
        capacity = cargoType.CargoCapacity;
        scrapCollected = 0;
        SetUpScrapdBar();
        UpdateVisuals();
    }

    public override void InstallComponent(UpgradeSO upgrade)
    {
        cargoType = (CargoUSO)upgrade;
        capacity = cargoType.CargoCapacity;
        scrapCollected = 0;
        SetUpScrapdBar();
        UpdateVisuals();
    }

    public override void RemoveComponent()
    {
        cargoType = null;
        capacity = 0;
        scrapCollected = 0;
        SetUpScrapdBar();
        UpdateVisuals();
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
            UpdateVisuals();
            return true;
        }
        if (scrapCollected + rareScrapCollected + value > capacity)
        {
            return false;
        }
        rareScrapItems.Add(scrap);
        rareScrapCollected += value;
        UpdateVisuals();
        return true;
    }

    public void DepositCurrency()
    {
        scrapCollected = 0;
        UpdateVisuals();
    }

    void SetUpScrapdBar()
    {
        RectTransform rectTransform = scrapSlider.GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 10 * capacity);
    }

    void UpdateVisuals()
    {
        scrapSlider.value = scrapCollected / ((float)capacity + .0000001f);
    }
}

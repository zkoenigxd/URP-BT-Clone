using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrecyController : MonoBehaviour
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

    public void InstallComponent(CargoUSO cargoUSO)
    {
        cargoType = cargoUSO;
        capacity = cargoType.CargoCapacity;
        scrapCollected = 0;
        SetUpScrapdBar();
        UpdateVisuals();
    }

    public void RemoveComponent()
    {
        cargoType = null;
        capacity = 0;
        scrapCollected = 0;
        SetUpScrapdBar();
        UpdateVisuals();
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

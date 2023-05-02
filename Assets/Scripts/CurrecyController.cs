using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrecyController : MonoBehaviour
{
    [SerializeField] Slider scrapSlider;
    [SerializeField] int capacity;


    GameManager gameManager;
    int scrapCollected;
    int rareScrapCollected;
    List<Scrap> rareScrapItems;

    public int CurrentCurrency => scrapCollected;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
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

    void SetUpScrapdBar()
    {
        RectTransform rectTransform = scrapSlider.GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 10 * capacity);
    }

    void UpdateVisuals()
    {
        scrapSlider.value = scrapCollected / (float)capacity;
    }
}

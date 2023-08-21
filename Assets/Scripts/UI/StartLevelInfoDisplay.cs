using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartLevelInfoDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text factionNameText;
    [SerializeField] TMP_Text speciesText;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform techUIGroup;
    [SerializeField] Transform shipUIGroup;
    [SerializeField] Transform gravityWellUIGroup;
    [SerializeField] Transform sectorUIGroup;
    [SerializeField] TMP_Text unlockRequirements;


    public void UpdateInfo(FactionSO factionInfo, bool unlocked)
    {
        factionNameText.text = "<b>" + factionInfo.factionName + "</b>\n";
        speciesText.text = "<i>" + factionInfo.species + "</i>";

        foreach (Transform child in techUIGroup.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in shipUIGroup.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in gravityWellUIGroup.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in sectorUIGroup.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (string item in factionInfo.techSpecialties)
        {
            GameObject displayItem = Instantiate(itemPrefab);
            displayItem.transform.SetParent(techUIGroup, false);
            displayItem.GetComponent<TMP_Text>().text = item;
        }
        foreach (string item in factionInfo.shipSpecialties)
        {
            GameObject displayItem = Instantiate(itemPrefab);
            displayItem.transform.SetParent(shipUIGroup, false);
            displayItem.GetComponent<TMP_Text>().text = item;
        }
        foreach (string item in factionInfo.GravityWellAttributes)
        {
            GameObject displayItem = Instantiate(itemPrefab);
            displayItem.transform.SetParent(gravityWellUIGroup, false);
            displayItem.GetComponent<TMP_Text>().text = item;
        }
        foreach (string item in factionInfo.SectorAttributes)
        {
            GameObject displayItem = Instantiate(itemPrefab);
            displayItem.transform.SetParent(sectorUIGroup, false);
            displayItem.GetComponent<TMP_Text>().text = item;
        }
        if (!unlocked)
        {
            unlockRequirements.gameObject.SetActive(true);
            unlockRequirements.text = "Beat the game while this faction's spacedock is clear to unlock this starting position.";
        }
        else
        {
            unlockRequirements.gameObject.SetActive(false);
        }
    }

    public void UpdateInfo()
    {
        foreach (Transform child in techUIGroup.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in shipUIGroup.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in gravityWellUIGroup.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in sectorUIGroup.transform)
        {
            Destroy(child.gameObject);
        }
        factionNameText.text = "<b>Undiscovered</b>\n";
        speciesText.text = "<i>???</i>";
        string item = "???";
        GameObject techDisplayItem = Instantiate(itemPrefab);
        techDisplayItem.transform.SetParent(techUIGroup, false);
        techDisplayItem.GetComponent<TMP_Text>().text = item;
        GameObject shipDisplayItem = Instantiate(itemPrefab);
        shipDisplayItem.transform.SetParent(shipUIGroup, false);
        shipDisplayItem.GetComponent<TMP_Text>().text = item;
        GameObject gwDisplayItem = Instantiate(itemPrefab);
        gwDisplayItem.transform.SetParent(gravityWellUIGroup, false);
        gwDisplayItem.GetComponent<TMP_Text>().text = item;
        GameObject sectorDisplayItem = Instantiate(itemPrefab);
        sectorDisplayItem.transform.SetParent(sectorUIGroup, false);
        sectorDisplayItem.GetComponent<TMP_Text>().text = item;
        //techSpecialtyText.text = "<align=left>Tech Specialty:<line-height=0>\r\n<align=right>???";
        //shipSpecialtyText.text = "<align=left>Ship Specialty:<line-height=0>\r\n<align=right>???";
        //sectorTypeText.text = "<align=left>Sector Type:<line-height=0>\r\n<align=right>???";
        unlockRequirements.gameObject.SetActive(true);
        unlockRequirements.text = "Discover this faction to reveal its unlock requirements.";
    }
}

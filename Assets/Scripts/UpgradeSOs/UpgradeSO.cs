using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeType { Weapon, Cargo, Power, Shield, Magnet }
public abstract class UpgradeSO : ScriptableObject
{
    [SerializeField] string upgradeName;
    [TextArea(15, 20)]
    [SerializeField] string description;
    [SerializeField] int level;
    [SerializeField] int buyCost;
    [SerializeField] int sellCost;
    [SerializeField] Sprite storeIcon;
    [SerializeField] Faction faction;
    protected UpgradeType upgradeType;
    bool unlocked, canAfford;

    public Faction UpgradeFaction => faction;
    public UpgradeType UpgradeType => upgradeType;
    public int BuyCost => buyCost;
    public int SellCost => sellCost;
    public int Level => level;
    public string Name => upgradeName;
    public Sprite StoreIcon => storeIcon;
    public string Description => description;


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    public abstract void InstallComponent(UpgradeSO upgrade);
    public abstract void RemoveComponent();
    public abstract bool IsAvailable();
    public abstract UpgradeSO GetUpgrade();
    public abstract UpgradeType GetSlotType();
}

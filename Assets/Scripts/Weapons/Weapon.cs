using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public abstract void OperateWeapon(Vector3 fireDirection);

    public abstract void FireWeapon(Vector3 fireDirection);

    public abstract void FireWeapon(RaycastHit2D hit, float distance);
}
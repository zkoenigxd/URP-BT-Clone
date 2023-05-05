using System.Collections.Generic;
using UnityEngine;

public class UnitManager2 : MonoBehaviour
{
    UIManager uIManager;
    ArenaManager arena;
    Rigidbody2D unitRB;
    //Weapon[] attachedWeapons;
    Vector3 moveInput;
    Vector3 fireDirection;
    Vector2 fireInput;
    Vector2 boostInput;
    float speedModifier;
    bool canFire;

    [SerializeField] float unitSpeed;
    [SerializeField] float boostScalar;
    [SerializeField] GameObject engineAnimations;
    [SerializeField] AudioSource engineSound;

    public Vector3 FireDirection => fireDirection;
    public Vector2 FireInput => fireInput;
    public Vector2 MoveInput => moveInput;
    public Vector2 BoostInput => boostInput;
    public float SpeedModifier => speedModifier;
    public bool CanFire => canFire;
    public float UnitSpeed => unitSpeed;

    private void Awake()
    {
        uIManager = FindObjectOfType<UIManager>();
        arena = FindObjectOfType<ArenaManager>();
        if (!arena)
            Debug.LogWarning("ArenaManager not found");
        unitRB = GetComponent<Rigidbody2D>();
        //attachedWeapons = GetComponentsInChildren<Weapon>();
        //Debug.Log(attachedWeapons.ToString());
        //Boost();

    }

    private void FixedUpdate()
    {
        if (arena.IsPlayerInZone)
        {
            speedModifier = 1.0f;
            unitRB.drag = 1;
            canFire = true;
        }
        else
        {
            unitRB.drag = arena.DragMultiplier;
            speedModifier = 2.0f;
            canFire = false;
        }

        //foreach (Weapon weapon in attachedWeapons)
        //{
        //    if (weapon != null)
        //        weapon.OperateWeapon(fireInput);
        //}
        Move();
        TurnShip();
    }

    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    public void SetFireInput(Vector2 input)
    {
        fireInput = input;
    }

    public void SetBoostInpust(Vector2 input)
    {
        boostInput = input;
    }

    private void TurnShip()
    {
        if (moveInput != Vector3.zero)
        {
            float angle = Mathf.Atan2(-moveInput.x, moveInput.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), .1f);
        }
    }

    private void Move()
    {
        unitRB.AddForce(speedModifier * unitSpeed * moveInput);
        if(engineAnimations != null && engineSound != null)
        {
            if (moveInput == Vector3.zero)
            {
                engineAnimations.SetActive(false);
                engineSound.Stop();
            }
            else
            {
                engineAnimations.SetActive(true);
                if(!engineSound.isPlaying)
                    engineSound.Play();
            }
        }
    }

    public void Boost()
    {
        //Debug.Log("Boost Activated with force: " + boostInput);
        unitRB.AddForce(boostInput / boostScalar, ForceMode2D.Impulse);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        arena.RemoveUnit(this.GetComponent<EnemyBrain>());
        Debug.Log("Removed " + name + " from unit list");
        if (GetComponent<Player>() != null)
        {
            uIManager.DisplayRestartButton();
            return;
        }
        if (arena.CheckAllEnemiesDefeated())
        {
            Debug.Log("Level Cleared");
            //uIManager.DisplayWinButton();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    WeaponController[] attachedControllers;
    GameObject player;
    Rigidbody2D enemyRB;
    ArenaManager arena;
    UnitManager2 unitManager;
    Vector3 startPosition;
    Vector2 playerDirection;
    bool inWeapon1Range;
    bool playerFound, tooClose, inPatrolZone, braking;
    bool isInArena;

    [SerializeField] int engagementDistance, standoffDistance, patrolZoneRadius, weapon1Range;
    [SerializeField] int reloadTime1;
    [SerializeField] int recoil1;
    [SerializeField] float maxSpeed = 10;

    private void Awake()
    {
        Physics2D.IgnoreLayerCollision(7, 11);
    }

    void Start()
    {
        attachedControllers = GetComponentsInChildren<WeaponController>();
        unitManager = GetComponent<UnitManager2>();
        arena = FindObjectOfType<ArenaManager>();
        if (!arena)
            Debug.LogWarning("ArenaManager not found");
        startPosition = transform.position;
        enemyRB = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>().gameObject;
        if (standoffDistance >= engagementDistance)
        {
            standoffDistance = engagementDistance - 5;
        }
        if (standoffDistance < 0)
        {
            standoffDistance = 0;
        }
    }

    void Update()
    {
        if (player != null)
        {
            playerDirection = player.transform.position - transform.position;
            if (playerDirection.magnitude < engagementDistance)
            {
                if (playerDirection.magnitude < weapon1Range && playerDirection.magnitude > standoffDistance)
                    inWeapon1Range = true;
                playerFound = true;
                tooClose = false;
            }
            else if (playerDirection.magnitude < weapon1Range && playerDirection.magnitude <= standoffDistance)
            {
                if (playerDirection.magnitude < weapon1Range && playerDirection.magnitude > standoffDistance)
                    inWeapon1Range = true;
                playerFound = true;
                tooClose = true;
            }
            else
            {
                playerFound = false;
                inWeapon1Range = false;
            }
        }
        else
        {
            playerFound = false;
            inWeapon1Range = false;
            tooClose = false;
        }
        if ((startPosition - transform.position).magnitude > patrolZoneRadius)
            inPatrolZone = false;
        else
            inPatrolZone = true;
        if (playerFound && !tooClose)
        {
            unitManager.SetMoveInput(playerDirection.normalized);
            if (inWeapon1Range)
            {
                foreach (var controller in attachedControllers)
                {
                    if(controller != null)
                        controller.StartRepeatFire();
                }
            }
            else foreach (var controller in attachedControllers) controller.StopRepeatFire();
        }
        else if (playerFound && tooClose && !braking)
        {
            if (inWeapon1Range) foreach (var controller in attachedControllers) controller.StartRepeatFire();
            else foreach (var controller in attachedControllers) controller.StopRepeatFire();
            HitBrakes();
        }
        else
        {
            Patrol();
        }

        if (!isInArena)
            PushIntoArena();
    }

    private void HitBrakes()
    {
        braking = true;
        if (enemyRB.velocity.magnitude > .1f)
        {
            unitManager.SetMoveInput(-enemyRB.velocity.normalized);
        }
        else
        {
            braking = false;
        }
    }

    void PushIntoArena()
    {
        unitManager.SetMoveInput(transform.position.normalized * -2);
    }

    void Patrol()
    {
        if (inPatrolZone)
        {
            if (enemyRB.velocity.magnitude < maxSpeed && !braking)
            {
                Vector2 OrthoganalClockwise = new(transform.position.y, -transform.position.x);
                unitManager.SetMoveInput(-OrthoganalClockwise.normalized);
            }
            else
            {
                HitBrakes();
            }
        }
        else
        {
            if (enemyRB.velocity.magnitude < maxSpeed / 2 && !braking)
            {
                Vector2 zoneDirection = startPosition - transform.position;
                unitManager.SetMoveInput(-zoneDirection.normalized);
            }
            else
            {
                HitBrakes();
            }
        }
    }

    public void LeftArena()
    {
        isInArena = false;
    }

    public void ReturnedToArena()
    {
        isInArena = true;
    }
}

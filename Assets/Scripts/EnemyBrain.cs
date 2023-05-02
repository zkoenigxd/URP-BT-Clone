using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyBrain : MonoBehaviour
{
    WeaponController[] attachedControllers;
    Transform playerTransform;
    Vector2 playerDirection;
    //ArenaManager arena;
    //UnitManager2 unitManager;
    Vector3 startPosition;
    //bool inWeapon1Range;
    bool playerFound, tooClose, inPatrolZone, attacking;
    //bool isInArena;

    bool onPatrol, inChase;

    //Path path;
    //int currentWaypoint = 0;
    //bool reachedEndOfPath;

    AIDestinationSetter destinationSetter;
    Patrol patrolController;


    //Seeker seeker;
    //Rigidbody2D enemyRB;

    [SerializeField] bool isPatrollingEnemey;
    //[SerializeField] Transform target;
    //[SerializeField] float speed;
    //[SerializeField] float nextWayPointDistance;



    [SerializeField] int engagementDistance, standoffDistance, patrolZoneRadius, weapon1Range;
    //[SerializeField] int reloadTime1;
    //[SerializeField] int recoil1;

    private void Awake()
    {
        Physics2D.IgnoreLayerCollision(7, 11);
        //seeker = GetComponent<Seeker>();
        //enemyRB = GetComponent<Rigidbody2D>();
        patrolController = GetComponent<Patrol>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        patrolController.enabled = isPatrollingEnemey;
        destinationSetter.enabled = !isPatrollingEnemey;
        startPosition = transform.position;
        playerFound = false;
        onPatrol = true;
        attacking = false;
    }

    void Start()
    {
        attachedControllers = GetComponentsInChildren<WeaponController>();
        //unitManager = GetComponent<UnitManager2>();
        //arena = FindObjectOfType<ArenaManager>();
        //if (!arena)
        //    Debug.LogWarning("ArenaManager not found");
        startPosition = transform.position;
        playerTransform = FindObjectOfType<Player>().gameObject.transform;
        if (standoffDistance >= engagementDistance)
        {
            standoffDistance = engagementDistance - 5;
        }
        if (standoffDistance < 0)
        {
            standoffDistance = 0;
        }
    }

    void ChaseObject(Transform chaseObject)
    {
        playerFound = true;
        onPatrol = false;
        patrolController.enabled = false;
        destinationSetter.enabled = true;
        destinationSetter.target = chaseObject;
        //InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void PatrolArea()
    {
        onPatrol = true;
        playerFound = false;
        patrolController.enabled = true;
        destinationSetter.enabled = false;
    }

    private void Update()
    {

        if (playerTransform != null)
        {
            playerDirection = playerTransform.position - transform.position;
            if (playerDirection.magnitude <= weapon1Range && !attacking)
            {
                attacking = true;
                //Debug.Log("Attack Initialized");
                foreach (var controller in attachedControllers)
                {
                    if (controller != null)
                        controller.StartRepeatFire();
                }
            }
            if (playerDirection.magnitude > weapon1Range && attacking)
            {
                attacking = false;
                //Debug.Log("Attack Canceled");
                foreach (var controller in attachedControllers)
                    controller.StopRepeatFire();
            }
            if ((transform.position - startPosition).magnitude > patrolZoneRadius && !onPatrol)
            {
                //Debug.Log("Going to Patrol Zone");
                PatrolArea();
            }
            if (playerDirection.magnitude < engagementDistance && !playerFound)
            {
                //Debug.Log("Chasing Player");
                ChaseObject(playerTransform);
            }
        }
        else
        {
            if (!onPatrol)
            {
                PatrolArea();
            }
            if (attacking)
            {
                attacking = false;
                //Debug.Log("Attack Canceled");
                foreach (var controller in attachedControllers)
                    controller.StopRepeatFire();
            }
        }
    }













    //void UpdatePath()
    //{
    //    if (seeker.IsDone())
    //    {
    //        seeker.StartPath(enemyRB.position, target.position, OnPathComplete);
    //    }
    //}

    //void OnPathComplete(Path p)
    //{
    //    if (!p.error)
    //    {
    //        path = p;
    //        currentWaypoint = 0;
    //    }
    //}

    //private void FixedUpdate()
    //{
    //    if (path == null)
    //    {
    //        return;
    //    }
    //    if(currentWaypoint >= path.vectorPath.Count)
    //    {
    //        reachedEndOfPath = true;
    //        return;
    //    }
    //    else
    //    {
    //        reachedEndOfPath = false;
    //    }

    //    Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - enemyRB.position).normalized;
    //    Vector2 force = speed * direction * Time.deltaTime;

    //    enemyRB.AddForce(force);

    //    float distance = Vector2.Distance(enemyRB.position, path.vectorPath[currentWaypoint]);

    //    if (distance < nextWayPointDistance)
    //    {
    //        currentWaypoint++;
    //    }
    //}
}

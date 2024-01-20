using BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody;
using System;
using UnityEngine;

public class Turret : MonoBehaviour
{
    bool fullRotation;
    float rotationMidpoint;
    [SerializeField] Transform traceTarget = null;
    [Tooltip("Width of valid turnig arc (from 0 to 'limit'). Set to Zero for 360 degree rotation. Rotate the parent 'Mount' component to change the start location of the rotation limit")]
    [Range(0, 359)]
    [SerializeField] int rotationLimit = 0;
    [Tooltip("Speed is represented as degrees per second")]
    [SerializeField] float maxAngularSpeed = 180f;
    [SerializeField] Mount mount;

    [Space(20)]
    [SerializeField] bool debug = false;

    void Awake()
    {
        if (rotationLimit != 0)
        {
            fullRotation = false;
            rotationMidpoint = (360 - rotationLimit) / 2.0f;
        }
        else
        {
            fullRotation = true;
        }
        if (traceTarget == null)
        {
            if (GetComponentInParent<Player>() != null)
                traceTarget = GetComponentInParent<Player>().TargetTransform;
            else
                traceTarget = GameObject.Find("Player").transform; // Replace with an assignment from a parent object for AI controlled entities
        }
    }

    void Update()
    {
        if (traceTarget != null)
        {
            Vector2 targetDirection = (traceTarget.position - transform.position).normalized;
            float targetRotation = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            float rotationDifference = Mathf.DeltaAngle(transform.eulerAngles.z, targetRotation);
            float rotationChange;
            if (!fullRotation)
            {
                rotationDifference = CalculateRotation(rotationDifference);
            }
            rotationChange = Mathf.Sign(rotationDifference) * Mathf.Min(Mathf.Abs(rotationDifference), maxAngularSpeed * Time.deltaTime);
            transform.Rotate(Vector3.forward, rotationChange);
            if (debug)
                DebugDraw(targetDirection);
        }
    }

    float CalculateRotation(float rotationDifference)
    {

        float rotationAttemptAngle = ((transform.localEulerAngles.z + rotationDifference) + 360) % 360;
        float currentAngle = (transform.localEulerAngles.z + 360) % 360;
        Debug.Log("Attempt angle: " + rotationAttemptAngle + "  Current: " + currentAngle);
        if (rotationAttemptAngle > currentAngle)
        {
            if (rotationAttemptAngle < rotationLimit) // counterclockwise, less than limit
            {
                if ((rotationAttemptAngle - currentAngle) < 0)
                    Debug.LogWarning("Counterclockwise was negative 1: " + (rotationAttemptAngle - currentAngle));
                return rotationAttemptAngle - currentAngle; // POSITIVE
            }
            else if (rotationAttemptAngle < rotationMidpoint) // counterclockwise, stop at limit
            {
                if ((currentAngle - rotationLimit) < 0)
                    Debug.LogWarning("Counterclockwise was negative 2: " + (currentAngle - rotationLimit));
                return currentAngle - rotationLimit;
            }
            else
            {
                if ((0 - currentAngle) > 0)
                    Debug.LogWarning("Clockwise was positive 1: " + (0 - currentAngle));
                return 0 - currentAngle; // clockwise, return to zero
            }

        }
        else // rotation attempt angle is smaller than (or equal to) current
        {
            if ((rotationAttemptAngle - currentAngle) > 0)
                Debug.LogWarning("Clockwise was positive 2: " + (rotationAttemptAngle - currentAngle));
            return rotationAttemptAngle - currentAngle;
        }
    }

    void DebugDraw(Vector2 targetDirection)
    {
        Debug.DrawRay(transform.position, targetDirection, Color.green);
        Debug.DrawRay(transform.position, transform.right, Color.blue);
    }

    public void SetTraceTarget(Transform target)
    {
        traceTarget = target;
    }
}

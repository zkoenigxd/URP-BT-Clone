using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Mount : MonoBehaviour
{
    bool tracingEnable = true;
    bool rotationLimit = false;
    [SerializeField] Transform traceTarget = null;
    [SerializeField] float minRotationLimit;
    [SerializeField] float maxRotationLimit;
    [SerializeField] float maxAngularSpeed = 180f;

    Transform vehicleTransform;

    void Awake()
    {
        if (minRotationLimit != maxRotationLimit)
        {
            rotationLimit = true;
        }
        if (GetComponentInParent<Player>())
        {
            vehicleTransform = GetComponentInParent<Player>().gameObject.transform;
        }
        if (GetComponentInParent<EnemyBrain>())
        {
            vehicleTransform = GetComponentInParent<EnemyBrain>().gameObject.transform;
        }
        if (traceTarget == null)
        {
            if (GetComponentInParent<Player>() != null)
                traceTarget = GetComponentInParent<Player>().TargetTransform;
            else
                traceTarget = GameObject.Find("Player").transform;
        }
    }

    private void Update()
    {
        if (tracingEnable && traceTarget != null)
        {
            Vector2 inputVector = traceTarget.position - transform.position;
            float targetRotation = Mathf.Atan2(inputVector.y, inputVector.x) * Mathf.Rad2Deg;
            targetRotation = (targetRotation + 360) % 360;
            float currentRotation = transform.eulerAngles.z;
            float rotationDifference = Mathf.DeltaAngle(currentRotation, targetRotation);

            // Calculate the allowed rotation for this frame based on the maximum angular speed and time
            float allowedRotation = maxAngularSpeed * Time.deltaTime;

            // Clamp the rotation difference to the allowed rotation
            rotationDifference = Mathf.Clamp(rotationDifference, -allowedRotation, allowedRotation);

            float newRotation = currentRotation + rotationDifference;
            float minLimit = (vehicleTransform.eulerAngles.z + minRotationLimit + 360) % 360;
            float maxLimit = (vehicleTransform.eulerAngles.z + maxRotationLimit + 360) % 360;
            if (rotationLimit)
            {
                if (minLimit < maxLimit)
                    newRotation = Mathf.Clamp(newRotation, minLimit, maxLimit);
                else
                    newRotation = Mathf.Clamp(newRotation, maxLimit, minLimit);
            }
            transform.rotation = Quaternion.AngleAxis(newRotation, Vector3.forward);
            Debug.Log("Target Rotation: " + targetRotation + "  New Rotation: " + newRotation + "\nMinLimit: " + minLimit + "  Max Limit: " + maxLimit);
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools
{
    public static bool TraceTarget(Transform workObject, Vector3 targetPos, float deltaAngle, bool rotationLimit)
    {
        if (workObject == null)
            return false;

        Vector2 direct = workObject.position - targetPos; //changed to V2

        // = new Vector3(direct.x, direct.y, 0.0f);

        float angle = Vector3.Angle(Vector3.up, direct);
        if ((targetPos.x - workObject.TransformPoint(Vector3.zero).x) < 0)
        {
            angle = 360.0f - angle;
        }
        float targetAngle = Mathf.Round(angle / (float)deltaAngle) * (float)deltaAngle;


        float currAngle = workObject.rotation.eulerAngles.z;

        if (rotationLimit)
        {
            if (Mathf.Abs(currAngle - targetAngle) > deltaAngle)
            {
                targetAngle = currAngle + GetStepDirection(currAngle, targetAngle, 0) * deltaAngle;
                workObject.rotation = Quaternion.Euler(0.0f, 0.0f, targetAngle);
                return true;
            }
        }
        else
        {
            if (Mathf.Abs(currAngle - targetAngle) > deltaAngle)
            {
                targetAngle = currAngle + GetStepDirection(currAngle, targetAngle) * deltaAngle;
                workObject.rotation = Quaternion.Euler(0.0f, 0.0f, targetAngle);
                return true;
            }
        }
        return false;
    }

    //180  175
    private static float GetStepDirection(float currAngle, float targetAngle)
    {
        float oppositeAngle = currAngle + 180.0f;
        if (oppositeAngle > 360.0f)
            oppositeAngle -= 360.0f;
        if ((int)currAngle <= 180)
        {
            if (targetAngle >= currAngle && targetAngle < oppositeAngle)
                return 1.0f;
            else
                return -1.0f;
        }
        else
        {
            if (targetAngle >= currAngle || targetAngle < oppositeAngle)
                return 1.0f;
            else
                return -1.0f;
        }
    }

    private static float GetStepDirection(float currAngle, float targetAngle, int limitLocation)
    {
        if (targetAngle >= currAngle)
            return 1.0f;
        else
            return -1.0f;
    }
}

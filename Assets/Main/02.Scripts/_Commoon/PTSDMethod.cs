using System.Collections.Generic;
using UnityEngine;

public class PTSDMethod
{
    public static TrollerMove GetClosestTarget(List<TrollerMove> targets, Vector3 fromPosition)
    {
        TrollerMove closest = null;
        float minDistance = Mathf.Infinity;

        foreach (TrollerMove target in targets)
        {
            float distance = Vector3.Distance(fromPosition, target.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = target.transform.GetComponent<TrollerMove>();
            }
        }

        return closest;
    }
}

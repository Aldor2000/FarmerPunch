using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform targetTransform;  // Assign the bubble's transform in the Inspector
    public float followSpeed = 5f;     // Adjust for smoothness

    void Update()
    {
        if (targetTransform != null)
        {
            Vector3 targetPosition = new Vector3(targetTransform.position.x, targetTransform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }
}

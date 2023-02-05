using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmBehaviour : MonoBehaviour
{
    private Transform originalParent;
    private Vector3 originalPosition;
    private Vector3 wantedPosition => originalParent.position + originalPosition;

    public Animator animator;

    public Transform hand;

    public float moveSpeed;
    public float moveSpeedSpring;
    private float currentSpeed;

    public float rotationSpeed;

    private void Awake()
    {
        originalParent= transform.parent;
        originalPosition = transform.localPosition;
        transform.parent = null;
    }

    private void Update()
    {
        if(originalParent == null)
        {
            Destroy(gameObject);
            return;
        }
        float dist = Vector3.Distance(transform.position, wantedPosition);

        currentSpeed = Mathf.Lerp(currentSpeed, dist * moveSpeed, Time.deltaTime * moveSpeedSpring);

        transform.position += (wantedPosition - transform.position).normalized * Time.deltaTime * currentSpeed;
        //transform.position = Vector3.MoveTowards(transform.position, wantedPosition, Time.deltaTime * currentSpeed);

        transform.rotation = Quaternion.Slerp(transform.rotation, originalParent.rotation, Time.deltaTime * rotationSpeed);

    }
}

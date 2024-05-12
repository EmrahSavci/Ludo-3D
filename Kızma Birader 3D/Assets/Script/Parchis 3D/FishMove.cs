using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMove : MonoBehaviour
{
    public List<Transform> movePoint = new List<Transform>();
    public int pointIndex = 0;
    public float moveSpeed = 5;
    public float rotateSpeed = 250;
    public Transform targetPoint;
    void Start()
    {
        SelectPoint();
    }

    void SelectPoint()
    {
        pointIndex = Random.Range(0, movePoint.Count);
        targetPoint = movePoint[pointIndex];
    }
    void Update()
    {
        if (targetPoint == null)
            return;

        if ((targetPoint.position - transform.position).sqrMagnitude <= 0.2f)
            SelectPoint();
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, Time.deltaTime * moveSpeed);
            Vector3 directionToTarget = (targetPoint.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(-directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
        }
            
    }
}

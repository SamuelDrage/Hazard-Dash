using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 nextPosition;
    private Vector3 originPosition;
    public float smooth;
    public float resetTime;
    private bool isPlatformMoving;

    private void Start()
    {
        originPosition = transform.localPosition;
        isPlatformMoving = false;
        StartCoroutine(ResetTime());
    }

    void FixedUpdate()
    {
        if (isPlatformMoving)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, nextPosition, smooth * Time.deltaTime);
            if (Vector3.Distance(transform.localPosition, nextPosition) <= 0.1f)
            {
                isPlatformMoving = false;
                nextPosition = originPosition;
                originPosition = transform.localPosition;
                StartCoroutine(ResetTime());
            }
        }
    }

    IEnumerator ResetTime()
    {
        yield return new WaitForSecondsRealtime(resetTime);
        isPlatformMoving = true;
    }
}

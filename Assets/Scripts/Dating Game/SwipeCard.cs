using UnityEngine;
using System.Collections;
using System;

public class SwipeCard : MonoBehaviour
{
    [SerializeField] float maxHorizontalMove = 200f;
    [SerializeField] float rotationFactor = 0.2f;
    [SerializeField] float maxTiltDegrees = 20f;
    [SerializeField] float swipeThreshold = 150f;
    [SerializeField] float swipeVelocityThreshold = 800f;
    [SerializeField] float returnSpeed = 10f;
    [SerializeField] float offscreenDistance = 1000f;
    [SerializeField] float flyOutDuration = 1f;

    Vector3 startPos;
    Quaternion startRot;
    Vector3 lastPos;
    Vector3 velocity;
    Vector3 dragDelta;
    bool dragging;

    public bool HasFlownOff { get; private set; }

    public Action<SwipeCard> OnDestroyed;

    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        lastPos = transform.position;
    }

    public void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragging = true;
            lastPos = transform.position;
            velocity = Vector3.zero;
        }

        if (dragging && Input.GetMouseButton(0))
        {
            float moveX = Mathf.Clamp(Input.mousePosition.x / Screen.width - 0.5f, -maxHorizontalMove, maxHorizontalMove);

            Vector3 targetPos = new Vector3(startPos.x + moveX, startPos.y, startPos.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.3f);

            float rot = Mathf.Clamp(moveX * rotationFactor, -maxTiltDegrees, maxTiltDegrees);
            transform.rotation = Quaternion.Euler(0f, 0f, rot);

            dragDelta = new Vector3(moveX, 0f, 0f);

            if (Mathf.Abs(moveX) < 0.05f)
            {
                transform.position = Vector3.Lerp(transform.position, startPos, Time.deltaTime * returnSpeed);
                transform.rotation = Quaternion.Slerp(transform.rotation, startRot, Time.deltaTime * returnSpeed);
            }
        }

        if (Input.GetMouseButtonUp(0) && dragging)
        {
            dragging = false;

            bool far = Mathf.Abs(dragDelta.x) > swipeThreshold;
            bool fast = Mathf.Abs(velocity.x) > swipeVelocityThreshold;

            if (far || fast)
            {
                Vector3 dir = ((dragDelta.x != 0f ? dragDelta.x : velocity.x) > 0f) ? Vector3.right : Vector3.left;
                StartCoroutine(FlyOff(dir));
            }
            else
            {
                StartCoroutine(SnapBack());
            }
        }
    }

    IEnumerator SnapBack()
    {
        float t = 0f;
        Vector3 fromPos = transform.position;
        Quaternion fromRot = transform.rotation;
        while (t < 1f)
        {
            t += Time.deltaTime * returnSpeed;
            transform.position = Vector3.Lerp(fromPos, startPos, t);
            transform.rotation = Quaternion.Slerp(fromRot, startRot, t);
            yield return null;
        }
        transform.position = startPos;
        transform.rotation = startRot;
    }

    IEnumerator FlyOff(Vector3 dir)
    {
        HasFlownOff = true;
        Vector3 fromPos = transform.position;
        Quaternion fromRot = transform.rotation;
        Vector3 toPos = fromPos + dir * offscreenDistance;
        Quaternion toRot = Quaternion.Euler(0f, 0f, dir.x > 0f ? maxTiltDegrees : -maxTiltDegrees);

        float t = 0f;
        while (t < 0.6f)
        {
            t += Time.deltaTime / Mathf.Max(flyOutDuration, 0.0001f);
            transform.position = Vector3.Lerp(fromPos, toPos, t);
            transform.rotation = Quaternion.Slerp(fromRot, toRot, t);
            yield return null;
        }

        OnDestroyed?.Invoke(this); 
        Destroy(gameObject);
    }
}

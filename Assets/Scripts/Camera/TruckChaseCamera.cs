
using UnityEngine;

public class TruckChaseCamera : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Camera Position")]
    [SerializeField] private Vector3 offset =
        new Vector3(0f, 5f, -9f);

    [SerializeField, Min(0.1f)]
    private float followSpeed = 5f;

    [SerializeField, Min(0.1f)]
    private float lookHeight = 1.5f;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 desiredPosition =
            target.TransformPoint(offset);

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime);

        Vector3 lookPoint =
            target.position + Vector3.up * lookHeight;

        transform.LookAt(lookPoint);
    }
}

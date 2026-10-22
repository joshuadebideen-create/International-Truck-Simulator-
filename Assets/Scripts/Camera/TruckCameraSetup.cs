
using UnityEngine;

public class TruckCameraSetup : MonoBehaviour
{
    [SerializeField] private Transform truckTarget;
    [SerializeField] private Camera chaseCamera;

    public void Configure(Transform target, Camera cameraToUse)
    {
        truckTarget = target;
        chaseCamera = cameraToUse;

        if (chaseCamera == null || truckTarget == null)
            return;

        TruckChaseCamera follow =
            chaseCamera.GetComponent<TruckChaseCamera>();

        if (follow == null)
            follow = chaseCamera.gameObject
                .AddComponent<TruckChaseCamera>();

        follow.SetTarget(truckTarget);
    }

    private void Start()
    {
        if (truckTarget != null && chaseCamera != null)
            Configure(truckTarget, chaseCamera);
    }
}

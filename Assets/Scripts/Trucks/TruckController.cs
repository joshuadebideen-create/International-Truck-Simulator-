
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TruckController : MonoBehaviour
{
    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider[] driveWheels;
    [SerializeField] private WheelCollider[] steeringWheels;
    [SerializeField] private WheelCollider[] brakeWheels;

    [Header("Truck Configuration")]
    [SerializeField] private TruckDefinition truckDefinition;

    [Header("Driving Settings")]
    [Min(0f)]
    [SerializeField] private float maxMotorTorque = 2500f;

    [Min(0f)]
    [SerializeField] private float maxBrakeTorque = 6000f;

    [Range(0f, 45f)]
    [SerializeField] private float maxSteeringAngle = 32f;

    [Header("Truck State")]
    [SerializeField, Range(0f, 1f)]
    private float throttleInput;

    [SerializeField, Range(0f, 1f)]
    private float brakeInput;

    [SerializeField, Range(-1f, 1f)]
    private float steeringInput;

    private Rigidbody truckBody;

    public float SpeedKmh { get; private set; }

    private void Awake()
    {
        truckBody = GetComponent<Rigidbody>();

        if (truckDefinition != null)
        {
            truckBody.mass = truckDefinition.vehicleMassKg;
            maxMotorTorque = truckDefinition.torqueNm;
        }
    }

    public void SetControls(
        float throttle,
        float brake,
        float steering)
    {
        throttleInput = Mathf.Clamp01(throttle);
        brakeInput = Mathf.Clamp01(brake);
        steeringInput = Mathf.Clamp(steering, -1f, 1f);
    }

    public void SetThrottle(float value)
    {
        throttleInput = Mathf.Clamp01(value);
    }

    public void SetBrake(float value)
    {
        brakeInput = Mathf.Clamp01(value);
    }

    public void SetSteering(float value)
    {
        steeringInput = Mathf.Clamp(value, -1f, 1f);
    }

    public void ResetControls()
    {
        throttleInput = 0f;
        brakeInput = 0f;
        steeringInput = 0f;
    }

    private void FixedUpdate()
    {
        if (truckBody == null)
            return;

        Vector3 velocity = truckBody.linearVelocity;

        float maxSpeedKmh = truckDefinition != null
            ? truckDefinition.maximumSpeedKmh
            : 0f;

        Vector3 horizontalVelocity =
            new Vector3(velocity.x, 0f, velocity.z);

        if (maxSpeedKmh > 0f &&
            horizontalVelocity.magnitude > maxSpeedKmh / 3.6f)
        {
            horizontalVelocity =
                horizontalVelocity.normalized * (maxSpeedKmh / 3.6f);

            truckBody.linearVelocity = new Vector3(
                horizontalVelocity.x,
                velocity.y,
                horizontalVelocity.z
            );
        }

        SpeedKmh = truckBody.linearVelocity.magnitude * 3.6f;

        ApplyMotorTorque();
        ApplyBrakes();
        ApplySteering();
    }

    private void ApplyMotorTorque()
    {
        if (driveWheels == null)
            return;

        float torque = maxMotorTorque * throttleInput;

        foreach (WheelCollider wheel in driveWheels)
        {
            if (wheel != null)
                wheel.motorTorque = torque;
        }
    }

    private void ApplyBrakes()
    {
        if (brakeWheels == null)
            return;

        float torque = maxBrakeTorque * brakeInput;

        foreach (WheelCollider wheel in brakeWheels)
        {
            if (wheel != null)
                wheel.brakeTorque = torque;
        }
    }

    private void ApplySteering()
    {
        if (steeringWheels == null)
            return;

        float angle = maxSteeringAngle * steeringInput;

        foreach (WheelCollider wheel in steeringWheels)
        {
            if (wheel != null)
                wheel.steerAngle = angle;
        }
    }
}


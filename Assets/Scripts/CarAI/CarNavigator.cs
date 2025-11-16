using UnityEngine;
using StarterAssets;

public class CarNavigatorScript : MonoBehaviour
{
    [Header("Car Settings")]
    public float baseMovingSpeed = 10f;
    public float turningSpeed = 300f;
    public float stopDistance = 1f;

    [Header("Destination Settings")]
    public Vector3 destination;
    public bool destinationReached;
    private float currentMovingSpeed;

    [Header("Player Detection")]
    public GameObject sensor; // Assign an empty GameObject at car's front
    public float detectionRange = 10f;
    public float brakeDistance = 5f; // Distance to start slowing down
    [SerializeField] private GameObject detectedPlayer;
    [SerializeField] private bool playerDetected;

    private void Start()
    {
        currentMovingSpeed = baseMovingSpeed;

        // Warn if sensor isn't assigned
        if (sensor == null)
        {
            Debug.LogWarning("Sensor not assigned to CarNavigatorScript. Player detection won't work.");
        }
    }

    private void Update()
    {
        DetectPlayer();
        HandlePlayerDetection();
        Drive();
        CheckDestinationReached();
    }

    void DetectPlayer()
    {
        playerDetected = false;
        detectedPlayer = null;

        if (sensor == null) return;

        // 1. First do the wide sphere check (unchanged)
        Collider[] hits = Physics.OverlapSphere(sensor.transform.position, 3f);
        foreach (Collider col in hits)
        {
            if (col.CompareTag("Player") || col.GetComponent<ThirdPersonController>() != null)
            {
                detectedPlayer = col.gameObject;
                playerDetected = true;
                return;
            }
        }

        // 2. Cone-shaped raycast with 5-degree spread
        float coneAngle = 5f; // Total cone width (5 degrees)
        int raysToCast = 5; // Number of rays to cast within the cone

        for (int i = 0; i < raysToCast; i++)
        {
            // Calculate angle for this ray (-2.5° to +2.5°)
            float angle = Mathf.Lerp(-coneAngle / 2, coneAngle / 2, (float)i / (raysToCast - 1));

            // Create rotated direction
            Vector3 direction = Quaternion.Euler(0, angle, 0) * sensor.transform.forward;

            // Cast the ray
            RaycastHit hitInfo;
            if (Physics.Raycast(sensor.transform.position, direction, out hitInfo, detectionRange))
            {
                if (hitInfo.transform.CompareTag("Player") ||
                    hitInfo.transform.GetComponent<ThirdPersonController>() != null)
                {
                    detectedPlayer = hitInfo.transform.gameObject;
                    playerDetected = true;
                    return; // Exit on first detection
                }
            }
        }
    }

    void HandlePlayerDetection()
    {
        if (!playerDetected || detectedPlayer == null)
        {
            currentMovingSpeed = Mathf.Lerp(currentMovingSpeed, baseMovingSpeed, Time.deltaTime * 5f);
            return;
        }

        float distance = Vector3.Distance(sensor.transform.position, detectedPlayer.transform.position);

        if (distance <= brakeDistance)
        {
            float speedFactor = Mathf.Clamp01(distance / brakeDistance);
            currentMovingSpeed = Mathf.Lerp(0, baseMovingSpeed * 0.3f, speedFactor);

            if (distance < stopDistance * 1.5f)
            {
                currentMovingSpeed = 0;
            }
        }
    }

    void CheckDestinationReached()
    {
        if (Vector3.Distance(transform.position, destination) <= stopDistance)
        {
            destinationReached = true;
        }
    }

    public void Drive()
    {
        if (transform.position != destination && !destinationReached)
        {
            Vector3 destinationDirection = destination - transform.position;
            destinationDirection.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                turningSpeed * Time.deltaTime
            );

            transform.Translate(Vector3.forward * currentMovingSpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (sensor == null) return;

        float coneAngle = 5f;
        int raysToCast = 5;
        float gizmoSphereSize = 0.1f;

        // Draw the center line (original forward ray)
        Gizmos.color = Color.magenta;
        Vector3 centerEnd = sensor.transform.position + sensor.transform.forward * detectionRange;
        Gizmos.DrawLine(sensor.transform.position, centerEnd);

        // Draw the cone rays
        for (int i = 0; i < raysToCast; i++)
        {
            float angle = Mathf.Lerp(-coneAngle / 2, coneAngle / 2, (float)i / (raysToCast - 1));
            Vector3 direction = Quaternion.Euler(0, angle, 0) * sensor.transform.forward;
            Vector3 endPoint = sensor.transform.position + direction * detectionRange;

            Gizmos.color = i == raysToCast / 2 ? Color.magenta : new Color(1f, 0.5f, 0f); // Orange for side rays
            Gizmos.DrawLine(sensor.transform.position, endPoint);

            // Small sphere at end of each ray
            Gizmos.DrawSphere(endPoint, gizmoSphereSize);
        }

        // Draw detection sphere if player is detected
        if (playerDetected && detectedPlayer != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(detectedPlayer.transform.position, 0.5f);

            // Draw line to detected player
            Gizmos.DrawLine(sensor.transform.position, detectedPlayer.transform.position);
        }

        // Draw the initial wide detection sphere
        Gizmos.color = new Color(0f, 1f, 0f, 0.1f); // Transparent green
        Gizmos.DrawWireSphere(sensor.transform.position, 3f);
    }
}
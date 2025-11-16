using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionRange = 3f;
    public float interactionAngle = 45f;
    private InteractableBase currentInteractable;

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (currentInteractable == null) return;

        // NPC Interaction
        if (currentInteractable is InteractableNPC && Input.GetKeyDown(KeyCode.E))
        {
            currentInteractable.TriggerInteraction();
        }
        // Equipment Interaction
        else if (currentInteractable is InteractableEquipment && Input.GetKeyDown(KeyCode.I))
        {
            currentInteractable.TriggerInteraction();
        }
        // Skip Interaction (works for both)
        else if (Input.GetKeyDown(KeyCode.H))
        {
            currentInteractable.SkipInteraction();
        }
    }

    void FixedUpdate()
    {
        DetectInteractables();
    }

    void DetectInteractables()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionRange);
        InteractableBase nearestInteractable = null;
        float nearestDistance = Mathf.Infinity;

        foreach (var hit in hitColliders)
        {
            var interactable = hit.GetComponent<InteractableBase>();
            if (!interactable) continue;

            Vector3 directionToTarget = hit.transform.position - transform.position;
            directionToTarget.y = 0f;
            float angle = Vector3.Angle(transform.forward, directionToTarget);
            float distance = directionToTarget.magnitude;

            if (angle < interactionAngle && distance < nearestDistance)
            {
                nearestInteractable = interactable;
                nearestDistance = distance;
            }
        }

        UpdateCurrentInteractable(nearestInteractable);
    }

    void UpdateCurrentInteractable(InteractableBase newInteractable)
    {
        // Nothing changed
        if (currentInteractable == newInteractable) return;

        // Hide previous prompt
        if (currentInteractable != null)
        {
            currentInteractable.ShowInteractionPrompt(false);
        }

        // Show new prompt
        if (newInteractable != null)
        {
            newInteractable.ShowInteractionPrompt(true);
        }

        currentInteractable = newInteractable;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactionRange);

        Vector3 leftBoundary = Quaternion.Euler(0, -interactionAngle, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, interactionAngle, 0) * transform.forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, leftBoundary * interactionRange);
        Gizmos.DrawRay(transform.position, rightBoundary * interactionRange);
    }
}
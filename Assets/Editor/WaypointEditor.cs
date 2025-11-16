using UnityEngine;
using UnityEditor;

public class WaypointEditor
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void OnDrawSceneGizmo(Waypoint waypoint, GizmoType gizmoType)
    {
        // Set sphere color based on selection
        if ((gizmoType & GizmoType.Selected) != 0)
        {
            Gizmos.color = Color.blue;
        }
        else
        {
            Gizmos.color = Color.blue * 1f;
        }

        // Draw the waypoint sphere
        Gizmos.DrawSphere(waypoint.transform.position, 0.3f);

        // Draw the waypoint width indicator
        Gizmos.color = Color.white;
        Gizmos.DrawLine(waypoint.transform.position + (waypoint.transform.right * waypoint.waypointWidth / 2f),
                       waypoint.transform.position - (waypoint.transform.right * waypoint.waypointWidth / 2f));

        // Draw connections to previous waypoint (red)
        if (waypoint.previousWaypoint != null)
        {
            Gizmos.color = Color.red;
            Vector3 leftOffset = waypoint.transform.right * waypoint.waypointWidth / 2f;
            Vector3 rightOffset = -waypoint.transform.right * waypoint.waypointWidth / 2f;

            // Draw from left side to previous waypoint's right side
            Vector3 previousRightOffset = waypoint.previousWaypoint.transform.right * waypoint.previousWaypoint.waypointWidth / 2f;
            Gizmos.DrawLine(waypoint.transform.position + leftOffset,
                           waypoint.previousWaypoint.transform.position + previousRightOffset);

            // Draw from right side to previous waypoint's left side
            Vector3 previousLeftOffset = -waypoint.previousWaypoint.transform.right * waypoint.previousWaypoint.waypointWidth / 2f;
            Gizmos.DrawLine(waypoint.transform.position + rightOffset,
                           waypoint.previousWaypoint.transform.position + previousLeftOffset);
        }

        // Draw connections to next waypoint (green)
        if (waypoint.nextWaypoint != null)
        {
            Gizmos.color = Color.green;
            Vector3 leftOffset = waypoint.transform.right * waypoint.waypointWidth / 2f;
            Vector3 rightOffset = -waypoint.transform.right * waypoint.waypointWidth / 2f;

            // Draw from left side to next waypoint's right side
            Vector3 nextRightOffset = waypoint.nextWaypoint.transform.right * waypoint.nextWaypoint.waypointWidth / 2f;
            Gizmos.DrawLine(waypoint.transform.position + leftOffset,
                           waypoint.nextWaypoint.transform.position + nextRightOffset);

            // Draw from right side to next waypoint's left side
            Vector3 nextLeftOffset = -waypoint.nextWaypoint.transform.right * waypoint.nextWaypoint.waypointWidth / 2f;
            Gizmos.DrawLine(waypoint.transform.position + rightOffset,
                           waypoint.nextWaypoint.transform.position + nextLeftOffset);
        }
    }
}
using UnityEngine;

public class CarWaypointNavigator : MonoBehaviour
{
    [Header("Car AI")]
    public CarNavigatorScript car;
    public Waypoint currentWaypoint;

    private void Awake()
    {
        car = GetComponent<CarNavigatorScript>();
    }

    private void Start()
    {
        if (currentWaypoint != null)
        {
            car.destination = currentWaypoint.GetPosition();
            car.destinationReached = false;
        }
    }

    private void Update()
    {
        if (car.destinationReached && currentWaypoint != null && currentWaypoint.nextWaypoint != null)
        {
            currentWaypoint = currentWaypoint.nextWaypoint;
            car.destination = currentWaypoint.GetPosition();
            car.destinationReached = false;
        }
    }
}
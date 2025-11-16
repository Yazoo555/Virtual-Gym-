using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    public GameObject[] AlPrefab;
    public int A1ToSpawn;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        // Get all waypoints from children
        List<Transform> waypoints = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
            waypoints.Add(transform.GetChild(i));

        // Shuffle waypoints to randomize
        for (int i = 0; i < waypoints.Count; i++)
        {
            Transform temp = waypoints[i];
            int randomIndex = Random.Range(i, waypoints.Count);
            waypoints[i] = waypoints[randomIndex];
            waypoints[randomIndex] = temp;
        }

        int spawnCount = Mathf.Min(A1ToSpawn, waypoints.Count); // Don't spawn more than available waypoints

        for (int count = 0; count < spawnCount; count++)
        {
            int prefabIndex = Random.Range(0, AlPrefab.Length);
            GameObject obj = Instantiate(AlPrefab[prefabIndex]);

            Transform waypoint = waypoints[count];
            obj.GetComponent<CarWaypointNavigator>().currentWaypoint = waypoint.GetComponent<Waypoint>();
            obj.transform.position = waypoint.position;

            yield return new WaitForSeconds(1f);
        }
    }
}

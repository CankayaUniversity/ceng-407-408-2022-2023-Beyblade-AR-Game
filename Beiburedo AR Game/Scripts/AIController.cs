using UnityEngine;

public class AIController : MonoBehaviour
{
    public Transform[] waypoints; // Array of waypoint transforms
    private int currentWaypointIndex = 0; // Index of the current waypoint
    public float movementSpeed = 5f; // Movement speed of the AI

    private void Start()
    {
        StartCoroutine(ChooseRandomWaypointWithDelay(1f));
    }

    private System.Collections.IEnumerator ChooseRandomWaypointWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetRandomDestination();
    }

    private void SetRandomDestination()
    {
        if (waypoints.Length > 0)
        {
            int randomIndex = Random.Range(0, waypoints.Length);
            currentWaypointIndex = randomIndex;
        }
    }

    private void Update()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogError("No waypoints assigned.");
            return;
        }

        Transform currentWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (currentWaypoint.position - transform.position).normalized;
        Vector3 movement = direction * movementSpeed * Time.deltaTime;

        transform.Translate(movement);

        if (Vector3.Distance(transform.position, currentWaypoint.position) < 0.1f)
        {
            StartCoroutine(ChooseRandomWaypointWithDelay(1f));
        }
    }
}

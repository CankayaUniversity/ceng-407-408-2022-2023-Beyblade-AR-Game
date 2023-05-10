using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAiController : MonoBehaviour
{
    public NavMeshAgent agent;
    public float moveSpeed = 5f;
    public Transform[] waypoints;
    public float waitTime = 2f;

    private int currentWaypointIndex = 0;
    private bool isWaiting = false;
    private float waitTimer = 0f;
    public float sliderShowTime = 0.7f;
    public GameObject LosePanel;

    public Slider PlayerHealthSlider;

    private bool canCollide = true; // Whether the enemy can collide with the player

    private void Start()
    {
        agent.speed = moveSpeed;
        GoToNextWaypoint();
    }

    private void Update()
    {
        if (isWaiting)
        {
            // Wait at the current waypoint for the specified time
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                isWaiting = false;
                waitTimer = 0f;
                GoToNextWaypoint();
            }
        }
        else
        {
            // Check if the AI has reached the current waypoint
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                isWaiting = true;
            }
        }
    }

    private void GoToNextWaypoint()
    {
        // Choose a random waypoint to go to next
        int randomWaypointIndex = Random.Range(0, waypoints.Length);
        currentWaypointIndex = randomWaypointIndex;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    private IEnumerator OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && canCollide)
        {
            Debug.Log("Collided with Player!");
            PlayerHealthSlider.value = PlayerHealthSlider.value - 0.5f;

            if (PlayerHealthSlider.value <= 0)
            {
                LosePanel.SetActive(true);
                Invoke("ShowSlider", sliderShowTime);
            }
            else
            {
                ShowSlider();
            }

            canCollide = false; // Set canCollide to false to prevent further collisions
            yield return new WaitForSeconds(1f); // Wait for one second
            canCollide = true; // Set canCollide back to true
        }
    }

    private void ShowSlider()
    {
        PlayerHealthSlider.gameObject.SetActive(true);
        Invoke("HideSlider", sliderShowTime);
    }

    private void HideSlider()
    {
        PlayerHealthSlider.gameObject.SetActive(false);
    }
}

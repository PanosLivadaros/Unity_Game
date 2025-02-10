using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class NPCMovement : MonoBehaviour
{
    public Transform destination; // Target destination
    private NavMeshAgent agent; // NavMeshAgent controlling movement
    private Vector3 originalPosition; // Original position of NPC
    private Quaternion originalRotation; // Original rotation of NPC
    private bool isReturning = false; // Flag to track if NPC is returning to its original position
    private float threshold = 1f; // Distance threshold to consider as "reached"
    public float pauseDuration = 5f; // Time to stop at each destination

    public Animator animator;

    private bool isPaused = false; // Flag to prevent movement during pause

    void Start()
    {
        animator.SetBool("IsWalking", true);

        // Get NavMeshAgent and Animator components
        agent = this.GetComponent<NavMeshAgent>();

        // Save original position
        originalPosition = this.transform.position;
        originalRotation = this.transform.rotation;
    }

    void Update()
    {
        // Prevent movement while paused
        if (isPaused) return;

        // Check if NPC has reached the current target
        if (!isReturning && Vector3.Distance(transform.position, destination.position) <= threshold)
        {
            StartCoroutine(PauseBeforeReturning(originalPosition));
        }
        else if (isReturning && Vector3.Distance(transform.position, originalPosition) <= threshold)
        {
            this.transform.rotation = new Quaternion(this.transform.rotation.x, originalRotation.y, this.transform.rotation.z, 0f); // Rotate NPC to face the initial direction he was facing
            StartCoroutine(PauseBeforeReturning(destination.position));
        }
        else if (!isReturning)
        {
            // Keep moving towards the destination
            agent.SetDestination(destination.position);
        }
    }

    private IEnumerator PauseBeforeReturning(Vector3 nextTarget)
    {
        // Stop movement and set the pause flag
        agent.ResetPath();
        animator.SetBool("IsWalking", false);
        isPaused = true;

        // Wait for the specified pause duration
        yield return new WaitForSeconds(pauseDuration);

        // Resume movement to the next target
        agent.SetDestination(nextTarget);
        isReturning = !isReturning; // Toggle the returning state
        animator.SetBool("IsWalking", true);
        isPaused = false;
    }
}

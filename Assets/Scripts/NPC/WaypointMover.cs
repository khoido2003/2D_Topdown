using System.Collections;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    [SerializeField]
    private Transform waypointParent;

    [SerializeField]
    private float moveSpeed = 2f;

    [SerializeField]
    private float waitTime = 2f;

    [SerializeField]
    private bool loopWaypoint = true;

    private Transform[] waypoints;

    private int curIndexWaypoint;

    private bool isWaiting = false;

    private Animator animator;

    private void Start()
    {
        //  animator = GetComponent<Animator>();

        waypoints = new Transform[waypointParent.childCount];

        for (int i = 0; i < waypointParent.childCount; i++)
        {
            waypoints[i] = waypointParent.GetChild(i);
        }
    }

    private void Update()
    {
        if (PauseGame.IsGamePaused || isWaiting)
        {
            return;
        }

        MoveToWayPoint();
    }

    void MoveToWayPoint()
    {
        Transform target = waypoints[curIndexWaypoint];

        Vector2 direction = (target.position - transform.position).normalized;

        /*
        animator.SetFloat("InputX", direction.x);
        animator.SetFloat("InputY", direction.y);
        animator.SetBool("IsWalking", direction.magnitude > 0f);

        */
        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            StartCoroutine(WaitAtWayPoint());
        }
    }

    IEnumerator WaitAtWayPoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);

        //animator.SetBool("IsWalking", false);

        curIndexWaypoint = loopWaypoint
            ? (curIndexWaypoint + 1) % waypoints.Length
            : Mathf.Min(curIndexWaypoint + 1, waypoints.Length - 1);

        isWaiting = false;
    }
}

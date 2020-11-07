using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowAI : MonoBehaviour
{
    public enum States { Patrol, Follow, Attack }

    public NavMeshAgent agent;
    public GameObject target;
    public States currentState;
    public Transform[] wayPoints;
    private int currentWayPoint = 0;

    public float distance = 100f;
    public float attackDistance = 1.8f;

    public Animator animator;
    private NavMeshHit hit;
    public bool blocked;
    private float forgetTime = 2f;
    [SerializeField] private float unseenTime = 2f;
    public bool hitte;
    private GameObject player;

    void Start()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        hitte = player.gameObject.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>().hitted;

        UpdateStates();

        if (Vector3.Distance(agent.transform.position, target.transform.position) <= distance && !blocked)
        {
            currentState = States.Follow;
        }
        if (Vector3.Distance(agent.transform.position, target.transform.position) <= attackDistance)
        {
            currentState = States.Attack;
        }
        if (unseenTime >= forgetTime)
            currentState = States.Patrol;

        blocked = NavMesh.Raycast(transform.position + new Vector3(0, 1.175f, 0), target.transform.position + new Vector3(0, 1.8f, 0), out hit, NavMesh.AllAreas);
        Debug.DrawLine(transform.position + new Vector3(0, 1.75f, 0), target.transform.position + new Vector3(0, 1.8f, 0), blocked ? Color.red : Color.blue);
        if (blocked)
        {
            Debug.DrawRay(hit.position, Vector3.up, Color.red);
            unseenTime += Time.deltaTime;
        }
        if (!blocked)
        {
            unseenTime = 0;
        }
    }

    private void UpdateStates()
    {
        switch (currentState)
        {
            case States.Patrol:
                Patrol();
                break;
            case States.Follow:
                Follow();
                break;
            case States.Attack:
                Attack();
                break;
        }
    }

    private void Patrol()
    {
        if (agent.destination != wayPoints[currentWayPoint].position)
        {
            agent.destination = wayPoints[currentWayPoint].position;
            transform.LookAt(agent.destination);
        }
        if (HasReached())
        {
            //currentWayPoint++;
            //if (currentWayPoint >= wayPoints.Length)
            //{currentWayPoint = 0;}
            currentWayPoint = (currentWayPoint + 1) % wayPoints.Length;
        }
        animator.SetFloat("Speed", 0.4f);
    }

    private void Follow()
    {
        if (target != null)
        {
            agent.SetDestination(target.transform.position);
            animator.SetFloat("Speed", 1f);
        }
    }

    private void Attack()
    {
        agent.speed = 4.5f;
        transform.LookAt(target.transform.position);
        animator.SetFloat("Speed", 0f);
        animator.SetBool("Attacking", true);
        if (Vector3.Distance(agent.transform.position, target.transform.position) >= attackDistance)
        {
            animator.SetBool("Attacking", false);
        }
    }

    private bool HasReached()
    {
        return (agent.hasPath && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);
    }



    public float damage = 50;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword") && hitte)
        {
            gameObject.GetComponent<HealthController>().ApplyDamage(damage);
        }
    }
}

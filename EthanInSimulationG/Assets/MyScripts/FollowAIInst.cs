using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowAIInst : MonoBehaviour
{
    public enum States { Idle, Follow, Attack, Dead }

    public NavMeshAgent agent;
    private GameObject target;
    public States currentState;

    public float distance = 10f;
    public float attackDistance = 1.8f;

    public Animator animator;
    private NavMeshHit hit;
    public bool blocked;
    private float forgetTime = 2f;
    [SerializeField] private float unseenTime = 2f;
    //public bool hitte;
    private GameObject player;
    private EnemyActions actions;
    private HealthController zombieHealthController;
    public float zombieHealth;
    public bool isDead = false;
    private Rigidbody enemyRb;
    public GameObject bloodEffect;
    protected CapsuleCollider capsuleCollider;
    private float zombieDamage = 2f;


    float ATimer = 0;
    public AudioClip hitSound;
    public AudioClip attackSound;
    AudioSource audioSource;
    private GameManager gameManager;
    private float damage;

    void Start()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }

        target = GameObject.FindGameObjectWithTag("Player");
        player = GameObject.FindGameObjectWithTag("Player");
        actions = GetComponent<EnemyActions>();
        zombieHealthController = GetComponent<HealthController>();
        enemyRb = GetComponent<Rigidbody>();
        currentState = States.Idle;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    void FixedUpdate()
    {
        if (gameManager.isGameActive)
        {
            if (GameObject.FindWithTag("Sword"))
                damage = GameObject.FindWithTag("Sword").GetComponent<Weapon>().weaponDamage;


            //hitte = player.gameObject.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>().hitted;
            zombieHealth = zombieHealthController.health;

            if (isDead || (zombieHealth <= 0))
            {
                Debug.Log("deeeeeed");
                //currentState = States.Dead;
                agent.speed = 0f;
                currentState = States.Dead;
            }
            // if (isDead || zombieHealth <= 0)
            // {
            //     enabled = false;
            // }

            if (!isDead)
            {
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
                    currentState = States.Idle;

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
        }
        else
        {
            currentState = States.Idle;
        }

    }

    private void UpdateStates()
    {
        switch (currentState)
        {
            case States.Idle:
                Idle();
                break;
            case States.Follow:
                Follow();
                break;
            case States.Attack:
                Attack();
                break;
            case States.Dead:
                Dead();
                break;
        }
    }

    private void Idle()
    {

    }

    private void Follow()
    {
        if (target != null)
        {
            agent.SetDestination(target.transform.position);
            animator.SetFloat("Speed", 1f);
            agent.speed = 5f;
        }
    }

    private void Attack()
    {
        ASound();
        transform.LookAt(target.transform.position);
        animator.SetFloat("Speed", 0f);
        animator.SetBool("Attacking", true);
        if (Vector3.Distance(agent.transform.position, target.transform.position) >= attackDistance)
        {
            animator.SetBool("Attacking", false);
        }
    }

    private void Dead()
    {
        Debug.Log("deadzombiieeeee");
        isDead = true;
        Debug.Log("DeadState");
        actions.Fall();
        DeadZombieCollider();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            //other.GetComponent<Collider>().enabled = false;

            zombieHealthController.ApplyDamage(damage);
            audioSource.PlayOneShot(hitSound, 0.2f);

            //Vector3 awayFromSwordHitPoint = (transform.position - other.gameObject.transform.position);
            Vector3 awayFromPlayer = (transform.position - other.GetComponentInParent<Transform>().position);
            enemyRb.AddForce(awayFromPlayer * 1000f * Time.deltaTime, ForceMode.Impulse);

            Instantiate(bloodEffect, transform.position + new Vector3(0, 1.7f, 0), Quaternion.identity);


            if (zombieHealth > damage)
            {
                actions.Damaged();
            }
            if (zombieHealth <= damage)
            {
                currentState = States.Dead;
            }
            StartCoroutine(CalmDown());
        }
        if (other.CompareTag("Arrow"))
        {
            damage = GameObject.FindWithTag("Arrow").GetComponent<Weapon>().weaponDamage;
            zombieHealthController.ApplyDamage(damage);
            audioSource.PlayOneShot(hitSound, 0.2f);
            Vector3 awayFromPlayer = (transform.position - other.GetComponentInParent<Transform>().position);
            enemyRb.AddForce(awayFromPlayer * 1000f * Time.deltaTime, ForceMode.Impulse);

            Instantiate(bloodEffect, transform.position + new Vector3(0, 1.7f, 0), Quaternion.identity);
            if (zombieHealth > damage)
            {
                actions.Damaged();
            }
            if (zombieHealth <= damage)
            {
                currentState = States.Dead;
            }
        }

    }

    void DeadZombieCollider()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.enabled = false;
        // capsuleCollider.direction = 2;
        // capsuleCollider.height = 1.8f;
        // capsuleCollider.center = new Vector3(capsuleCollider.center.x, 0.15f, 0.18f);
    }

    void ASound()
    {
        ATimer += Time.deltaTime;
        if (ATimer >= 1f)
        {
            audioSource.PlayOneShot(attackSound, 0.2f);
            GameObject.FindWithTag("Player").GetComponent<HealthController>().ApplyDamage(zombieDamage);
            ATimer = 0;
        }
    }

    IEnumerator CalmDown()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject.FindWithTag("Sword").GetComponent<Collider>().enabled = true;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyController : MonoBehaviour
{
    public float camDist = 4;
    public float camAng = 2.5f;
    public float camHigh = 0;
    public FixedJoystick leftJoystick;
    public FixedTouchField touchField;
    [SerializeField] private float cameraAngleY;
    [SerializeField] private float cameraAngleSpeed = 0.1f;
    [SerializeField] private float cameraPosY = 3f;
    [SerializeField] private float cameraPosSpeed = 0.01f;
    protected Vector3 input;
    protected Vector3 velo;
    public bool isOnGround;

    public FixedButton jumpButton;
    public FixedButton crouchButton;
    public FixedButton attackButton;
    public FixedButton attack2Button;
    public FixedButton laserButton;
    public FixedButton thumbleButton;
    protected CapsuleCollider capsuleCollider;
    public bool isCrouching = false;
    public float jumpForce = 4f;

    public float thrust = 30.0f;
    public Rigidbody playerRb;
    //private Animator animator; ///******
    protected Actions actions;

    protected bool isAttacking = false;
    float walkingSoundTimer = 0f;
    public float damage = 50f;
    public bool hasPowerup;
    public float powerupStrength = 500f;
    public GameObject powerupIndicator;
    public Transform laserPoint;
    public LineRenderer laserTrail;
    public GameObject laserPointer;
    public bool areEyesHot = false;
    public bool isThumbling = false;
    private float playerHealth;
    private float health = 100;
    public Vector3 laserEnd;
    public ParticleSystem laserPointEnd;
    public Light laserSpotLight;
    private GameManager gameManager;
    public float laserDamage = 5f;
    public Collider swordCollider;
    private Collider arrowCollider;
    public GameObject getHitEffect;
    private ParticleSystem swordTrail;
    //private ParticleSystem arrowTrail;
    private bool withSword;
    private Animator animator;
    private bool isAiming;
    public GameObject arrowPrefab;
    public bool isChangeable = true;
    public float arrowPowerFactor;

    //sound
    public AudioClip jumpSound;
    public AudioClip walkingSound;
    public AudioClip laserSound;
    public AudioClip attackSound;
    public AudioClip thumbleSound;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        actions = GetComponent<Actions>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        audioSource = GetComponent<AudioSource>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        //getHitEffect = GameObject.Find("Canvas/Get Hit Effect");
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (gameManager.isGameActive)
        {
            withSword = gameObject.GetComponent<SwitchWeapon>().withSword;
            //playerTransform = gameObject.GetComponent<Transform>();

            if (withSword)
            {
                swordCollider = GameObject.FindWithTag("Sword").GetComponent<Collider>();
                swordTrail = GameObject.FindWithTag("Sword").GetComponentInChildren<ParticleSystem>();
            }
            //if (GameObject.FindWithTag("Bow"))
            if (!withSword)
            {
                //GameObject.FindWithTag("Arrow").SetActive(true);
                //arrowCollider = GameObject.FindWithTag("Arrow").GetComponent<Collider>();
                //arrowTrail = GameObject.FindWithTag("Arrow").GetComponentInChildren<ParticleSystem>();
            }
            playerHealth = GetComponent<HealthController>().health;

            if (gameManager.isGameActive)
            {
                PositionDetection();

                Walk();
                Jump();
                Crouch();
                if (withSword)
                    Attack();
                if (!withSword)
                    BowAttack();
                if (Input.GetKeyDown(KeyCode.R))
                {
                    actions.NEW();
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    actions.RRR();
                }
                LaserAttack();
                Thumble();
                WhenDamaged();

                powerupIndicator.transform.position = transform.position + new Vector3(0, 0.5f, 0);
            }
        }
    }

    private void Walk()
    {
        if (!isThumbling && !isAttacking)
        {
            walkingSoundTimer += Time.deltaTime;
            if (playerRb.velocity.magnitude > 3f && isOnGround)
            {
                actions.Run();
                walkingSoundTimer += Time.deltaTime;
                if (walkingSoundTimer >= 0.6f)
                {
                    audioSource.PlayOneShot(walkingSound, 0.2f);
                    walkingSoundTimer = 0;
                }
            }
            else if (playerRb.velocity.magnitude > 0.1f && isOnGround)
            {
                actions.Walk();
                if (walkingSoundTimer >= 0.6f)
                {
                    audioSource.PlayOneShot(walkingSound, 0.3f);
                    walkingSoundTimer = 0;
                }
            }
            else
                actions.Idle();
        }
    }

    private void BowAttack()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (Input.GetKeyDown(KeyCode.A) || attackButton.pressed)
        {
            arrowPowerFactor += Time.deltaTime;
            if (arrowPowerFactor > 2)
                arrowPowerFactor = 2;

            //float bi deger ver dene!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (!isAiming)
            {
                actions.DrawArrow();
                actions.Aiming(true);
            }
            isAiming = true;
        }

        if (!attackButton.pressed)
        {
            if (isAiming)
            {
                actions.Aiming(false);
                actions.DropArrow();
                isAiming = false;
                float time = Time.deltaTime;

                StartCoroutine(ArrowDelay());
            }
            // if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Aiming"))
            // {
            // actions.Aiming(false);
            // isAiming = false;
            // }
            //playerTransform = startPos;
        }
    }
    IEnumerator ArrowDelay()
    {
        yield return new WaitForSeconds(0.5f);
        Instantiate(arrowPrefab, laserPoint.position, Camera.main.transform.rotation);  //Laser&arrow point is the same(Aim)
        arrowCollider = GameObject.FindWithTag("Arrow").GetComponent<Collider>();
        arrowPowerFactor = 0;
    }
    private void Attack()
    {
        if (!isAttacking && !isCrouching)
        {
            swordCollider.enabled = false;

            if (Input.GetKeyDown(KeyCode.A) || attackButton.pressed)
            {
                swordCollider.enabled = true;
                swordTrail.Play();
                isChangeable = false;

                isAttacking = true;
                actions.Attack();
                //audioSource.PlayOneShot(attackSound, 0.2f);
                StartCoroutine(calmDown(0.5f));
            }
            if (Input.GetKeyDown(KeyCode.T) || attack2Button.pressed)
            {
                swordCollider.enabled = true;
                swordTrail.Play();
                isChangeable = false;

                isAttacking = true;
                actions.Attack2();
                StartCoroutine(calmDown(2f));
            }
        }

    }

    private void Jump()
    {
        if (isOnGround && jumpButton.pressed)
        {
            actions.Jump();
            //audioSource.PlayOneShot(jumpSound, 0.5f);
            playerRb.velocity = new Vector3(playerRb.velocity.x, jumpForce, playerRb.velocity.z);
            //splayerRb.AddForce(transform.up * thrust, ForceMode.Impulse);
        }
    }

    private void Crouch()
    {
        var cButton = crouchButton.pressed;
        if (!isCrouching && Input.GetKey(KeyCode.C) || cButton)
        {
            //crouch
            capsuleCollider.height = 0.7f;
            capsuleCollider.center = new Vector3(capsuleCollider.center.x, 0.35f, capsuleCollider.center.z);
            isCrouching = true;
            actions.Sitting(true);
        }

        Debug.DrawRay(transform.position, Vector3.up * 0.2f, Color.green);

        if (isCrouching && !Input.GetKey(KeyCode.C) && !cButton)
        {
            //try to stand up
            var cantStandUp = Physics.Raycast(transform.position, Vector3.up, 2f);

            if (!cantStandUp)
            {
                capsuleCollider.height = 1.4f; //char heigt 1.6 but idle pos collider height 1.4
                capsuleCollider.center = new Vector3(capsuleCollider.center.x, 0.7f, capsuleCollider.center.z);
                isCrouching = false;
                actions.Sitting(false);
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            hasPowerup = true;
            Destroy(other.gameObject);
            powerupIndicator.gameObject.SetActive(true);
            StartCoroutine(PowerupCountdownRoutine());
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (other.gameObject.transform.position - transform.position);
            enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }
    }

    private void PositionDetection()
    {
        input = new Vector3(leftJoystick.input.x, 0, leftJoystick.input.y);
        velo = Quaternion.AngleAxis(cameraAngleY, Vector3.up) * input * 5f;
        if (Input.GetKey(KeyCode.W))
        {
            velo.z = 10;
        }
        isOnGround = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.5f);

        playerRb.velocity = new Vector3(velo.x, playerRb.velocity.y, velo.z);
        transform.rotation = Quaternion.AngleAxis(cameraAngleY + Vector3.SignedAngle(Vector3.forward, input.normalized + Vector3.forward * 0.001f, Vector3.up), Vector3.up);

        cameraAngleY += touchField.TouchDist.x * cameraAngleSpeed;
        cameraPosY = Mathf.Clamp(cameraPosY - touchField.TouchDist.y * cameraPosSpeed, 1f, 7f);

        Camera.main.transform.position = transform.position + Quaternion.AngleAxis(cameraAngleY + 180, Vector3.up) * new Vector3(0, cameraPosY - camHigh, camDist);
        Camera.main.transform.rotation = Quaternion.LookRotation(transform.position + Vector3.up * camAng - Camera.main.transform.position, Vector3.up);

    }

    // void Fireball()
    // {
    //     if (Input.GetKeyDown(KeyCode.Q))
    //     {
    //         Instantiate(fireballPrefab, transform.position, transform.rotation);
    //         //Destroy(fireballPrefab, 3f);
    //     }
    // }

    void SpawnLaser(Vector3 hitPoint)
    {
        GameObject laserEffect = Instantiate(laserTrail.gameObject, laserPoint.position, Quaternion.identity);
        LineRenderer laserR = laserEffect.GetComponent<LineRenderer>();
        laserR.SetPosition(0, laserPoint.position);
        laserR.SetPosition(1, hitPoint);
        Destroy(laserEffect, 0.1f);
    }

    void LaserAttack()
    {
        if (!isAttacking)
        {
            RaycastHit hit;
            float range = 175f;
            laserPoint.rotation = Camera.main.transform.rotation;

            if (Physics.Raycast(laserPoint.position, laserPoint.transform.forward, out hit, range))
            {
                Debug.Log(hit.transform.name);

                if (!areEyesHot && (playerRb.velocity.magnitude == 0))
                {
                    Instantiate(laserPointEnd, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));

                    if (hit.collider.tag == "Enemy")
                        Instantiate(laserSpotLight, hit.point + new Vector3(0, 1.7f, 0), Quaternion.FromToRotation(Vector3.up, hit.normal));
                }


                if (!areEyesHot && (playerRb.velocity.magnitude == 0) && (laserButton.pressed))
                {
                    if (!isCrouching)
                        actions.Laser();

                    audioSource.PlayOneShot(laserSound, 0.05f);
                    if (hit.transform.GetComponent<HealthController>())
                    {
                        hit.transform.GetComponent<HealthController>().ApplyDamage(laserDamage);
                        Vector3 awayFromPlayer = (hit.transform.GetComponent<Transform>().position - transform.position);
                        //hit.transform.GetComponent<Rigidbody>().AddForce(awayFromPlayer * 500f * Time.deltaTime, ForceMode.Impulse);  //Push them back!
                        //hit.transform.GetComponent<Rigidbody>().AddRelativeForce(awayFromPlayer * 500f * Time.deltaTime, ForceMode.Impulse);
                        hit.transform.GetComponent<Rigidbody>().AddExplosionForce(1000f, transform.position, 150, 3f);
                    }

                    SpawnLaser(hit.point);
                    //Instantiate(laserPointer, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                    Instantiate(laserPointer, hit.point, Quaternion.identity);
                    StartCoroutine(HotEyes());
                }
                if (areEyesHot)
                {
                    StartCoroutine(ColdEyes());
                }
            }
        }
    }

    void Thumble()
    {
        if ((thumbleButton.pressed || Input.GetKey(KeyCode.Q)) && !isThumbling)
        {
            isThumbling = true;
            actions.Thumble();
            audioSource.PlayOneShot(thumbleSound, 0.8f);
            StartCoroutine(ThumbleTime());
        }

        if (isThumbling)
        {
            capsuleCollider.height = 0.7f;
            capsuleCollider.center = new Vector3(capsuleCollider.center.x, 0.35f, capsuleCollider.center.z);
            isCrouching = true;
            actions.Sitting(true);
        }
        // if (!isThumbling)
        // {
        //     capsuleCollider.height = 1.4f;
        //     capsuleCollider.center = new Vector3(capsuleCollider.center.x, 0.7f, capsuleCollider.center.z);
        // }
    }

    IEnumerator calmDown(float calmTime)
    {
        yield return new WaitForSeconds(calmTime);
        isAttacking = false;
        //if (swordCollider)
        swordTrail.Stop();
        isChangeable = true;
    }

    // IEnumerator CoPlayDelayedClip(float f)           //start delay walking sound after attack or thumle
    // {
    //     yield return new WaitForSeconds(0.5f);
    //     audioSource.PlayOneShot(walkingSound, f);
    // }

    void WhenDamaged()
    {
        if (health != playerHealth)
        {
            //actions.Damaged();
            health = playerHealth;
            getHitEffect.SetActive(true);
            StartCoroutine(GetHitEff());

        }
    }

    IEnumerator ThumbleTime()
    {
        yield return new WaitForSeconds(0.5f);
        isThumbling = false;
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
    }
    IEnumerator JumpSoundCd()
    {
        audioSource.PlayOneShot(jumpSound, 0.5f);
        yield return new WaitForSeconds(3);
    }
    IEnumerator HotEyes()
    {
        yield return new WaitForSeconds(0.5f);
        areEyesHot = true;
    }
    IEnumerator ColdEyes()
    {
        yield return new WaitForSeconds(2);
        areEyesHot = false;
    }

    IEnumerator GetHitEff()
    {
        yield return new WaitForSeconds(0.1f);
        getHitEffect.SetActive(false);
    }
    // IEnumerator BowAimingCancel()
    // {
    //     yield return new WaitForSeconds(1f);
    //     //actions.Aiming(false);
    //     //transform.Rotate(0, 180, 0);
    // }
}
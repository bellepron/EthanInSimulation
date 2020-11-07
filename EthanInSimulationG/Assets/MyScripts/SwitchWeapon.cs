using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchWeapon : MonoBehaviour
{
    PlayerController playerController;
    public FixedButton switchWeapon0;
    public FixedButton switchWeapon1;
    public FixedButton switchWeapon2;
    public FixedButton switchWeapon3;
    public FixedButton switchWeapon4;

    public AudioClip changingSound;
    AudioSource audioSource;

    private bool isChanging = false;
    public bool withSword = true;
    private bool isChangeable;


    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        isChangeable = GameObject.FindWithTag("Player").GetComponent<MyController>().isChangeable;

        if (isChangeable)
        {
            if ((!isChanging && switchWeapon0.pressed) || (Input.GetKey(KeyCode.Alpha1) && !isChanging))
            {
                withSword = true;
                isChanging = true;
                playerController.SetArsenal("Igne");
                audioSource.PlayOneShot(changingSound, 0.7f);
                StartCoroutine(calmDown());
            }
            else if ((!isChanging && switchWeapon1.pressed) || (Input.GetKey(KeyCode.Alpha2) && !isChanging))
            {
                withSword = false;
                isChanging = true;
                playerController.SetArsenal("DoubleIgne");
                audioSource.PlayOneShot(changingSound, 0.7f);
                StartCoroutine(calmDown());
            }
            else if ((!isChanging && switchWeapon2.pressed) || (Input.GetKey(KeyCode.Alpha3) && !isChanging))
            {
                withSword = true;
                isChanging = true;
                playerController.SetArsenal("Rifle");
                audioSource.PlayOneShot(changingSound, 0.7f);
                StartCoroutine(calmDown());
            }
            else if ((!isChanging && switchWeapon3.pressed) || (Input.GetKey(KeyCode.Alpha4) && !isChanging))
            {
                withSword = true;
                isChanging = true;
                playerController.SetArsenal("SphereIgne");
                audioSource.PlayOneShot(changingSound, 0.7f);
                StartCoroutine(calmDown());
            }
            else if ((!isChanging && switchWeapon4.pressed) || (Input.GetKey(KeyCode.Alpha5) && !isChanging))
            {
                withSword = true;
                isChanging = true;
                playerController.SetArsenal("DoubleSphere");
                audioSource.PlayOneShot(changingSound, 0.7f);
                StartCoroutine(calmDown());
            }
        }
    }

    IEnumerator calmDown()
    {
        yield return new WaitForSeconds(1);
        isChanging = false;
    }
}

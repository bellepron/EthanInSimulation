using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMove : MonoBehaviour
{
    private Transform shootPoint;
    private Vector3 targetDir;
    private float angleBetween;
    private float speed = 50f;
    private float powerFactor;
    private float pF;
    void Awake()
    {
        powerFactor = GameObject.FindWithTag("Player").GetComponent<MyController>().arrowPowerFactor;
    }

    void Update()
    {


        transform.position += transform.forward * Time.deltaTime * speed * powerFactor;


        Destroy(gameObject, 3f);
    }
}

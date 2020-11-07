using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballMove : MonoBehaviour
{
    private float speed = 20f;
    void Update()
    {
        transform.Translate(Camera.main.transform.position * Time.deltaTime * speed);
    }
}

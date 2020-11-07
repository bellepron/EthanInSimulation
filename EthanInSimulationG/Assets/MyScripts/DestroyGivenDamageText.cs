using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGivenDamageText : MonoBehaviour
{
    Transform point;

    void Start()
    {
        point = gameObject.GetComponent<Transform>();
    }
    void Update()
    {
        point.position += Vector3.up * Time.deltaTime * 10;
        Destroy(gameObject, 0.7f);
    }
}
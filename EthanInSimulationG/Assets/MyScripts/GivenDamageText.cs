using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GivenDamageText : MonoBehaviour
{
    private float disappearTimer = 1f;
    private TextMeshPro textMesh;
    private Color textColor;

    void Start()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
        textColor = textMesh.color;
    }

    void Update()
    {
        float moveYSpeed = 3f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;
        transform.Rotate(Vector3.up * Time.deltaTime * 100f);

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float disappearSpeed = 2f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;

            if (textColor.a < 0)
                Destroy(gameObject);
        }
    }
}

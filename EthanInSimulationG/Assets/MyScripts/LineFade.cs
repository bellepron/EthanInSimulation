using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineFade : MonoBehaviour
{
    [SerializeField] private Color color;
    [SerializeField] private float speed = 10f;
    LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        //move towards zero
        color.a = Mathf.Lerp(color.a, 0, Time.deltaTime * speed);

        //update color
        //lr.SetColors(color, color);
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }
}

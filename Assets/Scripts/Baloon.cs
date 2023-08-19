using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baloon : MonoBehaviour
{
    public Transform attachmentPoint;
    public GameObject player;
    private LineRenderer lineRenderer;
    private Vector3 relativePosition;
    private bool canUpdatePosition;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        canUpdatePosition = false;
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(delayedPositionChange());
    }

    IEnumerator delayedPositionChange()
    {
        yield return new WaitForSeconds(0.2f);

        if (player)
        {
            relativePosition = transform.position - player.transform.position;
        }
        canUpdatePosition = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (attachmentPoint)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, attachmentPoint.position);
        }

        if (player && canUpdatePosition)
        {
            transform.position = player.transform.position + relativePosition;
        }
    }
}

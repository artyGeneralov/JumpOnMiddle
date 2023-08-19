using System.Collections;
using UnityEngine;

public class Baloon : MonoBehaviour
{
    public Transform attachmentPoint;
    public GameObject player;
    private LineRenderer lineRenderer;
    private Vector3 relativePosition;
    private bool canUpdatePosition;

    void Start()
    {
        canUpdatePosition = false;
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

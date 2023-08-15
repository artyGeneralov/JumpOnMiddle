using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float thrust;
    [SerializeField] float rotSpeed = 5;
    [SerializeField] float maxVelocity;
    float curPos, lastPos;
    bool isScriptReady;

    void Start()
    {
        isScriptReady = false;
        StartCoroutine(StartWhenReady());
    }

    IEnumerator StartWhenReady()
    {
        while(rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
            yield return null;
        }
        curPos = lastPos = rb.position.x;
        isScriptReady = true;

    }


    void Update()
    {
        if (isScriptReady == false)
            return;

        //rb.rotation += rotSpeed;
        Vector3 jumpForce = new Vector3(thrust, thrust, 0);
        if (Input.GetKeyDown(KeyCode.W))
        {
            rb.AddForce(jumpForce, ForceMode2D.Impulse);
        }
        lastPos = curPos;


        ClampVelocity();
    }

    void ClampVelocity()
    {
        Vector2 currentVelocity = rb.velocity;

        if(currentVelocity.magnitude > maxVelocity)
        {
            rb.velocity = currentVelocity.normalized * maxVelocity;
        }
    }
}

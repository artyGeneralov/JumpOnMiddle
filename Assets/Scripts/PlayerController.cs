using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]Rigidbody2D rb;
    [SerializeField] float thrust;
    [SerializeField] float rotSpeed = 5;
    float curPos, lastPos;
    // Start is called before the first frame update
    void Start()
    {
        curPos = lastPos = rb.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        curPos = rb.position.x;
        float linSpeed = curPos - lastPos;
        linSpeed = Mathf.Pow(linSpeed, 10);
        Debug.Log("linspeed = " + linSpeed + "rotation " + rb.rotation);
        
        //rb.rotation += rotSpeed;
        Vector3 jumpForce = new Vector3(thrust,thrust, 0);
        if (Input.GetKeyDown(KeyCode.Space))
            rb.AddForce(jumpForce, ForceMode2D.Impulse);
        lastPos = curPos;
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{

    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, 
                                                    player.transform.position.y, 
                                                    mainCamera.transform.position.z);

    }
}

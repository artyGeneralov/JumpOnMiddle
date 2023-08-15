using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScaler : MonoBehaviour
{
    float startPoint;
    float endPoint;
    [SerializeField] GameObject platform;
    void Start()
    {
        platform = GameObject.FindGameObjectWithTag("Platform");
        startPoint = transform.position.y;
        endPoint = platform.transform.position.y;
        float height = endPoint - startPoint;
        transform.position = new Vector3(transform.position.x, height / 2, transform.position.z);
        transform.localScale = new Vector3(transform.localScale.x, height, transform.localScale.z);
    }


}

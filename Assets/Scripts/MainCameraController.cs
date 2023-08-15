using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    
    [SerializeField] float maxZoomOut;
    [SerializeField] float zoomOutSpeed;
    [SerializeField] float zoomInSpeed;
    [SerializeField] float zoomOutPositionChange;
    [SerializeField] float orginalZoom;
    [SerializeField] float playerFollowSpeed;
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject player;
    [SerializeField] int scaleSizeOnZoom;
    public float originalAspect { get; private set; }
    public float originalOrthographicSize { get; private set; }

    public float currentCameraPositionY { get; private set; }
    public float currentCameraPositionX { get; private set; }

    ScaleController scale;
    [HideInInspector] public int originalScaleSize;


    void Start()
    {
        scale = FindObjectOfType<ScaleController>();
        originalScaleSize = scale.visibleScaleSize;
        originalAspect = mainCamera.aspect;
        originalOrthographicSize = mainCamera.orthographicSize;
        currentCameraPositionY = mainCamera.transform.position.y;
        currentCameraPositionX = mainCamera.transform.position.x;
    }

    void Update()
    {

        // follow player
        

        currentCameraPositionY = mainCamera.transform.position.y;

        if (Input.GetMouseButton(1))
        {

            ZoomOut();
        }
        else
        {

            ZoomIn();
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x,
                        Mathf.MoveTowards(mainCamera.transform.position.y, player.transform.position.y, playerFollowSpeed * Time.deltaTime),
                        mainCamera.transform.position.z);
        }
        FollowPlayer();

    }

    public void FollowPlayer()
    {
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x,
            Mathf.MoveTowards(mainCamera.transform.position.y, player.transform.position.y, playerFollowSpeed * Time.deltaTime),
            mainCamera.transform.position.z);
    }


    public void ZoomOut()
    {
        mainCamera.orthographicSize = Mathf.MoveTowards(mainCamera.orthographicSize, maxZoomOut, zoomOutSpeed * Time.deltaTime);
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x,
                                Mathf.MoveTowards(mainCamera.transform.position.y, player.transform.position.y + zoomOutPositionChange, zoomOutSpeed * Time.deltaTime),
                                mainCamera.transform.position.z);
        
        scale.visibleScaleSize = scaleSizeOnZoom;
    }

    public void ZoomIn()
    {
        mainCamera.orthographicSize = Mathf.MoveTowards(mainCamera.orthographicSize, orginalZoom, zoomInSpeed * Time.deltaTime);
        if(mainCamera.orthographicSize == orginalZoom)
        {
            scale.visibleScaleSize = originalScaleSize;
        }
    }
}

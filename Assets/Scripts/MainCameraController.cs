
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

    InputChannel inputChannel;
    [HideInInspector] public int originalScaleSize;

    bool shouldZoomOut = false;

    void Start()
    {
        scale = FindObjectOfType<ScaleController>();
        originalScaleSize = scale.visibleScaleSize;
        originalAspect = mainCamera.aspect;
        originalOrthographicSize = mainCamera.orthographicSize;
        currentCameraPositionY = mainCamera.transform.position.y;
        currentCameraPositionX = mainCamera.transform.position.x;
        addListeners();
    }

    private void addListeners()
    {
        var bacon = FindObjectOfType<Beacon>();
        inputChannel = bacon.inputChannel;
        inputChannel.ZoomOutEvent += HandleZoomOut;
        inputChannel.ZoomInEvent += HandleZoomIn;
    }
    private void HandleZoomOut()
    {
        shouldZoomOut = true;
    }
    private void HandleZoomIn()
    {
        shouldZoomOut = false;
    }

    void Update()
    {
        currentCameraPositionY = mainCamera.transform.position.y;
        if (shouldZoomOut)
        {

            ZoomOut();
        }
        else
        {

            ZoomIn();
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x,
                        Mathf.MoveTowards(mainCamera.transform.position.y, player.transform.position.y, playerFollowSpeed * Time.deltaTime),
                        mainCamera.transform.position.z);
        FollowPlayer();
        }
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

    private void OnDestroy()
    {
        inputChannel.ZoomOutEvent -= HandleZoomOut;
        inputChannel.ZoomInEvent -= HandleZoomIn;
    }
}

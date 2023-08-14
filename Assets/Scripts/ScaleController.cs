using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleController : MonoBehaviour
{
    [SerializeField] GameObject smallNotchPrefab;
    [SerializeField] GameObject largeNotchPrefab;
    [SerializeField] Camera mainCamera;
    [SerializeField] float largeNotchX, smallNotchX;
    [SerializeField] int firstInterval;
    [SerializeField] int smallNotchesInterval;
    [SerializeField] int largeNotchesInterval;
    int smallNotchesToRender = 10;
    int largeNotchesToRender = 5;
    // Start is called before the first frame update
    List<GameObject> activeNotches = new List<GameObject>();
    ObjectPool smallNotchPool;
    ObjectPool largeNotchPool;

    float cameraHeight, cameraWidth;
    float cameraMiddle;
    int highestRendered, lowestRendered;


    void Start()
    {
        smallNotchPool = new ObjectPool(smallNotchPrefab, smallNotchesToRender);
        largeNotchPool = new ObjectPool(largeNotchPrefab, largeNotchesToRender);

        cameraHeight = 2f * mainCamera.orthographicSize;
        cameraWidth = cameraHeight * mainCamera.aspect;
        highestRendered = lowestRendered = 0;
    }

    void RenderNotches()
    {

        // render up
        for (int pos = highestRendered; pos < Mathf.Round(cameraMiddle + cameraHeight); pos++)
            if(pos % largeNotchesInterval == 0 || pos % smallNotchesInterval == 0)
                RenderSingleNotch(pos);


        // render down
        for (int pos = lowestRendered; pos > Mathf.Round(cameraMiddle - cameraHeight); pos--)
            if (pos % largeNotchesInterval == 0 || pos % smallNotchesInterval == 0)
                RenderSingleNotch(pos);
        highestRendered = (int)Mathf.Round(cameraMiddle + cameraHeight);
        lowestRendered = (int)Mathf.Round(cameraMiddle - cameraHeight);


        // remove the ones off screen
        for (int i = activeNotches.Count - 1; i >= 0; i--)
        {
            GameObject notch = activeNotches[i];
            float notchMiddle = notch.transform.position.y;
            if (notchMiddle > cameraMiddle + cameraHeight || notchMiddle < cameraMiddle - cameraHeight)
            {
                if (notchMiddle % largeNotchesInterval == 0)
                {
                    largeNotchPool.DeactivateAndPool(notch);
                }
                else
                {
                    smallNotchPool.DeactivateAndPool(notch);
                }
                activeNotches.RemoveAt(i);
            }
        }
    }

    void RenderSingleNotch(int pos)
    {
        GameObject notch;
        if (pos % largeNotchesInterval == 0)
        {
            notch = largeNotchPool.GetPooledObject();
            notch.transform.position = new Vector3(largeNotchX, pos, 0);
        }
        else
        {
            notch = smallNotchPool.GetPooledObject();
            notch.transform.position = new Vector3(smallNotchX, pos, 0);
        }

        TMPro.TextMeshPro txt = notch.GetComponentInChildren<TMPro.TextMeshPro>();
        // set notch text
        txt.text = pos.ToString();
        activeNotches.Add(notch);
    }




    // Update is called once per frame
    void Update()
    {

        cameraMiddle = mainCamera.transform.position.y;
        RenderNotches();
    }
}

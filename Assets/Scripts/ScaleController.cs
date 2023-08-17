using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleController : MonoBehaviour
{
    [SerializeField] GameObject smallNotchPrefab;
    [SerializeField] GameObject largeNotchPrefab;
    [SerializeField] MainCameraController mainCamera;
    [SerializeField] float largeNotchX, smallNotchX;


    [SerializeField] int smallNotchesInterval;
    [SerializeField] int largeNotchesInterval;


    /*Serialized:*/ public int visibleScaleSize;


    int defaultPoolSize = 20;
    NotchPool notchPool;
    float cameraMiddle;
    int highestRendered, lowestRendered;
    int lowerBound, upperBound;


    void Start()
    {
        GetCurrentCameraPosition();
        notchPool = new NotchPool(largeNotchPrefab, smallNotchPrefab, defaultPoolSize);
        setCurrentBounds();
        lowestRendered = highestRendered = lowerBound;
        
    }

    void Update()
    {
        setCurrentBounds();
        DrawNotches();
        DestroyInvisibleNotches();
    }

    void setCurrentBounds()
    {
        GetCurrentCameraPosition();
        lowerBound = (int)Mathf.Max(0, cameraMiddle - visibleScaleSize);
        upperBound = (int)(cameraMiddle + visibleScaleSize);
    }

    void DrawNotches()
    {

        for (int pos = lowerBound; pos <= upperBound; pos++)
        {

            if(pos >= lowestRendered && pos <= highestRendered) { continue; }
            if(pos % largeNotchesInterval == 0)
            {
                GameObject largeNotch = notchPool.Get(NotchPool.PrefabType.LargeNotch);
                largeNotch.transform.position = new Vector3(largeNotchX, pos, largeNotch.transform.position.z);
                TMPro.TextMeshPro txt = largeNotch.GetComponentInChildren<TMPro.TextMeshPro>();
                txt.text = pos.ToString();
            }
            else if(pos % smallNotchesInterval == 0)
            {
                GameObject smallNotch = notchPool.Get(NotchPool.PrefabType.SmallNotch);
                smallNotch.transform.position = new Vector3(smallNotchX, pos, smallNotch.transform.position.z);
                TMPro.TextMeshPro txt = smallNotch.GetComponentInChildren<TMPro.TextMeshPro>();
                txt.text = pos.ToString();
            }
        }
        lowestRendered = lowerBound;
        highestRendered = upperBound;

    }


    void DestroyInvisibleNotches()
    {
        List<GameObject> notchesToRelease = new List<GameObject>();
        notchPool.ForEachActiveNotch((notch) => {
            if (notch.transform.position.y > upperBound || notch.transform.position.y < lowerBound)
                notchesToRelease.Add(notch);
        });

        foreach (GameObject notch in notchesToRelease)
        {
            notchPool.Release(notch);
        }
    }


    void GetCurrentCameraPosition()
    {
        cameraMiddle = mainCamera.currentCameraPositionY;
    }

}

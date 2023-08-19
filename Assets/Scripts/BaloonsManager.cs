using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaloonsManager : MonoBehaviour
{
    [SerializeField] GameObject baloonPrefab;
    [SerializeField] GameObject player;
    [SerializeField] List<Transform> attachmentPoints;
    [SerializeField] public readonly int maxBaloons = 3;
    List<GameObject> instantiatedBaloons;

    void Start()
    {
        instantiatedBaloons = new List<GameObject>();
        InstantiateBaloons();
    }


    public void AddBaloon()
    {
        if(instantiatedBaloons.Count < maxBaloons)
        {
            Transform attachmentPoint = attachmentPoints[instantiatedBaloons.Count];
            GameObject newBaloon = Instantiate(baloonPrefab, attachmentPoint.position, Quaternion.identity, player.transform);
            newBaloon.transform.position += Vector3.up * 2.5f;

            Baloon baloonScript = newBaloon.GetComponent<Baloon>();
            if (baloonScript)
            {
                baloonScript.attachmentPoint = attachmentPoint;
            }

            instantiatedBaloons.Add(newBaloon);
        }
    }

    public void RemoveBaloon()
    {
        if(instantiatedBaloons.Count > 0)
        {
            GameObject baloonToRemove = instantiatedBaloons[instantiatedBaloons.Count - 1];
            instantiatedBaloons.RemoveAt(instantiatedBaloons.Count - 1);
            Destroy(baloonToRemove);
        }

        // TODO: add pop animation
    }

    public void removeAllBaloons()
    {
        for(int i = instantiatedBaloons.Count - 1; i >= 0; i--)
        {
            GameObject baloonToRemove = instantiatedBaloons[i];
            instantiatedBaloons.RemoveAt(i);
            Destroy(baloonToRemove);
        }
    }

    public int GetCurrentBaloonCount()
    {
        return instantiatedBaloons.Count;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
    }

    private void InstantiateBaloons()
    {
        for(int i = 0; i < maxBaloons; i++)
        {
            AddBaloon();
        }
    }
}

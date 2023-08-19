using System;
using UnityEngine;

public class ObsticleEventController : MonoBehaviour
{

    public event Action touchedPlayer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            touchedPlayer?.Invoke();
            Destroy(this.gameObject);
        }
    }
}

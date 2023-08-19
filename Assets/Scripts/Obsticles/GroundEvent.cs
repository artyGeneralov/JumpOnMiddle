using System;
using UnityEngine;

public class GroundEvent : MonoBehaviour
{
    public event Action touchedPlayer;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            touchedPlayer?.Invoke();
        }
    }
}

using System;
using UnityEngine;


public class PowerUpItem : MonoBehaviour
{
    public static event Action OnTouchedPlayer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnTouchedPlayer?.Invoke();
    }
}

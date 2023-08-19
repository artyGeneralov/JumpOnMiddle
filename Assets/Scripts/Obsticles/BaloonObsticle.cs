using UnityEngine;
using System;

public class BaloonObsticle : MonoBehaviour
{
    public static event Action OnTouchedPlayer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnTouchedPlayer?.Invoke();
    }
}

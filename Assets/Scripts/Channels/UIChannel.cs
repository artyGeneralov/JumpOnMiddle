using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UI Channel", menuName = "Channels/UI Channel", order = 1)]
public class UIChannel : ScriptableObject
{
    public Action HideInstructionEvent;
    public Action ShowEndScreenEvent;
    public Action<int> UpdateSpeedEvent;
    public Action ShowSpeedCounterEvent;

    public void HideInstructionUI()
    {
        HideInstructionEvent?.Invoke();
    }

    public void ShowEndScreenUI()
    {
        ShowEndScreenEvent?.Invoke();
    }

    public void UpdateSpeed(int speed)
    {
        UpdateSpeedEvent?.Invoke(speed);
    }

    public void ShowSpeedCounter()
    {
        ShowSpeedCounterEvent?.Invoke();
    }

}

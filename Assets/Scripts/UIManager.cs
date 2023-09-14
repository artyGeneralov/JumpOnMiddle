using System.Text;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] GameObject instructionsUI;
    [SerializeField] TMPro.TextMeshProUGUI currentSpeedUI;
    [SerializeField] TMPro.TextMeshProUGUI endScreenUI;
    [SerializeField] GameObject currentSpeedPanel;
    [SerializeField] GameObject endGamePanel;
    string speedPrefix = "Current Speed: ";
    UIChannel uiChannel;

    private void Start()
    {
        instructionsUI.SetActive(true);
        currentSpeedPanel.SetActive(false);
        endGamePanel.SetActive(false);
        var bacon = FindObjectOfType<Beacon>();
        uiChannel = bacon.UIChannel;
        uiChannel.HideInstructionEvent += HideInstructions;
        uiChannel.ShowEndScreenEvent += SpawnEndGameSummary;
        uiChannel.UpdateSpeedEvent += updateSpeed;
        uiChannel.ShowSpeedCounterEvent += SpawnSpeedCounter;
    }

    public void SpawnInstruction()
    {
        instructionsUI.SetActive(true);
    }

    public void HideInstructions()
    {
        instructionsUI.SetActive(false);
    }

    public void SpawnSpeedCounter()
    {
        currentSpeedPanel.SetActive(true);
    }

    public void HideSpeedCounter()
    {
        currentSpeedPanel.SetActive(false);
    }

    public void SpawnEndGameSummary()
    {
        string speedText = currentSpeedUI.text;
        int finalVelocity = int.Parse(speedText.Substring(speedText.IndexOf(speedPrefix) + speedPrefix.Length));

        HideSpeedCounter();
        StringBuilder sb = new StringBuilder();
        sb.Append("You've reached the ground with a speed of: ");
        sb.Append(finalVelocity);
        sb.Append("\n\nCan you do better ? ");
        currentSpeedPanel.SetActive(false);
        endGamePanel.SetActive(true);
        endScreenUI.text = sb.ToString();
    }

    public void HideEndGameSummary()
    {
        endScreenUI.alpha = 0f;
    }

    public void updateSpeed(int speed)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(speedPrefix);
        sb.Append(speed);
        currentSpeedUI.text = sb.ToString();
    }

    private void OnDestroy()
    {
        uiChannel.HideInstructionEvent -= HideInstructions;
        uiChannel.ShowEndScreenEvent -= SpawnEndGameSummary;
        uiChannel.UpdateSpeedEvent -= updateSpeed;
        uiChannel.ShowSpeedCounterEvent -= SpawnSpeedCounter;
    }
}

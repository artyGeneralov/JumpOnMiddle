using System.Text;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] GameObject instructionsUI;
    [SerializeField] TMPro.TextMeshProUGUI currentSpeedUI;
    [SerializeField] TMPro.TextMeshProUGUI endScreenUI;
    [SerializeField] GameObject currentSpeedPanel;
    [SerializeField] GameObject endGamePanel;

    private void Start()
    {
        instructionsUI.SetActive(true);
        currentSpeedPanel.SetActive(false);
        endGamePanel.SetActive(false);
        
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

    public void SpawnEndGameSummary(int finalVelocity)
    {
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
        sb.Append("Current Speed: ");
        sb.Append(speed);
        currentSpeedUI.text = sb.ToString();
    }
}

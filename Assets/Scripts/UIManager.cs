using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] GameObject instructionsUI;
    [SerializeField] TMPro.TextMeshProUGUI currentSpeedUI;
    [SerializeField] TMPro.TextMeshProUGUI endScreenUI;
    [SerializeField] TMPro.TextMeshProUGUI InvCounter;
    [SerializeField] GameObject invPanel;
    [SerializeField] GameObject currentSpeedPanel;
    [SerializeField] GameObject endGamePanel;
    [SerializeField] int InvulnurabilityDepletionSpeed = 1;

    string speedPrefix = "Current Speed: ";
    UIChannel uiChannel;

    private void Start()
    {
        instructionsUI.SetActive(true);
        currentSpeedPanel.SetActive(false);
        endGamePanel.SetActive(false);
        addListeners();
    }


    private void addListeners()
    {
        var bacon = FindObjectOfType<Beacon>();
        uiChannel = bacon.UIChannel;
        uiChannel.HideInstructionEvent += HideInstructions;
        uiChannel.ShowEndScreenEvent += SpawnEndGameSummary;
        uiChannel.UpdateSpeedEvent += UpdateSpeed;
        uiChannel.ShowSpeedCounterEvent += SpawnSpeedCounter;
        uiChannel.UpdateInvEvent += UpdateInv;
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

    public void SpawnInv()
    {
        invPanel.SetActive(true);
    }

    public void HideInv()
    {
        invPanel.SetActive(false);
    }

    public void UpdateInv(int amount)
    {
            Slider slider = invPanel.GetComponentInChildren<Slider>();
            slider.value = amount / 100f;
            InvCounter.text = amount.ToString();
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

    public void UpdateSpeed(int speed)
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
        uiChannel.UpdateSpeedEvent -= UpdateSpeed;
        uiChannel.ShowSpeedCounterEvent -= SpawnSpeedCounter;
    }
}

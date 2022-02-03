using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject StartButton;
    [SerializeField]
    GameObject ExcavationScreen;
    [SerializeField]
    GameObject ResultsScreen;

    public TextMeshProUGUI ScanToggleText;
    public TextMeshProUGUI ExtractedValueText;
    public TextMeshProUGUI DialogBox;
    public TextMeshProUGUI ResultsText;
    public TextMeshProUGUI ScansText;
    public TextMeshProUGUI ExtractText;

    public ExcavationManager excManager;

    private void Awake()
    {
        excManager.ExtractedValueUpdated.AddListener(UpdateExtractedValueText);
        excManager.FinishedExcavation.AddListener(ShowResults);
        excManager.ChangeMode.AddListener(ChangeToMode);
        excManager.ScanUsed.AddListener(SetScansLeft);
        excManager.ExtractUsed.AddListener(SetExtractsLeft);
    }

    public void StartExcavation()
    {
        StartButton.SetActive(false);
        ExcavationScreen.SetActive(true);

        excManager.ResetValues();
    }

    public void FinishExcavation()
    {
        StartButton.SetActive(true);
        ExcavationScreen.SetActive(false);
    }

    public void ToggleMode()
    {
        excManager.IsScanning = !excManager.IsScanning;

        ChangeToMode(excManager.IsScanning);
    }

    private void ChangeToMode(bool scanning)
    {
        if (scanning)
        {
            ScanToggleText.text = "Scan Mode";
            DialogBox.text = "Switched to Scan Mode!";
        }
        else
        {
            ScanToggleText.text = "Excavation Mode";
            DialogBox.text = "Switched to Excavation Mode!";
        }
    }

    private void UpdateExtractedValueText(int valueGained)
    {
        ExtractedValueText.text = excManager.extractedValue.ToString();
        DialogBox.text = "You obtained " + valueGained.ToString() + " Extractium Ore!";
    }

    private void ShowResults(int finalValue)
    {
        ResultsText.text = "You earned a grand total of " + finalValue.ToString() + " Extractium!";
        ResultsScreen.SetActive(true);
    }

    private void SetScansLeft(int left)
    {
        ScansText.text = left.ToString();
    }

    private void SetExtractsLeft(int left)
    {
        ExtractText.text = left.ToString();
    }
}

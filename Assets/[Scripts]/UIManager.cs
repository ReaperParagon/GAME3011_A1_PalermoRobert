using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Screens / Buttons")]
    [SerializeField]
    GameObject StartButton;
    [SerializeField]
    GameObject ExcavationScreen;
    [SerializeField]
    GameObject ResultsScreen;

    [Header("UI Texts")]
    [SerializeField]
    private TextMeshProUGUI ScanToggleText;
    [SerializeField]
    private TextMeshProUGUI ExtractedValueText;
    [SerializeField]
    private TextMeshProUGUI DialogBox;
    [SerializeField]
    private TextMeshProUGUI ResultsText;
    [SerializeField]
    private TextMeshProUGUI ScansText;
    [SerializeField]
    private TextMeshProUGUI ExtractText;

    [Header("Excavation Manager")]
    [SerializeField]
    private ExcavationManager excManager;

    private void Awake()
    {
        if (excManager == null)
            excManager = FindObjectOfType<ExcavationManager>();

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

    private void UpdateExtractedValueText(int totalValue)
    {
        ExtractedValueText.text = totalValue.ToString();
    }

    private void ShowResults(int finalValue)
    {
        ResultsText.text = "You earned a grand total of " + finalValue.ToString() + " Extractium Ore!";
        ResultsScreen.SetActive(true);
    }

    private void SetScansLeft(int left)
    {
        ScansText.text = left.ToString();
        DialogBox.text = "You have " + left.ToString() + " Scans remaining!";
    }

    private void SetExtractsLeft(int left, int valueGained)
    {
        ExtractText.text = left.ToString();
        DialogBox.text = "You obtained " + valueGained.ToString() + " Extractium Ore! " + left.ToString() + " Extractions remaining!";
    }
}

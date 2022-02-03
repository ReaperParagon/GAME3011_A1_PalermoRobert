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

    public TextMeshProUGUI ScanToggleText;
    public TextMeshProUGUI ExtractedValueText;
    public TextMeshProUGUI DialogBox;

    public ExcavationManager excManager;

    private void Awake()
    {
        excManager.ExtractedValueUpdated.AddListener(UpdateExtractedValueText);
    }

    public void StartExcavation()
    {
        StartButton.SetActive(false);
        ExcavationScreen.SetActive(true);

        // TODO: Reset excavation screen
    }

    public void FinishExcavation()
    {
        StartButton.SetActive(true);
        ExcavationScreen.SetActive(false);
    }

    public void ToggleMode()
    {
        excManager.IsScanning = !excManager.IsScanning;

        if (excManager.IsScanning)
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
}

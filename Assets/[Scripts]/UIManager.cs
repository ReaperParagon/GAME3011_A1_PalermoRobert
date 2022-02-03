using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject StartButton;
    [SerializeField]
    GameObject ExcavationScreen;

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
}

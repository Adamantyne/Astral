using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPhaseController : MonoBehaviour
{
    [SerializeField] private int phase;
    [SerializeField] private Button currentButton;

    void Start()
    {
        int _phaseCompleted = PhasesCache.PhasesCacheInstance.GetPhaseCompleted();
        if(phase>_phaseCompleted+1 && phase>1){
            currentButton.interactable = false;
        }
    }
}

using UnityEngine;

public class PhasesCache : MonoBehaviour
{
    public static PhasesCache PhasesCacheInstance;
    void Awake()
    {
        PhasesCacheInstance = this;
    }

    public int GetPhaseCompleted()
    {
        return PlayerPrefs.GetInt("phaseCompleted");
    }

    public void SetPhaseCompleted(int _completedStage)
    {
        if(GetPhaseCompleted() > _completedStage) return;
        PlayerPrefs.SetInt("phaseCompleted", _completedStage);
        PlayerPrefs.Save();
    }

    public bool ComparePhases(int _phase)
    {
        return _phase >= GetPhaseCompleted();
    }
}

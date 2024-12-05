using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptionScript : MonoBehaviour
{
    [SerializeField] int corruptionLevel;
    
    public void SetCorruptionLevel(int _newLevel)
    {
        corruptionLevel = _newLevel;
    }

    public int GetCorruptionLevel()
    {
        return corruptionLevel;
    }

    public void AddToCorruptionLevel(int _value)
    {
        corruptionLevel += _value;
    }

    public bool EvaluateCorruption(int _threshhold)
    {
        bool isCorrupt = false;

        if(corruptionLevel >= _threshhold)
            isCorrupt = true;

        return isCorrupt;
    }

    public bool IsMaxed(int _max)
    {
        bool isMaxed = false;

        if (corruptionLevel == _max)
            isMaxed = true;

        return isMaxed;
    }
}

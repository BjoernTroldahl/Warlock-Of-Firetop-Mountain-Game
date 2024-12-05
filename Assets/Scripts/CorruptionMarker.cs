using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CorruptionMarker : Marker, INotification
{
    public int corruptionThreshhold;

    public PropertyName id => throw new System.NotImplementedException();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

public class SceneSwitchMarker : Marker, INotification
{
    public int scene;
    public PropertyName id => throw new System.NotImplementedException();
}

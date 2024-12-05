using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SignalReceiver))]
public class SceneSwitchBridge : MonoBehaviour, INotificationReceiver
{
    public void OnNotify(Playable origin, INotification notification, object context)
    {
        SceneSwitchMarker sceneM = notification as SceneSwitchMarker;

        if (sceneM != null)
        {
            Debug.Log("Loading scene " + sceneM.scene);
            SceneManager.LoadScene(sceneM.scene);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[RequireComponent(typeof(SignalReceiver))]
public class CorruptionBridge : MonoBehaviour, INotificationReceiver
{
    [SerializeField] CorruptionScript cs;

    [SerializeField] Cutscene controller;

    public PlayableDirector targetTimeline_belowThreshold;
    public PlayableDirector targetTimeline_aboveThreshold;

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        CorruptionMarker cm = notification as CorruptionMarker;

        if (cm != null)
        {
            PlayableDirector targetTimeline;

            if (cs.EvaluateCorruption(cm.corruptionThreshhold))
            {
                targetTimeline = targetTimeline_aboveThreshold;
            }
            else
            {
                targetTimeline = targetTimeline_belowThreshold;
            }

            controller.RunChoice(targetTimeline);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cs = GameObject.FindGameObjectWithTag("Player").GetComponent<CorruptionScript>();

        controller = GameObject.FindGameObjectWithTag("Timeline").GetComponent<Cutscene>();
    }
}

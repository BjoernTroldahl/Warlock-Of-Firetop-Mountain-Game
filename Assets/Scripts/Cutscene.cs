using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class Cutscene : MonoBehaviour
{
    [Header("Cutscene controller")]
    [SerializeField] PlayableDirector currentDirector;

    public void setDirector(PlayableDirector _director)
    {
        currentDirector = _director;
    }

    public PlayableDirector getDirector()
    {
        return currentDirector;
    }

    void Awake()
    {
        StartTimeline();
    }

    public void StartTimeline()
    {
        currentDirector.Resume();
        currentDirector.Play();
    }

    public void RunChoice(PlayableDirector scene)
    {
        currentDirector.Pause();

        currentDirector = scene;

        currentDirector.Play();
    }
}
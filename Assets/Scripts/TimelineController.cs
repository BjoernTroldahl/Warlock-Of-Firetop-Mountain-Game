using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    [SerializeField] PlayableDirector choice_one;
    [SerializeField] PlayableDirector choice_two;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public PlayableDirector getChoiceOne()
    {
        return choice_one;
    }

    public PlayableDirector getChoiceTwo()
    {
        return choice_two;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

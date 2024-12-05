using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerChoiceControlAsset : PlayableAsset
{
    public string choiceOne;
    public string choiceTwo;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        // Create two button objects with the strings of choice one and two

        // Return a value based on the users choice

        // Take value and apply it to the state based animator to change scene
        throw new System.NotImplementedException();
    }
}

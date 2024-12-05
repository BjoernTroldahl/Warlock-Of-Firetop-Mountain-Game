using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.Timeline;
// Required packages: UnityEngine.UI, UnityEngine.Playables and UnityEngine.Timeline.

// Automatically attaches a SignalReceiver to the Game Object when the script is added
[RequireComponent(typeof(SignalReceiver))]
// Takes from the MonoBehaviour base class, and the INotificationReceiver interface
public class ButtonBridge : MonoBehaviour, INotificationReceiver
{
    // Buttons
    public Button leftButton;
    public Button rightButton;

    // Timelines to go to
    public PlayableDirector targetTimeline_Left;
    public PlayableDirector targetTimeline_Right;

    // Get the buttons automatically, based on the tags of the buttons.
    private void Start()
    {
        leftButton = GameObject.FindGameObjectWithTag("LeftButton").GetComponent<Button>();
        rightButton = GameObject.FindGameObjectWithTag("RightButton").GetComponent<Button>();
    }

    // Gets run when the script receives a ding from an INotification, I.E. our ButtonMarker.cs script
    public void OnNotify(Playable origin, INotification notification, object context)
    {
        // Interprets the notification as a ButtonMarker
        ButtonMarker bm = notification as ButtonMarker;

        // Makes sure it is not a missfire notification, incase something goes wrong.
        if (bm != null)
        {
            // Changes the text in the left and right button, as well as enabling the buttons to the user
            leftButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = bm.buttons[0].buttonText;
            leftButton.gameObject.SetActive(true);
            rightButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = bm.buttons[1].buttonText;
            rightButton.gameObject.SetActive(true);

            // Applies the corruption to the buttons ButtonScript
            leftButton.GetComponent<ButtonScript>().corruption = bm.buttons[0].choiceCorruption;
            rightButton.GetComponent<ButtonScript>().corruption = bm.buttons[1].choiceCorruption;

            // Applies the timeline each button is supposed to go to, to the buttons ButtonScript
            leftButton.GetComponent<ButtonScript>().targetTimeline = targetTimeline_Left;
            rightButton.GetComponent<ButtonScript>().targetTimeline = targetTimeline_Right;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System.IO;
using TMPro;
// Needs playables to access buttons
// Needs System.IO to write to files
// Needs TMPro to access TextMeshPro objects

public class ButtonScript : MonoBehaviour
{
    // corruption from getting clicked
    [HideInInspector]
    public int corruption;

    // Timeline to go to
    [HideInInspector]
    public PlayableDirector targetTimeline;

    // corruptionscript
    CorruptionScript corruptionScript;
    // controller
    Cutscene controller;

    // file to save to
    string fileName = Application.streamingAssetsPath + "/TextFiles/" + "data.txt";

    public void Start()
    {
        // Finds the corruption script
        corruptionScript = GameObject.FindGameObjectWithTag("Player").GetComponent<CorruptionScript>();

        // finds the cutscene contrller
        controller = GameObject.FindGameObjectWithTag("Timeline").GetComponent<Cutscene>();

        //Starts delay
        StartCoroutine(D_Start());
    }

    // Add a delay to the button getting hidden, else it might mess up and hide the buttons before every timeline gets the button game object
    IEnumerator D_Start()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    // Function for when the button gets pressed
    public void OnPress()
    {
        // Adds to the corruption level
        corruptionScript.AddToCorruptionLevel(corruption);

        // Tells the controller to run the timeline of the button
        controller.RunChoice(targetTimeline);

        // Hides both buttons, because the other also needs to be hidden when one gets pressed
        GameObject.FindGameObjectWithTag("LeftButton").SetActive(false);
        GameObject.FindGameObjectWithTag("RightButton").SetActive(false);

        // Text to be written to file, to get the users' choices throughout the experience
        string textToWrite = gameObject.GetComponentInChildren<TextMeshProUGUI>().text;

        // add it to a file
        File.AppendAllText(fileName, textToWrite + "\n");
    }
}

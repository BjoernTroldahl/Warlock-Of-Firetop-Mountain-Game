using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class RadioScript : MonoBehaviour
{
    string fileName;
    string path = "/TextFiles/";

    [SerializeField] GameObject input;

    ToggleGroup toggleGroup;

    [SerializeField] StartButton sb;
    [SerializeField] GameObject[] gbToHide = new GameObject[2];

    // Start is called before the first frame update
    void Start()
    {
        Directory.CreateDirectory(Application.streamingAssetsPath + path);
        toggleGroup = GetComponent<ToggleGroup>();
    }

    // Update is called once per frame
    public void Submit()
    {
        string participantID = input.GetComponent<TMP_InputField>().text;
        Debug.Log(participantID);
        Toggle toggle = toggleGroup.ActiveToggles().FirstOrDefault();
        string toggleText = toggle.name;
        Debug.Log(toggleText);

        if(!toggleText.Equals(""))
        {
            //fileName = Application.streamingAssetsPath + path + participantID + "_" + toggleText + ".txt";
            fileName = Application.streamingAssetsPath + path + "data.txt";

            string textToWrite = "ParticipantID: " + participantID + "\nCondition: " + toggle.GetComponentInChildren<Text>().text;

            if (!File.Exists(fileName))
            {
                File.WriteAllText(fileName, "");
            }

            File.AppendAllText(fileName, textToWrite + "\n");

            sb.gameObject.SetActive(true);
            if (toggleText.Equals("Option 1"))
            {
                sb.scene = 1;
                //SceneManager.LoadScene(1);
            } else if (toggleText.Equals("Option 2"))
            {
                sb.scene = 2;
                //SceneManager.LoadScene(2);
            }

            foreach (GameObject i in gbToHide)
            {
                i.SetActive(false);
            }
        }
    }
}

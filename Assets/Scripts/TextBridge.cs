using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public class TextBridge : MonoBehaviour, INotificationReceiver
{
    [Header("Text Settings")]
    [TextArea] [SerializeField] private string[] itemInfo = new string[1];
    [SerializeField] private float textSpeed = 0.01f;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI itemInfoText;
    private int currentDisplayingText = 0;

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        TextMarker bm = notification as TextMarker;

        if (bm != null)
        {
            itemInfo[0] = bm.text;

            ActivateText();
        }
    }

    public void ActivateText()
    {
        //Start Coroutine
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        for (int i = 0; i < itemInfo[currentDisplayingText].Length + 1; i++)
        {
            itemInfoText.text = itemInfo[currentDisplayingText].Substring(0, i);
            yield return new WaitForSeconds(textSpeed);
        }
    }
}

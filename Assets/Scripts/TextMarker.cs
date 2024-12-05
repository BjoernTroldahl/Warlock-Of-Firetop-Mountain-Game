using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using TMPro;

public class TextMarker : Marker, INotification
{
    [TextArea(15, 20)]
    public string text = "";

    public PropertyName id { get { return new PropertyName(); } }
}
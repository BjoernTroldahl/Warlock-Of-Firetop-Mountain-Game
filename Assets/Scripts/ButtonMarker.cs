using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
// The two UnityEngine packages "Playables" and "Timeline" are needed.

/* Instead of using the MonoBehavior as the base class,
 * it instead takes from the Marker base class
 * and the INotification interface, as those are responsible
 * for giving it the marker system, as well as the ability
 * to communicate cross scripts respectively.
*/
public class ButtonMarker : Marker, INotification
{
    // As there are two buttons, a buttonInfo array is set to have the size of 2
    public buttonInfo[] buttons = new buttonInfo[2];

    // Button info with the text and corruption of that choice
    [System.Serializable]
    public class buttonInfo
    {
        public string buttonText;
        public int choiceCorruption;
    }

    // from the INotification interface - responsible for sending data across
    public PropertyName id { get { return new PropertyName(); } }
}

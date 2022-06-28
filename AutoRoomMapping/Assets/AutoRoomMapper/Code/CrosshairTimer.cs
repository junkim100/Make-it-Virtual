using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace lemnel.AutoRoomMapper
{
    public class CrosshairTimer : MonoBehaviour
    {
        public TextMeshProUGUI timeText;
        public int sec;

        void Start ()
        {
            timeText.text = sec.ToString();
            StartCoroutine (second ());
        }

        void Update ()
        {
            if (sec < 0) {
                timeText.text = "!";
                StopCoroutine (second ());
            }
        }
        IEnumerator second()
        {
            yield return new WaitForSeconds (1f);
            sec--;
            timeText.text = sec.ToString();
            StartCoroutine (second ());
        }
    }
}

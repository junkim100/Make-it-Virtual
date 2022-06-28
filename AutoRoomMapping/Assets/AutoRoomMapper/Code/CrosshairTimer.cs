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
        private int SEC;
        private bool start = false;

        public void TimerSetup()
        {
            SEC = sec;
            timeText.text = sec.ToString();
            StartCoroutine(second());
        }

        public void TimerStart()
        {
            start = true;
            if (sec < 0) {
                timeText.text = "!";
                StopCoroutine(second());
            }
        }

        public void TimerStop() {
            start = false;
            sec = SEC;
            timeText.text = sec.ToString();
            StopCoroutine(second());
        }

        public IEnumerator second()
        {
            yield return new WaitForSeconds (1f);
            if (start)
                sec--;
            if (sec < 0)
                TimerStop();
            timeText.text = sec.ToString();
            StartCoroutine(second());
        }
    }
}

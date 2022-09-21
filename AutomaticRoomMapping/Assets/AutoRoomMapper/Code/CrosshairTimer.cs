using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Pixelplacement.XRTools;

namespace lemnel.AutoRoomMapper
{
    public class CrosshairTimer : MonoBehaviour
    {
        public TextMeshProUGUI timeText;
        public int sec;
        public static float timeLeft;

        void Start() {
            TimerSetup();
        }

        public void TimerSetup()
        {
            timeLeft = sec+1;
        }

        public int TimerStart()
        {
            timeLeft -= Time.deltaTime;
            int seconds = Mathf.FloorToInt(timeLeft);
            timeText.text = seconds.ToString();
            return seconds;
        }

        public void TimerReset() {
            TimerSetup();
        }
    }
}

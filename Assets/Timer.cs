using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Timer : DefaultClass
{
    [SerializeField]
    private int minutes;
    [SerializeField]
    private Text timer;

    public bool bTimerEnded = false;

    private void Start()
    {
        StartCoroutine(StartTimer());
    }

    private string TimeToMinutes(int time)
    {
        int m = time / 100 / 60;
        int s = (time - (m * 100 * 60)) / 100;
        int ms = time - (m * 100 * 60) - (s * 100);

        return ((m < 10) ? "0" + m.ToString() : m.ToString()) + ":" + 
               ((s < 10) ? "0" + s.ToString() : s.ToString()) + ":" + 
               ((ms < 10) ? "0" + ms.ToString() : ms.ToString());
    }

    private IEnumerator StartTimer()
    {
        int time = minutes * 60 * 100;

        for (int i = 0; i < time; i++) {
            timer.text = TimeToMinutes(time - i);

            yield return new WaitForSeconds(0.01f);
        }

        bTimerEnded = true;
        timer.text = "RUN";
    }
}
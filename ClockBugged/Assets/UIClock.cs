using DigitalClock;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIClock : MonoBehaviour
{
    ClockDisplay clock;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        clock = new ClockDisplay(23, 50);
        text.text = clock.displayString;
        StartCoroutine(Clock());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Clock()
    {
        while (true)
        {
            clock.TimeTick();
            text.text = clock.displayString;
            yield return new WaitForSeconds(1);
        }
    }

    public void SetTime(string time)
    {
        
        int hour = int.Parse(time.Split(':')[0]);
        int minute = int.Parse(time.Split(':')[1]);
        clock.SetTime(hour, minute);
        
    }
}

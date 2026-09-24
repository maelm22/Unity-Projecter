using DigitalClock;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleClock : MonoBehaviour
{
    ClockDisplay clock;
    // Start is called before the first frame update
    void Start()
    {
        clock = new ClockDisplay(23, 50);
        Debug.Log(clock.displayString);
        StartCoroutine(Clock());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Clock()
    {
        while(true)
            {
            clock.TimeTick();
            Debug.Log(clock.displayString);
            yield return new WaitForSeconds(1);
        }
    }

    public void SetTime(int hour, int minute)
    {
        clock.SetTime(hour, minute);
    }
}

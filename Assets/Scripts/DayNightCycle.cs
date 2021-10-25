using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public float counter = 0;
    public int seconds = 0;
    public int minutes = 0;
    public int hours = 0;
    public int timeSpeed = 1;

    void Update()
    {
        counter += (Time.deltaTime * timeSpeed);
        if(counter >= 1)
        {
            seconds++;
            counter--;
        }
        if(seconds >= 60)
        {
            minutes++;
            seconds -= 60;
        }
        if(minutes >= 60)
        {
            minutes -= 60;
            hours++;
        }
        transform.Rotate(Vector3.back * Time.deltaTime * 0.25f * (timeSpeed), Space.World);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeUtils : MonoBehaviour
{
    const string timeFormat = "{0}:{1}";

    public static string TimeToText(float time)
    {
        if (time < 60)
        {
            return string.Format(timeFormat, "00", time.ToString("00"));
        }
        else if (time > 60 * 60)
        {
            return string.Format(timeFormat, "99", "99");
        }
        else
        {
            return string.Format(timeFormat, (time / 60).ToString("00"), (time % 60).ToString("00"));
        }
    }
}

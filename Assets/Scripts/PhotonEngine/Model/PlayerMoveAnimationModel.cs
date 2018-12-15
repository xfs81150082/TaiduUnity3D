using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class PlayerMoveAnimationModel {

	public bool IsMove { get; set; }
    //public DateTime Time { get;set ;}

    public string time;

    public void SetTime(DateTime dateTime)
    {
        time = dateTime.ToString("yyyyMMddHHmmssffff");
    }

    public DateTime GeTime()
    {
        DateTime dt = DateTime.ParseExact(time, "yyyyMMddHHmmssffff",CultureInfo.CurrentCulture);
        return dt;
    }


}

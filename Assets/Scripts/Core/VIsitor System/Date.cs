using System;
using UnityEngine;


[Serializable]
public class Date : IComparable<Date>
{
    public int month;
    public int day;
    public int year;

    public int CompareTo(Date other)
    {
        if (other == null)
            return 1;

        if (this.year != other.year)
        {
            return this.year.CompareTo(other.year);
        }

        if (this.month != other.month)
        {
            return this.month.CompareTo(other.month);
        }

        return this.day.CompareTo(other.day);
    }

    public override string ToString()
    {
        return $"{month}/{day}/{year}";
    }
}

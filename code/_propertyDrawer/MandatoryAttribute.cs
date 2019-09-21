using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
public class MandatoryAttribute : PropertyAttribute
{
    public MandatoryAttribute()
    {

    }
}

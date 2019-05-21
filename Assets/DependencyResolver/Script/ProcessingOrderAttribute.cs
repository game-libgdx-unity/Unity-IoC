using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class)]
public class ProcessingOrderAttribute : Attribute, IComparer
{
    public int Order { get; set; }

    public ProcessingOrderAttribute(int order = 0)
    {
        this.Order = order;
    }

    public int Compare(object x, object y)
    {
        return (x as ProcessingOrderAttribute).Order - (y as ProcessingOrderAttribute).Order;
    }
}
[AttributeUsage(AttributeTargets.Class)]
public class IgnoreProcessingAttribute : Attribute
{
    public IgnoreProcessingAttribute()
    {
    }
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class OverrideAttribute : Attribute
{
    public OverrideAttribute()
    {
    }
}

public class MonobehaviourComparer : IComparer<MonoBehaviour>
{
    public static MonobehaviourComparer Default = new MonobehaviourComparer();
    
    public int Compare(MonoBehaviour x, MonoBehaviour y)
    {
        var att1 = x.GetType().GetCustomAttributes(typeof(ProcessingOrderAttribute), false).FirstOrDefault() as ProcessingOrderAttribute;
        var o1 = att1.Order;
        var att2 = y.GetType().GetCustomAttributes(typeof(ProcessingOrderAttribute), false).FirstOrDefault() as ProcessingOrderAttribute;
        var o2 = att2.Order;

        return o1.CompareTo(o2);
    }
}
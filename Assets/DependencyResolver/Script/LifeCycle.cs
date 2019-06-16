﻿using System;

namespace UnityIoC
{
    [Flags]
    public enum LifeCycle : byte
    {
        Default = 0 << 0,
        Transient = 1 << 0,
        Singleton = 1 << 1,
        Component = 1 << 2,
        Children = 1 << 3,
        Prefab = 1 << 4,
        Cache = 1 << 5,
        
        SingletonComponent = Singleton | Component,

        Inject = Default |
                 Cache |
                 Component |
                 Prefab
    }

    public static class LifeCycleExtension
    {
        public static bool IsEqual(this LifeCycle first, LifeCycle second)
        {
            var firstAsByte = (byte) first;
            var secondAsByte = (byte) second;

            if (firstAsByte < (byte) LifeCycle.Singleton)
                firstAsByte = (byte) LifeCycle.Transient;

            if (secondAsByte < (byte) LifeCycle.Singleton)
                secondAsByte = (byte) LifeCycle.Transient;

            return (first & second) == second || firstAsByte.Equals(secondAsByte);
        }
    }
}
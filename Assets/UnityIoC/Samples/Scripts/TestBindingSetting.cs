﻿using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityIoC;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityIoC;

namespace SceneTest
{
    public class TestBindingSetting : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
            //create context which will automatically load binding setting by the assembly name
            //in this case, please refer to SceneTest setting from the resources folder.
            var assemblyContext = new AssemblyContext(GetType());

            //try to resolve the object by the default settings of this SceneTest assembly
            var obj3 = assemblyContext.Resolve<AbstractClass>(LifeCycle.Transient);
            var obj = assemblyContext.Resolve<AbstractClass>(LifeCycle.Singleton);
            var obj2 = assemblyContext.Resolve<AbstractClass>(LifeCycle.Singleton);

            //you should see a log of this action in unity console
            obj.DoSomething();
            
            //check ReferenceEquals, should be true
            Debug.Log("OKKKKK: " + ReferenceEquals(obj, obj2));
            Debug.Log("OKKKKK: " + !ReferenceEquals(obj3, obj2));
        }
    }
}
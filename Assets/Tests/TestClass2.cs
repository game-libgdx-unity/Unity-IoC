﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityIoC.Editor
{
	
	public class TestClass2 : TestInterface {
		public string JustAProperty { get; set; }

		public void DoSomething()
		{
			UniversalResolverDebug.Log("Hello 2"); 
		}
	}

}
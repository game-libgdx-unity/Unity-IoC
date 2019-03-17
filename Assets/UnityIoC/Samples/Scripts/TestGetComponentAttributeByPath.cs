﻿using System.Collections;
using System.Collections.Generic;
using SceneTest;
using UnityEngine;
using UnityIoC;

public class TestGetComponentAttributeByPath : MonoBehaviour
{

	//This attribute do the work as "Add Or GetComponent" from this GameObject
	[Component("child/child")] private TestComponent[] testComponents;
	[Component("child/child")] private TestComponent aTestComponent;
	// Use this for initialization
	void Awake ()
	{
		//create context which will automatically load binding setting by the assembly name
		//in this case, please refer to SceneTest setting from the resources folder.
		 new AssemblyContext(this);
	}

	private void Start()
	{
		print("Number of test components: "+testComponents.Length);
		
		aTestComponent.abstractClass.DoSomething();
	}
}
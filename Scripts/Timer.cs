using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
	[SerializeField] float initialTime;
	float currentTime;
	bool start = false;
	bool done = false;
	void Start()
	{
		currentTime = initialTime;
	}

	// Update is called once per frame
	void Update()
    {
		// putting code inside update... the ultimate sin
		if (start)
		{
			currentTime -= Time.deltaTime;

			if (currentTime <= 0)
			{
				// time's up
				
				start = false;
				done = true;
			}
		}
    }

	void StartTimer()
	{
		start = true;
		done = false;
	}

	public bool HasCompleted()
	{
		return done;
	}
}

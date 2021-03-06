using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSource : MonoBehaviour
{
	// light values
	[SerializeField] float intensityMax;
	[SerializeField] float intensityMin;
	[SerializeField] float intensityIncr;
	[SerializeField] Player player;

	bool active = false;
	Light lightRef;

    // Start is called before the first frame update
    void Start()
    {
		lightRef = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
		Lighten();
		Darken();
		FacePlayer();
	}


	public void Activate()
	{
		if (lightRef != null)
		{
			if (active)
			{
				Debug.Log("DeActivated");
				active = false;
			}
			else
			{
				Debug.Log("Activated");
				active = true;
			}
		}
		
	}


	void Lighten()
	{
		if (active && lightRef.intensity <= intensityMax)
		{
			lightRef.intensity += intensityIncr;
		}

	}

	void Darken()
	{
		if (active == false && lightRef.intensity >= intensityMin)
		{
			lightRef.intensity -= intensityIncr * 2;
		}
	}

	void FacePlayer()
	{
		Vector3 dir = player.gameObject.transform.position - transform.position;
		transform.rotation = Quaternion.LookRotation(dir);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
	[SerializeField] GameObject cam;
	[SerializeField] float sensitivity;
	[SerializeField] float speedFast;
	[SerializeField] float slowedSpeed;
	CharacterController control;
	[SerializeField] float grav;
	[SerializeField] LightSource lightSource;
	[SerializeField] Monster monster;
	[SerializeField] float sprintSpeed;
	[SerializeField] float sprintTimeTotal;
	[SerializeField] Slider slider;
	AudioSource footSteps;
	float sprintTime;
	[SerializeField] float sprintRechargeTotal;
	float sprintRecharge;
	[SerializeField] float sprintGainIncr;

	float rotation = 0.0f;
	Vector3 vel;
	bool active = false;
	float speed;
	// Start is called before the first frame update
	void Start()
	{
		// make sure the cursor can't leave the screen
		Cursor.lockState = CursorLockMode.Locked;
		speed = speedFast;
		sprintTime = sprintTimeTotal;
		sprintRecharge = sprintRechargeTotal;
		footSteps = GetComponent<AudioSource>();
		control = GetComponent<CharacterController>();
		control.detectCollisions = true;
	}

	// Update is called once per frame
	void Update()
	{
		Look();
		Move();
		Sprint();
		Activate();
	}

	void Look()
	{
		//float x = Input.GetAxis("Mouse X");
		//float y = Input.GetAxis("Mouse Y");

		//// mutliply by time to make sure it's not based on frame rate
		//x *= sensitivity * Time.deltaTime;
		//y *= sensitivity * Time.deltaTime;

		//rotation -= y;
		//rotation = Mathf.Clamp(rotation, -90.0f, 90.0f);


		//// rotate the camera
		//cam.transform.localRotation = Quaternion.Euler(rotation, 0.0f, 0.0f);
		//// rotate the player
		//gameObject.transform.Rotate(Vector3.up * x);

		float x = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
		float y = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

		rotation += y;

		if (rotation > 90)
		{
			rotation = 90;
			y = 0;

			Vector3 rot = cam.gameObject.transform.eulerAngles;
			rot.x = 270.0f;
			cam.gameObject.transform.eulerAngles = rot;

		}
		if (rotation < -90)
		{
			rotation = -90;
			y = 0;

			Vector3 rot = cam.gameObject.transform.eulerAngles;
			rot.x = 90.0f;
			cam.gameObject.transform.eulerAngles = rot;
		}

		cam.transform.Rotate(Vector3.left * y);
		transform.Rotate(Vector3.up * x);
	}


	void Move()
	{
		float horiz = Input.GetAxis("Horizontal");
		float vert = Input.GetAxis("Vertical");

		Vector3 direction = (horiz * transform.right) + (vert * cam.transform.forward);
		Vector3 result = direction * Time.deltaTime * speed;

		vel.y += grav * Time.deltaTime * Time.deltaTime;

		control.Move(vel);

		control.Move(result);

		//if we aren't stationary
		if (control.velocity.magnitude > 2f && footSteps.isPlaying == false)
		{
			Debug.Log("Playing Sound");
			footSteps.Play();
		}
		else if(control.velocity.magnitude < 2f && footSteps.isPlaying)
		{
			Debug.Log("Stopped playing sound");
			footSteps.Stop();
		}

	}

	void Activate()
	{
		if (lightSource != null && Input.GetKeyDown(KeyCode.Space))
		{
			lightSource.Activate();
			if (active)
			{
				active = false;
				speed = speedFast;
			}
			else
			{
				active = true;
				speed = slowedSpeed;
				monster.InvestigatePlayer(transform.position);
			}
		}
	}

	void Sprint()
	{

		// the sprint is based on timers because I'm tired 
		if (Input.GetKey(KeyCode.LeftShift) && sprintTime > 0 && active == false)
		{
			sprintTime -= Time.deltaTime;
			speed = sprintSpeed;
			sprintRecharge = sprintRechargeTotal;
		}

		sprintRecharge -= Time.deltaTime;

		if (sprintRecharge <= 0 && sprintTime <= sprintTimeTotal)
		{
			sprintTime += sprintGainIncr;
		}

		// update the UI

		// get a percentage related to our sprint total
		float fill = sprintTime / sprintTimeTotal;
		slider.value = fill;

	}

	private void OnTriggerEnter(Collider col)
	{
		// if we collided with the monster
		if (col.tag == "Monster")
		{
			SceneManager.LoadScene(3);
		}

		if (col.tag =="Win")
		{
			SceneManager.LoadScene(0);
		}
	}
}

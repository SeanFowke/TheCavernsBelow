using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuController : MonoBehaviour
{
	public void Play()
	{
		SceneManager.LoadScene(1);
	}

	public void LoadHowToPlay()
	{
		SceneManager.LoadScene(2);
	}

	public void ReturnToMainMenu()
	{
		SceneManager.LoadScene(0);
	}

	public void Exit()
	{
		Application.Quit();
	}
}

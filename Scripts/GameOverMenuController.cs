using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenuController : MonoBehaviour
{
	public void Retry()
	{
		SceneManager.LoadScene(1);
	}

	public void Exit()
	{
		Application.Quit();
	}
}

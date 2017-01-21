using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public GameObject loading;
	public GameObject buttons;

	public void StartGame()
	{
		buttons.SetActive(false);
		loading.SetActive(true);
		SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
	}

	public void QuitApplication()
	{
		Application.Quit();
	}

}

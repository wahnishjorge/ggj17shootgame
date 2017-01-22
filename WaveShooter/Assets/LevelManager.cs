using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
	public static LevelManager instance;

	public Text _waveTimer;
	public Text _waveText;
	public Text _ammoText;
	public GameObject[] _life;

	void Awake()
	{
		instance = this;
	}

	public void SetWave(int xWave)
	{
		_waveText.text = "Wave - " + xWave.ToString();
	}

	public void SetTimer(int xTimer)
	{
		_waveTimer.text = xTimer.ToString();
	}


	public void SetText(string xText)
	{
		_waveTimer.text = xText;
	}

	public void SetAmmo(string xText)
	{
		_ammoText.text = xText;
	}

	public void LoseLife(int xLife)
	{
		_life[xLife].SetActive(false);
	}

	public void GainLife(int xLife)
	{
		_life[xLife].SetActive(true);
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodyCamera : MonoBehaviour
{
	public Texture2D _tex;
	public bool _activateBloody;
	private bool _spit;
	private Rect _r;
	private float _bloodRate;
	private float _time = 0.9f;

	void Start()
	{
		_r = new Rect(0, 0, Screen.width, Screen.height);
		ActivateBloody();
	}

	void Update()
	{
		
		if (_activateBloody)
		{
			_bloodRate -= Time.deltaTime;
			if (_bloodRate < 0)
				_activateBloody = false;
		}
	}

	public void ActivateBloody()
	{
		_activateBloody = true;
		_bloodRate = _time;
	}

	void OnGUI()
	{
		if (_activateBloody)
		{
			GUI.DrawTexture(_r, _tex);
		}
	}
}

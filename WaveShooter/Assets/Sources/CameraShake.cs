using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	public static CameraShake instance;

	private Vector3 _originalPos;
	private float _timeAtCurrentFrame;
	private float _timeAtLastFrame;
	private float _fakeDelta;

	void Awake()
	{
		instance = this;
	}

	void Update()
	{
		_timeAtCurrentFrame = Time.realtimeSinceStartup;
		_fakeDelta = _timeAtCurrentFrame - _timeAtLastFrame;
		_timeAtLastFrame = _timeAtCurrentFrame;
	}

	public void Shake(float duration, float amount)
	{
		instance._originalPos = instance.gameObject.transform.localPosition;
		instance.StopAllCoroutines();
		instance.StartCoroutine(instance.cShake(duration, amount));
	}

	public IEnumerator cShake(float duration, float amount)
	{
		float endTime = Time.time + duration;

		while (duration > 0)
		{
			transform.localPosition = _originalPos + Random.insideUnitSphere * amount;

			duration -= _fakeDelta;

			yield return null;
		}

		transform.localPosition = _originalPos;
	}
}

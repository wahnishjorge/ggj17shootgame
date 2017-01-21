using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBaby : MonoBehaviour
{
	private List<GameObject> _blood = new List<GameObject>();

	void Awake()
	{
		foreach (Transform child in transform)
		{
			_blood.Add(child.gameObject);
		}
	}

	public void Activate(Vector3 xPos)
	{
		gameObject.transform.position = xPos;
		foreach (GameObject go in _blood)
		{
			go.gameObject.SetActive(true);
		}
		Destroy(this.gameObject,1.5f);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static int m_ZombiesCount = 0;
    public List<Waves> m_Wave = new List<Waves>();
    private int m_CurrentWave = 0;
    internal bool m_WaveExist = false;
    internal bool m_IsRandomLevel = false;
    private bool m_Wait = false;
    private List<Spawner> _spawner = new List<Spawner>();
    private List<Spawner> m_Spawner
    {
        get
        {
            if (_spawner == null || _spawner.Count == 0)
            {
                _spawner = new List<Spawner>();
                GameObject[] sObjs = GameObject.FindGameObjectsWithTag("Spawner");
                foreach(GameObject sObj in sObjs)
                {
                    _spawner.Add(sObj.GetComponent<Spawner>());
                }

            }
            return _spawner;
        }
    }

    // Use this for initialization
    void Start()
    {
        if (m_Wave.Count > 0)
            m_WaveExist = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (m_ZombiesCount == 0 && m_WaveExist && !m_Wait)
            StartCoroutine(StartWave());
	}

    void NextWave()
    {
        m_ZombiesCount = 0;
        m_Spawner[Random.Range(0, m_Spawner.Count - 1)].Spawn(m_Wave[m_CurrentWave].m_Objects);
        foreach (SpawnList sList in m_Wave[m_CurrentWave].m_Objects)
        {
            if (sList.m_IsEnemy)
                m_ZombiesCount += sList.m_Count;
        }
        m_CurrentWave++;
    }


    IEnumerator StartWave()
    {
        if (m_CurrentWave < m_Wave.Count)
        {
            m_Wait = true;
            m_WaveExist = true;
			LevelManager.instance.SetWave(m_CurrentWave + 1);
			yield return new WaitForSeconds(1f);
			LevelManager.instance.SetTimer(3);
            yield return new WaitForSeconds(1f);
			LevelManager.instance.SetTimer(2);
			yield return new WaitForSeconds(1f);
			LevelManager.instance.SetTimer(1);
			yield return new WaitForSeconds(1f);
			LevelManager.instance.SetText("Survive");
			yield return new WaitForSeconds(0.5f);
			LevelManager.instance.SetText("");
			NextWave();
            m_Wait = false;
        }
        else
        {
            m_WaveExist = false;
			LevelManager.instance.SetText("You win!!");
            yield return new WaitForSeconds(1f);
			LevelManager.instance.SetText("Go to next level in 3");
			yield return new WaitForSeconds(1f);
			LevelManager.instance.SetText("Go to next level in 2");
			yield return new WaitForSeconds(1f);
			LevelManager.instance.SetText("Go to next level in 1");
			yield return new WaitForSeconds(1f);
            if (m_IsRandomLevel)
                LevelManager.instance.LoadRandomLevel();
        }
    }

    public static void ZombieDie()
    {
        if (m_ZombiesCount - 1 >= 0)
            m_ZombiesCount--;
        else
            m_ZombiesCount = 0;
    }
}

[System.Serializable]
public class Waves
{
    public List<SpawnList> m_Objects = new List<SpawnList>();
}
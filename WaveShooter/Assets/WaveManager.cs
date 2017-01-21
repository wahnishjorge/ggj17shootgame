using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static int m_ZombiesCount = 0;
    public List<Waves> m_Wave = new List<Waves>();
    private int m_CurrentWave = 0;
    private bool m_WaveExist = false;
    private bool m_Wait = false;
    private Spawner _spawner;
    private Spawner m_Spawner
    {
        get
        {
            if (_spawner == null)
                _spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();
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
        foreach (SpawnList sList in m_Wave[m_CurrentWave].m_Objects)
        {
            if (sList.m_IsEnemy)
                m_ZombiesCount += sList.m_Count;
        }
        m_Spawner.Spawn(m_Wave[m_CurrentWave].m_Objects);
        m_CurrentWave++;
    }


    IEnumerator StartWave()
    {
        if (m_CurrentWave < m_Wave.Count)
        {
            m_Wait = true;
            m_WaveExist = true;
            Debug.Log("Wave " + (m_CurrentWave + 1).ToString());
            yield return new WaitForSeconds(1f);
            Debug.Log("3");
            yield return new WaitForSeconds(1f);
            Debug.Log("2");
            yield return new WaitForSeconds(1f);
            Debug.Log("1");
            yield return new WaitForSeconds(1f);
            Debug.Log("Survive");
            NextWave();
            m_Wait = false;
        }
        else
        {
            m_WaveExist = false;
            Debug.Log("You win!!");
            yield return new WaitForSeconds(1f);
            Debug.Log("Will exit in 3");
            yield return new WaitForSeconds(1f);
            Debug.Log("Will exit in 2");
            yield return new WaitForSeconds(1f);
            Debug.Log("Will exit in 1");
            yield return new WaitForSeconds(1f);
        }
    }

    public static void ZombieDie()
    {
        if (m_ZombiesCount - 1 >= 0)
            m_ZombiesCount--;
        else
            m_ZombiesCount = 0;

        Debug.Log("DIE");
    }
}

[System.Serializable]
public class Waves
{
    public List<SpawnList> m_Objects = new List<SpawnList>();
}
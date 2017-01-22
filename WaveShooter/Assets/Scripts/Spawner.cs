using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    private GameObject _playerObj;
    private GameObject m_PlayerObj
    {
        get
        {
            if (_playerObj == null)
                _playerObj = GameObject.FindGameObjectWithTag("Player");
            return _playerObj;
        }
    }

    public void Spawn(List<SpawnList> xObjects)
    {
        for (int i = 0; i < xObjects.Count; i++)
        {
            xObjects[i].m_Count = Random.Range(xObjects[i].m_CountMin, xObjects[i].m_CountMax);
            for (int j = 0; j < xObjects[i].m_Count; j++)
                Instantiate(xObjects[i].m_Obj, SpawnPosition(), Quaternion.identity);
        }
    }

    Vector3 SpawnPosition()
    {
        Vector3 sPosition = Vector3.zero;
        for(int i=0;i<3;i++)
        { 
            Vector3 randomDirection = GetRandomPosition();
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, 100, 1);
            sPosition = hit.position;
            if (Vector3.Distance(m_PlayerObj.transform.position, hit.position) > 3)
                break;
        }
        return sPosition;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color32(0, 255, 0, 50);
        Gizmos.DrawCube(transform.position, transform.localScale);
    }

    Vector3 GetRandomPosition()
    {
        float sStartX = transform.position.x - transform.localScale.x / 2f;
        float sEndX = transform.position.x + transform.localScale.x / 2f;
        if (sEndX < sStartX)
        {
            float sTmp = sStartX;
            sStartX = sEndX;
            sEndX = sTmp;
        }

        float sStartZ = transform.position.z - transform.localScale.z / 2f;
        float sEndZ = transform.position.z + transform.localScale.z / 2f;
        if (sEndZ < sStartZ)
        {
            float sTmp = sStartZ;
            sStartZ = sEndZ;
            sEndZ = sTmp;
        }

        return new Vector3(Random.Range(sStartX, sEndX), transform.position.y + transform.localScale.y, Random.Range(sStartZ, sEndZ));
    }
}

[System.Serializable]
public class SpawnList
{
    public GameObject m_Obj;
    public bool m_IsEnemy;
    public int m_Count = 1;
    public int m_CountMin = 1;
    public int m_CountMax = 1;
}
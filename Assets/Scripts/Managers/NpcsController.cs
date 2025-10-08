using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NpcsController : MonoBehaviour
{
    [SerializeField] private GameObject[] _npcs;
    [SerializeField] private Transform[] _positions;
    [SerializeField] private Transform[] _intermediatePoints;
    [SerializeField] private float[] _timesToSpawn;

    [SerializeField] private int _maxNpcsInScene = 5;

    void Start()
    {
        StartCoroutine(SpawnNpc());
    }

    IEnumerator SpawnNpc()
    {
        if(CheckNpcs()){
            Transform startPoint = RdnPositions();

            GameObject newNpc = Instantiate(RdnNpc(), startPoint.position, Quaternion.identity);
            NpcMovement movement = newNpc.GetComponent<NpcMovement>();

            List<Transform> path = GenerateRandomIntermediatePath();
            path.Add(RdnPositions());

            movement.SetPath(path);
        }

        yield return new WaitForSeconds(RdnTimeToSpawn());
        StartCoroutine(SpawnNpc());
    }

    List<Transform> GenerateRandomIntermediatePath()
    {
        List<Transform> path = new List<Transform>();
        List<Transform> pool = new List<Transform>(_intermediatePoints);

        int count = Random.Range(1, Mathf.Min(4, pool.Count + 1));

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, pool.Count);
            path.Add(pool[index]);
            pool.RemoveAt(index);
        }

        return path;
    }

    GameObject RdnNpc()
    {
        int rdnIndex = Random.Range(0, _npcs.Length);
        return _npcs[rdnIndex];
    }

    float RdnTimeToSpawn()
    {
        int rdnIndex = Random.Range(0, _timesToSpawn.Length);
        return _timesToSpawn[rdnIndex];
    }

    Transform RdnPositions()
    {
        int rdnIndex = Random.Range(0, _positions.Length);
        return _positions[rdnIndex];
    }

    private bool CheckNpcs(){
        if(GameObject.FindGameObjectsWithTag("Npc").Count() >= _maxNpcsInScene){
            return false;
        }
        return true;
    }
}

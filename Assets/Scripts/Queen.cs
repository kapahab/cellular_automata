using System.Collections;
using UnityEngine;

public class Queen : MonoBehaviour
{
    [SerializeField] private GameObject antPrefab;
    [SerializeField] private float antSpawnInterval = 5f;
    public AntManager antManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnAnts());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnAnts()
    {
        while (true)
        {
            antManager.InstantiateAnt(new Vector2Int((int)transform.position.x, (int)transform.position.y), new Vector2Int((int)transform.position.x, (int)transform.position.y));
            yield return new WaitForSeconds(antSpawnInterval);
        }
        
    }
}

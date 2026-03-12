using System.Collections;
using UnityEngine;

public class Queen : MonoBehaviour
{
    [SerializeField] private GameObject antPrefab;
    [SerializeField] private float antSpawnInterval = 5f;
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
            Instantiate(antPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(antSpawnInterval);
        }
        
    }
}

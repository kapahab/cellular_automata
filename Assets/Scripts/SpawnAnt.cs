using UnityEngine;

public class SpawnAnt : MonoBehaviour
{
    [SerializeField] GameObject antPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(1))
        //{
        //    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    mousePos.z = 0; // Set z to 0 for 2D
        //    Instantiate(antPrefab, mousePos, Quaternion.identity);
        //}
    }
}

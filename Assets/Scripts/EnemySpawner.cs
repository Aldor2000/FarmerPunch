using UnityEngine;
using UnityEditor;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float radius = 4f;
    [SerializeField] int numberToSpawn = 3;
    [SerializeField] GameObject smallEnemy;
    [SerializeField] GameObject LargeEnemy;

    [SerializeField] bool hasBigEnemy = false;

    //for gizmo
    private Color smallColor = Color.blue;
    private Color bigColor = Color.red;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnEnemies();
        if (hasBigEnemy)
        SpawnBigEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            //insideUnitCircle returns a  position inside a radiues
            Vector2 randPos = Random.insideUnitCircle.normalized * Random.Range(radius * 0.5f, radius);
            Vector3 spawnPos = transform.position + (Vector3)randPos;
            Instantiate(smallEnemy, spawnPos, Quaternion.identity);
        }
    }

    void SpawnBigEnemy()
    {
        Instantiate(LargeEnemy, transform.position, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = hasBigEnemy ? bigColor : smallColor ;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

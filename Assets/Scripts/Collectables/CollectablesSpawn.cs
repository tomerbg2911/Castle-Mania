using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesSpawn : MonoBehaviour
{
    // collectables spawn related vars
    public GameObject manaCollectablePrefab;
    public GameObject shieldCollectablePrefab;
    public float spawnTimeInterval;
    public float minXPosition;
    public float maxXPosition;
    public float minFallingSpeed;
    public float maxFallingSpeed;
    public float initYPosition;
    public float manaCollectableProb = 0.7f;
    private float shieldCollectableProb;

    // Start is called before the first frame update
    void Start()
    {
        shieldCollectableProb = 1 - manaCollectableProb;
        InvokeRepeating("Spawn", 2f, spawnTimeInterval);
    }


    void Spawn()
    {
        // generate random floats
        float prob = Random.Range(0f, 1f); // for collectable type randomization
        float xPosition = Random.Range(minXPosition, maxXPosition);
        float fallingSpeed = Random.Range(minFallingSpeed, maxFallingSpeed);

        Vector3 position = new Vector3(xPosition, initYPosition);
        GameObject CollectablePrefab = prob <= manaCollectableProb ? manaCollectablePrefab : shieldCollectablePrefab;
        GameObject newCollectable = Instantiate(CollectablePrefab, position, Quaternion.identity) as GameObject;
        Collectable collectable = newCollectable.GetComponent<Collectable>();
        collectable.fallingSpeed = fallingSpeed;
    }
}
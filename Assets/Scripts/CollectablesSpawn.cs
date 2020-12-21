using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesSpawn : MonoBehaviour
{
    // collectables spawn related vars
    public GameObject CollectableGameObject;
    public float spawnTimeInterval;
    public float minXPosition;
    public float maxXPosition;
    public float minFallingSpeed;
    public float maxFallingSpeed;
    public float initYPosition;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", 2f, spawnTimeInterval);
    }


    void Spawn()
    {
        float xPosition = Random.Range(minXPosition, maxXPosition);
        Vector3 position = new Vector3(xPosition, initYPosition);
        float fallingSpeed = Random.Range(minFallingSpeed, maxFallingSpeed);

        GameObject newCollectable = Instantiate(CollectableGameObject, position, Quaternion.identity) as GameObject;
        ManaCollectable manaCollectable = newCollectable.GetComponent<ManaCollectable>();
        manaCollectable.fallingSpeed = fallingSpeed;
    }
}
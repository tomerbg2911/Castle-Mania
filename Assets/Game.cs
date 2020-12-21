using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{   

    // collectables spawn related vars
    public GameObject CollectableGameObject;
    public float collectableTimeInterval;
    public float collectableMinX;
    public float collectableMaxX;
    public float collectableMinFallingSpeed;
    public float collectableMaxFallingSpeed;
    public float collectableInitY;
    private float timeSinceLastCollectable;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("CollectablesSpawn", 2f, collectableTimeInterval);
    }

    // Update is called once per frame
    void Update()
    {
        //CollectablesSpawn();
    }

    void CollectablesSpawn()
    {
        float xPosition = Random.Range(collectableMinX, collectableMaxX);
        Vector3 position = new Vector3(xPosition, collectableInitY);
        float fallingSpeed = Random.Range(collectableMinFallingSpeed, collectableMaxFallingSpeed);
        
        GameObject newCollectable = Instantiate(CollectableGameObject, position, Quaternion.identity) as GameObject;
        ManaCollectable manaCollectable = newCollectable.GetComponent<ManaCollectable>();
        manaCollectable.fallingSpeed = fallingSpeed;
    }
}

using System.Collections;
using System.Xml.Serialization;
using UnityEngine;

public class DragonTarget : MonoBehaviour
{
    private RectTransform targetAreaRect;
    private Vector3 positionToMoveTowards;
    public float speed = 0.5f;
    public float waitBeforeMoving = 0f;
    public float eps = 0.1f;


    private void Start()
    {
        targetAreaRect = transform.parent as RectTransform;
        transform.localPosition = getRandomPosition();
        positionToMoveTowards = getRandomPosition();
    }

    private void Update()
    {
        if(Vector3.Distance(transform.localPosition,positionToMoveTowards) > eps)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, positionToMoveTowards, speed * Time.deltaTime);
        }
        else
        {
            Invoke("MoveToNextRandomPosition", waitBeforeMoving);
        }
    }

    void MoveToNextRandomPosition()
    {
        positionToMoveTowards = getRandomPosition();
    }

    Vector3 getRandomPosition()
    {
        float randX = Random.Range(targetAreaRect.rect.xMin, targetAreaRect.rect.xMax);
        float randY = Random.Range(targetAreaRect.rect.yMin, targetAreaRect.rect.yMax);
        return new Vector3(randX, randY, transform.position.z);
    }
}
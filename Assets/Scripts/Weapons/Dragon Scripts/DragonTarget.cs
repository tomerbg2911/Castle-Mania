using System.Collections;
using UnityEngine;

public class DragonTarget : MonoBehaviour
{
    private RectTransform targetAreaRect;
    private Vector3 nextRandomPosition;
    public float speed = 0.5f;
    public float waitBeforeMoving = 0f;
    public float eps = 0.1f;


    private void Start()
    {
        targetAreaRect = transform.parent as RectTransform;
        setNextRandomPosition();
    }

    private void Update()
    {
        if(Vector3.Distance(transform.localPosition,nextRandomPosition) > eps)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, nextRandomPosition, speed);
        }
        else
        {
            Invoke("setNextRandomPosition", waitBeforeMoving);
        }
    }

    void setNextRandomPosition()
    {
        float randX = Random.Range(targetAreaRect.rect.xMin, targetAreaRect.rect.xMax);
        float randY = Random.Range(targetAreaRect.rect.yMin, targetAreaRect.rect.yMax);
        nextRandomPosition = new Vector3(randX, randY, transform.position.z);
    }
}
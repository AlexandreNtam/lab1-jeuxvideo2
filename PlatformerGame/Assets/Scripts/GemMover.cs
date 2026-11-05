using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemMover : MonoBehaviour
{
    private const float MIN_OFFSET = 0f;
    private const float MAX_OFFSET = 20f;
    private const float GEM_SPEED = 25f;

    private float offSet;
    private float direction;
    private Vector3 basePosition;

    void Start()
    {
        basePosition = transform.position;

        offSet = Random.Range(MIN_OFFSET + 1f, MAX_OFFSET - 1f);

        direction = Random.Range(-1, 1);
        if (direction == 0) direction = 1;
    }

    void Update()
    {
        MoveGem();
    }

    private void MoveGem()
    {
        offSet += (Time.deltaTime * direction * GEM_SPEED);

        if (offSet > MAX_OFFSET)
        {
            offSet = MAX_OFFSET;
            direction = -1f;
        }
        else if (offSet < MIN_OFFSET)
        {
            offSet = MIN_OFFSET;
            direction = 1f;
        }

        transform.position = basePosition + (Vector3.up * (offSet / 100f));
    }
}
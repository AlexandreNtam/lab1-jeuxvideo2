using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private Transform[] WalkingNodes;
    [SerializeField] private float speed = 1f;
    private int targetNodeIndex = 0;

    private Transform targetNode;
    private bool activeMonster = true;

    void Start()
    {
        targetNode = WalkingNodes[targetNodeIndex];
    }

    void Update()
    {
        MoveMonster();
    }

    private void MoveMonster()
    {
        if (activeMonster)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetNode.position, Time.deltaTime * speed);

            if (Vector3.Distance(transform.position, targetNode.position) < 0.01f)
            {
                activeMonster = false;
                if (++targetNodeIndex == WalkingNodes.Length) targetNodeIndex = 0;
                targetNode = WalkingNodes[targetNodeIndex];
                StartCoroutine(MonsterSleep());
            }
        }
    }

    private IEnumerator MonsterSleep()
    {
        yield return new WaitForSeconds(2);
        activeMonster = true;
    }
}
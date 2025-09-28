using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform target1, target2;
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private GameObject[] requiredEnemies;

    private Transform currentTarget;
    private bool canMove;
    void Start()
    {
        currentTarget = target1;

        if (requiredEnemies == null || requiredEnemies.Length == 0)
        {
            canMove = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!canMove)
        {
            CheckEnemies();
            return;
        }

        if (transform.position == target1.position)
        {
            currentTarget = target2;
        }

        if (transform.position == target2.position)
        {
            currentTarget = target1;
        }

        transform.position = Vector2.MoveTowards(
            transform.position,
            currentTarget.position,
            moveSpeed * Time.deltaTime
        );
    }

    private void CheckEnemies()
    {
       
        foreach (var enemy in requiredEnemies)
        {
            if (enemy != null)
                return;
        }
        canMove = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.transform.position.y > transform.position.y)
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }


}

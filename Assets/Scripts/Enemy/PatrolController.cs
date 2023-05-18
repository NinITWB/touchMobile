using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolController : MonoBehaviour
{
    [Header("Area's references")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;
    [SerializeField] private Transform Enemy;
    [SerializeField] private Animator animator;

    [SerializeField] private float enemyVelocity;

    [Header("Idle to rest")]
    [SerializeField] private float timeToIdle;

    private EnemyAttack enemy;
    private Vector3 initScale;
    private bool turnDirect;
    private float delayTimer;

    private void Awake()
    {
        initScale = Enemy.localScale;
        animator = GetComponentInChildren<Animator>();
        enemy = GetComponentInChildren<EnemyAttack>();
    }

    private void moveInDirection(float _direct)
    {
        delayTimer = 0;
        Enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direct, initScale.y, initScale.z);
        Enemy.position = new Vector3(Enemy.position.x + enemyVelocity * _direct * Time.deltaTime, Enemy.position.y, Enemy.position.z);
        animator.SetBool("isWalk", true);
        animator.SetBool("isWait", false);
    }

    private void Update()
    {
        if (enemy.getWaitToFire()) return;

        if (turnDirect)
        {
            if (Enemy.position.x >= leftEdge.position.x)
            {
                moveInDirection(-1);
            }
            else
            {
                changeDirection();
            }
        }
        else
        {
            if (Enemy.position.x <= rightEdge.position.x)
            {
                moveInDirection(1);
            }
            else
            {
                changeDirection();
            }
        }


    }

    private void changeDirection()
    {
        delayTimer += Time.deltaTime;
        animator.SetBool("isWalk", false);
        if (delayTimer >= timeToIdle) turnDirect = !turnDirect;

    }




}

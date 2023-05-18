using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header ("Attack's damage & cooldown")]
    [SerializeField] private int damage;
    [SerializeField] private float attackCooldown;

    [Header("Range attack")]
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;

    [Header("active FireBall")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireBall;

    [Header("State of object")]
    [SerializeField] private float timeIdle;

    [Header("Object's layer and its references")]
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private BoxCollider2D boxCollide;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject player;

    private float attackTimer = Mathf.Infinity;
    private RaycastHit2D hit;
    private bool waitToFire;

    public int getDamage()
    {
        return damage;
    }

    public void setWaitToFire(bool waitToFire)
    {
        this.waitToFire = waitToFire;
    }

    public bool getWaitToFire()
    {
        return waitToFire;
    }

    private void Awake()
    {
        boxCollide = GetComponent<BoxCollider2D>();
    }


    private void Update()
    {

        if (playerInSight())
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackCooldown)
            {
                runFireBall();
                StartCoroutine(attack());
                attackTimer = 0;
            }
        }
        
    }

    private IEnumerator attack()
    {
        animator.SetTrigger("attack");
        waitToFire = true;
        yield return new WaitWhile(() => hit.collider != null);

        animator.SetBool("isWait", true);
        yield return new WaitForSeconds(timeIdle);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer" + ".attack"))
        {
            animator.SetBool("isWait", true);
        }

        

        waitToFire = false;
    }

    private void runFireBall()
    {
        GoFireBall();
    }

    private void GoFireBall()
    {
        fireBall[objectPooling()].transform.position = firePoint.position;
        fireBall[objectPooling()].GetComponent<fireBall>().setDirection(Mathf.Sign(transform.localScale.x));
    }

    private int objectPooling()
    {
        for (int i = 0; i < fireBall.Length; i++)
        {
            if (!fireBall[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }

    private bool playerInSight()
    {
       hit = Physics2D.BoxCast(boxCollide.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
       new Vector3(boxCollide.bounds.size.x * range, boxCollide.bounds.size.y, boxCollide.bounds.size.z), 0, Vector2.left, 0, playerMask);

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(boxCollide.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, new Vector3(boxCollide.bounds.size.x * range, boxCollide.bounds.size.y, boxCollide.bounds.size.z));

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Number of Life")]
    [SerializeField] private float numLife;
    [SerializeField] private GameObject enemy;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (enemy != null)
            enemy = (GameObject)FindObjectOfType(typeof(GameObject));
       
    }

    private void Update()
    {
        //Debug.Log(numLife);
        if (numLife == 0)
        {
            StartCoroutine(Death());

            if (enemy.GetComponentInChildren<EnemyAttack>() != null)
            enemy.GetComponentInChildren<EnemyAttack>().enabled = false;


            GetComponent<playerMovement>().enabled = false;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<BoxCollider2D>().enabled = false;
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GetComponent<knightAttack>().getBlocked()) return;
        if (collision.tag == "Weapon")
        {
            numLife -= FindObjectOfType<EnemyAttack>().getDamage();
            animator.SetTrigger("hurt");

        }
    }

    private IEnumerator Death()
    {
        animator.SetTrigger("die");
        yield return new WaitForSeconds(1);

        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathOfEnemy : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void death()
    {
        GetComponent<EnemyAttack>().enabled = false;
        StartCoroutine(deathEffect());
    }

    private IEnumerator deathEffect()
    {
        animator.SetTrigger("die");
        yield return new WaitForSeconds(1);

        GetComponentInParent<PatrolController>().enabled = false;
        gameObject.SetActive(false);
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class knightAttack : MonoBehaviour
{
    [Header("player attack's parameters")]
    [SerializeField] private float attackCoolDown;
    [SerializeField] private float blockCooldown;
    [SerializeField] private GameObject enemy;

    [Header("Attack area")]
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private CapsuleCollider2D capsuleCollide;
    [SerializeField] private Button Attack;
    [SerializeField] private LayerMask UILayer;
    playerMovement playerMovement;

    private Animator animator;
   

    private float attackTimer;
    private float blockTimer;
    private bool blocked;

    public void setBlcoked(bool blocked)
    {
        this.blocked = blocked;
    }

    public bool getBlocked()
    {
        return this.blocked;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>(); 
        capsuleCollide = GetComponent<CapsuleCollider2D>();
        playerMovement = GetComponent<playerMovement>();

        if (enemy != null)
            enemy = (GameObject) FindObjectOfType(typeof(GameObject));
    }


    private void Update()
    {
       attackTimer += Time.deltaTime;
       blockTimer += Time.deltaTime;
       SetUpAttack();
       Block();
    }

    private void SetUpAttack()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                Vector2 position = Camera.main.ScreenToWorldPoint(touch.position);
                RaycastHit2D[] hits = Physics2D.RaycastAll(position, Vector2.right * .05f, .1f, UILayer);

                foreach (var coll in hits)
                {
                    if (coll.collider.name == Attack.name && attackTimer >= attackCoolDown && touch.phase == TouchPhase.Began)
                    {
                        attackTimer = 0;
                        animator.SetTrigger("killEnemy");
                    }
                    Debug.Log(coll.collider.name);
                }
                
            }
        }
    }

    private void Block()
    {
        if (!isInBlock()) blocked = false;
        else blocked = true;
        if (blocked == false)
            GetComponent<Health>().enabled = true;
        else
        {
            if (GetComponent<Health>() != null)
                GetComponent<Health>().enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.K) && blockTimer >= blockCooldown)
        {
             animator.SetTrigger("block");
            //animator.SetBool("isBlock", true);
            //animator.SetTrigger("block");
            blockTimer = 0;
        }
        //else if (Input.GetKeyUp(KeyCode.K))
        //{
            //animator.SetBool("isBlock", false);
        //}
           
    }

    private bool isInBlock()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer" + ".block"))
        {
            return true;
        }
        return false;
    }

    private bool isAttackArea()
    {
        RaycastHit2D hit = Physics2D.BoxCast(capsuleCollide.bounds.center + transform.right * range * Mathf.Sign(transform.localScale.x) * colliderDistance, 
            new Vector3(capsuleCollide.bounds.size.x * range, capsuleCollide.bounds.size.y, capsuleCollide.bounds.size.z), 0, Vector2.left, .1f, enemyMask);

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(capsuleCollide.bounds.center + transform.right * range * colliderDistance * Mathf.Sign(transform.localScale.x), 
            new Vector3(capsuleCollide.bounds.size.x * range, capsuleCollide.bounds.size.y, capsuleCollide.bounds.size.z));
    }

    private void kill()
    {
        if (isAttackArea())
        {
            if (FindObjectOfType<DeathOfEnemy>() != null)
            FindObjectOfType<DeathOfEnemy>().death();
        }
    }
}

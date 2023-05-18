using UnityEngine;

public class fireBall : MonoBehaviour
{
    [SerializeField] private float speed;

    private Animator anim;
    private BoxCollider2D boxCollider;

    private float direction;
    private float lifeTime;
    private bool hit;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (hit) return;
        float moveSpeed = speed * Time.deltaTime * direction;
        transform.Translate(new Vector3(moveSpeed, 0, 0));

        lifeTime += Time.deltaTime;
        if (lifeTime > 3) gameObject.SetActive(false);
    }

    public void setDirection(float _direct)
    {
        lifeTime = 0;
        direction = _direct;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;
        
        float scaleX = transform.localScale.x;

        if (Mathf.Sign(scaleX) != _direct)
        {
            scaleX = -scaleX;
        }

        transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        boxCollider.enabled = false;
        anim.SetTrigger("Explode");
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}

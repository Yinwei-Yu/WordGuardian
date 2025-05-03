using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float chaseRange = 5f; // 
    [SerializeField] private float moveSpeed = 3f; // 

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Debug.Log("Distance:" + distanceToPlayer);
        if (distanceToPlayer <= chaseRange)
        {
            Debug.Log("Chase!");
            Vector2 direction = (player.position - transform.position).normalized;
            movement = direction;
        }
        else
        {
            movement = Vector2.zero;
        }
    }

    void FixedUpdate()
    {

        if (movement != Vector2.zero)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}

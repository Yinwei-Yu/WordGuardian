using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float chaseRange = 5f; // 检测范围
    [SerializeField] private float moveSpeed = 3f; // 移动速度

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
        // 计算与玩家的距离
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange)
        {
            // 计算朝向玩家的方向
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
        // 移动敌人
        if (movement != Vector2.zero)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    // 可视化检测范围（仅在编辑器可见）
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}

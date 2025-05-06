using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; // 移动速度
    [SerializeField] private Animator animator;   // 动画控制器

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 如果未手动赋值，则尝试自动获取组件
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 获取输入
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // 标准化移动向量以防止斜向移动过快
        movement = movement.normalized;

        // 设置动画状态
        bool isRunning = movement.magnitude > 0; // 如果有输入，则认为在跑步
        if (animator != null)
        {
            animator.SetBool("isRun", isRunning);
        }

        // 根据水平方向调整角色朝向
        UpdateAnimation(movement.x, movement.y);
    }

    void FixedUpdate()
    {
        // 移动角色
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void UpdateAnimation(float x, float y)
    {
        // 更新动画状态
        if (x > 0 && transform.localScale.x < 0) // 向右移动且当前朝左
        {
            // 翻转朝向为右侧
            Vector3 currentScale = transform.localScale;
            transform.localScale = new Vector3(Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        }
        else if (x < 0 && transform.localScale.x > 0) // 向左移动且当前朝右
        {
            // 翻转朝向为左侧
            Vector3 currentScale = transform.localScale;
            transform.localScale = new Vector3(-Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        }

        // 设置动画状态
        animator.SetBool("isRun", x != 0 || y != 0); // 根据是否有水平移动设置跑步状态
    }
}
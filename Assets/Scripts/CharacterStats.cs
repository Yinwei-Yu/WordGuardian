using UnityEngine;
using UnityEngine.Events;

public class CharacterStats : MonoBehaviour
{
    // 当前角色的生命值
    public int health;
    // 角色的最大生命值
    public int maxHealth = 100;
    // 角色的攻击力
    public int attackPower = 10;

    // 当生命值发生变化时触发的事件
    public UnityEvent<int> OnHealthChanged;

    void Start()
    {
        // 初始化生命值为最大生命值
        health = maxHealth;
        // 如果OnHealthChanged事件不为空，则触发该事件
        if (OnHealthChanged != null)
            OnHealthChanged.Invoke(health);
    }

    /// <summary>
    /// 减少角色的生命值
    /// </summary>
    /// <param name="damage">受到的伤害值</param>
    public void DecreaseHealth(int damage)
    {
        // 减少生命值
        health -= damage;
        // 确保生命值不低于0
        if (health < 0) health = 0;
        // 如果OnHealthChanged事件不为空，则触发该事件
        if (OnHealthChanged != null)
            OnHealthChanged.Invoke(health);
    }
}




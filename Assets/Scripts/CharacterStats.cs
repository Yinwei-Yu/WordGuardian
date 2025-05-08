using UnityEngine;
using UnityEngine.Events;

public class CharacterStats : MonoBehaviour
{
    // ��ǰ��ɫ������ֵ
    public int health;
    // ��ɫ���������ֵ
    public int maxHealth = 100;
    // ��ɫ�Ĺ�����
    public int attackPower = 10;

    // ������ֵ�����仯ʱ�������¼�
    public UnityEvent<int> OnHealthChanged;

    void Start()
    {
        // ��ʼ������ֵΪ�������ֵ
        health = maxHealth;
        // ���OnHealthChanged�¼���Ϊ�գ��򴥷����¼�
        if (OnHealthChanged != null)
            OnHealthChanged.Invoke(health);
    }

    /// <summary>
    /// ���ٽ�ɫ������ֵ
    /// </summary>
    /// <param name="damage">�ܵ����˺�ֵ</param>
    public void DecreaseHealth(int damage)
    {
        // ��������ֵ
        health -= damage;
        // ȷ������ֵ������0
        if (health < 0) health = 0;
        // ���OnHealthChanged�¼���Ϊ�գ��򴥷����¼�
        if (OnHealthChanged != null)
            OnHealthChanged.Invoke(health);
    }
}




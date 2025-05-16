using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ����غ�״̬ö��
public enum TurnState { PlayerTurn, EnemyTurn }

public class BattleManager : MonoBehaviour
{
    // ����ģʽʵ��
    public static BattleManager instance;

    // ��ǰ�غ�״̬
    public TurnState currentState = TurnState.PlayerTurn;
    // ��ҽ�ɫ��״̬���
    public CharacterStats playerStats;
    // ���˽�ɫ��״̬���
    public CharacterStats enemyStats;
    // ���Ѫ��������
    public Slider playerHealthSlider;
    // ����Ѫ��������
    public Slider enemyHealthSlider;
    // ��ʾ������ı�
    public TMP_Text questionsText;
    // �ύ��ť
    public Button submitButton;
    // ����𰸵������
    public TMP_InputField inputAnswer;

    // ��ȷ�Ĵ�
    private string correctAnswer = "example"; // ��Ӧ�ø�����Ϸ�߼���̬����

    void Awake()
    {
        // ʵ�ֵ���ģʽ
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // ������ҽ�ɫ������ֵ�仯�¼�
        playerStats.OnHealthChanged.AddListener(UpdatePlayerHealthBar);
        // ���ĵ��˽�ɫ������ֵ�仯�¼�
        enemyStats.OnHealthChanged.AddListener(UpdateEnemyHealthBar);

        // ���³�ʼ����ֵ��ʾ
        UpdatePlayerHealthBar(playerStats.health);
        UpdateEnemyHealthBar(enemyStats.health);

        // �����ύ��ť����¼�
        submitButton.onClick.AddListener(CheckAnswerAndAttack);
    }

    /// <summary>
    /// ���𰸲�ִ�й���
    /// </summary>
    void CheckAnswerAndAttack()
    {
        // �������һغ�
        if (currentState == TurnState.PlayerTurn)
        {
            if (inputAnswer.text.ToLower() == correctAnswer.ToLower())
            {
                // ��������
                Attack(enemyStats);
                //// �л������˻غ�
                //currentState = TurnState.EnemyTurn;
                //// ��������
                //inputAnswer.text = "";
                //// �����µ�����
                //GenerateNewQuestion();
            }
            if(inputAnswer.text.ToLower() != correctAnswer.ToLower())
                // ��������
                inputAnswer.text = "";
            // �л������˻غ�
            currentState = TurnState.EnemyTurn;
            // �������
            Attack(playerStats);
            // �л�����һغ�
            currentState = TurnState.PlayerTurn;
            // �����µ�����
            GenerateNewQuestion();
        }
    }

    /// <summary>
    /// ִ�й�������
    /// </summary>
    /// <param name="target">�������Ľ�ɫ</param>
    void Attack(CharacterStats target)
    {
        // ���ݵ�ǰ�غϼ���Ŀ���ɫ������ֵ
        target.DecreaseHealth(currentState == TurnState.PlayerTurn ? playerStats.attackPower : enemyStats.attackPower);
        // ���Ŀ���ɫ�Ƿ�����
        if (target.health <= 0)
        {
            EndBattle();
        }
    }

    /// <summary>
    /// �������Ѫ����
    /// </summary>
    /// <param name="health">��ҵ�ǰ����ֵ</param>
    public void UpdatePlayerHealthBar(int health)
    {
        // �������Ѫ������ֵ
        playerHealthSlider.value = (float)health / playerStats.maxHealth;
    }

    /// <summary>
    /// ���µ���Ѫ����
    /// </summary>
    /// <param name="health">���˵�ǰ����ֵ</param>
    public void UpdateEnemyHealthBar(int health)
    {
        // ���õ���Ѫ������ֵ
        enemyHealthSlider.value = (float)health / enemyStats.maxHealth;
    }

    /// <summary>
    /// ����ս��
    /// </summary>
    void EndBattle()
    {
        Debug.Log("Battle ended!");
        // �������ս�������߼�
    }

    /// <summary>
    /// �����µ�����
    /// </summary>
    public void GenerateNewQuestion()
    {
        // ʾ��������һ���µ����Ⲣ������ȷ�Ĵ�
        questionsText.text = "What is the capital of France?";
        correctAnswer = "paris";
    }
}




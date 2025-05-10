using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 定义回合状态枚举
public enum TurnState { PlayerTurn, EnemyTurn }

public class BattleManager : MonoBehaviour
{
    // 单例模式实例
    public static BattleManager instance;

    // 当前回合状态
    public TurnState currentState = TurnState.PlayerTurn;
    // 玩家角色的状态组件
    public CharacterStats playerStats;
    // 敌人角色的状态组件
    public CharacterStats enemyStats;
    // 玩家血量条滑块
    public Slider playerHealthSlider;
    // 敌人血量条滑块
    public Slider enemyHealthSlider;
    // 显示问题的文本
    public TMP_Text questionsText;
    // 提交按钮
    public Button submitButton;
    // 输入答案的输入框
    public TMP_InputField inputAnswer;

    // 正确的答案
    private string correctAnswer = "example"; // 这应该根据游戏逻辑动态设置

    void Awake()
    {
        // 实现单例模式
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
        // 订阅玩家角色的生命值变化事件
        playerStats.OnHealthChanged.AddListener(UpdatePlayerHealthBar);
        // 订阅敌人角色的生命值变化事件
        enemyStats.OnHealthChanged.AddListener(UpdateEnemyHealthBar);

        // 更新初始生命值显示
        UpdatePlayerHealthBar(playerStats.health);
        UpdateEnemyHealthBar(enemyStats.health);

        // 监听提交按钮点击事件
        submitButton.onClick.AddListener(CheckAnswerAndAttack);
    }

    /// <summary>
    /// 检查答案并执行攻击
    /// </summary>
    void CheckAnswerAndAttack()
    {
        // 如果是玩家回合
        if (currentState == TurnState.PlayerTurn)
        {
            if (inputAnswer.text.ToLower() == correctAnswer.ToLower())
            {
                // 攻击敌人
                Attack(enemyStats);
                //// 切换到敌人回合
                //currentState = TurnState.EnemyTurn;
                //// 清空输入框
                //inputAnswer.text = "";
                //// 生成新的问题
                //GenerateNewQuestion();
            }
            if(inputAnswer.text.ToLower() != correctAnswer.ToLower())
                // 清空输入框
                inputAnswer.text = "";
            // 切换到敌人回合
            currentState = TurnState.EnemyTurn;
            // 攻击玩家
            Attack(playerStats);
            // 切换到玩家回合
            currentState = TurnState.PlayerTurn;
            // 生成新的问题
            GenerateNewQuestion();
        }
    }

    /// <summary>
    /// 执行攻击操作
    /// </summary>
    /// <param name="target">被攻击的角色</param>
    void Attack(CharacterStats target)
    {
        // 根据当前回合减少目标角色的生命值
        target.DecreaseHealth(currentState == TurnState.PlayerTurn ? playerStats.attackPower : enemyStats.attackPower);
        // 检查目标角色是否死亡
        if (target.health <= 0)
        {
            EndBattle();
        }
    }

    /// <summary>
    /// 更新玩家血量条
    /// </summary>
    /// <param name="health">玩家当前生命值</param>
    public void UpdatePlayerHealthBar(int health)
    {
        // 设置玩家血量条的值
        playerHealthSlider.value = (float)health / playerStats.maxHealth;
    }

    /// <summary>
    /// 更新敌人血量条
    /// </summary>
    /// <param name="health">敌人当前生命值</param>
    public void UpdateEnemyHealthBar(int health)
    {
        // 设置敌人血量条的值
        enemyHealthSlider.value = (float)health / enemyStats.maxHealth;
    }

    /// <summary>
    /// 结束战斗
    /// </summary>
    void EndBattle()
    {
        Debug.Log("Battle ended!");
        // 添加其他战斗结束逻辑
    }

    /// <summary>
    /// 生成新的问题
    /// </summary>
    public void GenerateNewQuestion()
    {
        // 示例：生成一个新的问题并设置正确的答案
        questionsText.text = "What is the capital of France?";
        correctAnswer = "paris";
    }
}




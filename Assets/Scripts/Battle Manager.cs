using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BattleManager.cs
// BattleManager.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text QuestionsText;
    [SerializeField] private TMP_InputField InputAnswer;
    [SerializeField] private Image WizardBlood;
    [SerializeField] private Image EnemyBlood;
    [SerializeField] private TMP_Text WizardName;
    [SerializeField] private TMP_Text EnemyName;

    [Header("战斗设置")]
    [SerializeField] private int wizardMaxHP = 100;
    [SerializeField] private int enemyMaxHP = 150;
    [SerializeField] private int baseDamage = 10;
    [SerializeField] private int wordBonusMultiplier = 3;

    private int wizardCurrentHP;
    private int enemyCurrentHP;
    private bool isPlayerTurn = true;

    void Start()
    {
        InitializeBattle();
        SetupUI();
    }

    void InitializeBattle()
    {
        wizardCurrentHP = wizardMaxHP;
        enemyCurrentHP = enemyMaxHP;
    }

    void SetupUI()
    {
        WizardName.text = "魔法师";
        EnemyName.text = "邪恶巫师";
        UpdateHealthBars();
    }

    // 由SubmitButton调用
    public void OnAnswerSubmit()
    {
        if (!isPlayerTurn) return;

        string answer = InputAnswer.text.Trim().ToLower();
        ValidateAnswer(answer);
        InputAnswer.text = "";
    }

    void ValidateAnswer(string answer)
    {
        // TODO: 连接你的单词验证系统
        bool isCorrect = CheckWordCorrectness(answer);

        if (isCorrect)
        {
            int damage = CalculateDamage(answer);
            StartCoroutine(PlayerAttackSequence(damage));
        }
        else
        {
            StartCoroutine(WrongAnswerPenalty());
        }
    }

    IEnumerator PlayerAttackSequence(int damage)
    {
        // 播放玩家攻击动画
        GetComponent<Animator>().SetTrigger("WizardAttack");

        yield return new WaitForSeconds(0.8f); // 等待攻击动画完成

        enemyCurrentHP = Mathf.Clamp(enemyCurrentHP - damage, 0, enemyMaxHP);
        UpdateHealthBars();

        QuestionsText.text = $"正确！造成 {damage} 点伤害！";

        if (enemyCurrentHP <= 0)
        {
            EndBattle(true);
            yield break;
        }

        yield return new WaitForSeconds(1.5f);
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        isPlayerTurn = false;
        QuestionsText.text = "敌人的回合...";

        yield return new WaitForSeconds(1f);

        // 播放敌人攻击动画
        GetComponent<Animator>().SetTrigger("EnemyAttack");

        yield return new WaitForSeconds(0.8f);

        int damage = baseDamage + Random.Range(-2, 3);
        wizardCurrentHP = Mathf.Clamp(wizardCurrentHP - damage, 0, wizardMaxHP);
        UpdateHealthBars();

        QuestionsText.text = $"敌人造成 {damage} 点伤害！";

        if (wizardCurrentHP <= 0)
        {
            EndBattle(false);
            yield break;
        }

        yield return new WaitForSeconds(1.5f);

        isPlayerTurn = true;
        QuestionsText.text = "你的回合！输入答案：";
    }

    IEnumerator WrongAnswerPenalty()
    {
        QuestionsText.text = "拼写错误！";
        yield return new WaitForSeconds(1f);
        StartCoroutine(EnemyTurn());
    }

    void UpdateHealthBars()
    {
        WizardBlood.fillAmount = (float)wizardCurrentHP / wizardMaxHP;
        EnemyBlood.fillAmount = (float)enemyCurrentHP / enemyMaxHP;
    }

    int CalculateDamage(string word)
    {
        return baseDamage + (word.Length * wordBonusMultiplier);
    }

    bool CheckWordCorrectness(string word)
    {
        // 示例验证逻辑，需要替换为实际单词验证
        return word.Length > 3; // 临时逻辑：单词长度大于3视为正确
    }

    void EndBattle(bool isVictory)
    {
        QuestionsText.text = isVictory ? "战斗胜利！" : "战斗失败...";
        InputAnswer.interactable = false;
    }
}

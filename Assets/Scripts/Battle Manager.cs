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

    [Header("ս������")]
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
        WizardName.text = "ħ��ʦ";
        EnemyName.text = "а����ʦ";
        UpdateHealthBars();
    }

    // ��SubmitButton����
    public void OnAnswerSubmit()
    {
        if (!isPlayerTurn) return;

        string answer = InputAnswer.text.Trim().ToLower();
        ValidateAnswer(answer);
        InputAnswer.text = "";
    }

    void ValidateAnswer(string answer)
    {
        // TODO: ������ĵ�����֤ϵͳ
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
        // ������ҹ�������
        GetComponent<Animator>().SetTrigger("WizardAttack");

        yield return new WaitForSeconds(0.8f); // �ȴ������������

        enemyCurrentHP = Mathf.Clamp(enemyCurrentHP - damage, 0, enemyMaxHP);
        UpdateHealthBars();

        QuestionsText.text = $"��ȷ����� {damage} ���˺���";

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
        QuestionsText.text = "���˵Ļغ�...";

        yield return new WaitForSeconds(1f);

        // ���ŵ��˹�������
        GetComponent<Animator>().SetTrigger("EnemyAttack");

        yield return new WaitForSeconds(0.8f);

        int damage = baseDamage + Random.Range(-2, 3);
        wizardCurrentHP = Mathf.Clamp(wizardCurrentHP - damage, 0, wizardMaxHP);
        UpdateHealthBars();

        QuestionsText.text = $"������� {damage} ���˺���";

        if (wizardCurrentHP <= 0)
        {
            EndBattle(false);
            yield break;
        }

        yield return new WaitForSeconds(1.5f);

        isPlayerTurn = true;
        QuestionsText.text = "��Ļغϣ�����𰸣�";
    }

    IEnumerator WrongAnswerPenalty()
    {
        QuestionsText.text = "ƴд����";
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
        // ʾ����֤�߼�����Ҫ�滻Ϊʵ�ʵ�����֤
        return word.Length > 3; // ��ʱ�߼������ʳ��ȴ���3��Ϊ��ȷ
    }

    void EndBattle(bool isVictory)
    {
        QuestionsText.text = isVictory ? "ս��ʤ����" : "ս��ʧ��...";
        InputAnswer.interactable = false;
    }
}

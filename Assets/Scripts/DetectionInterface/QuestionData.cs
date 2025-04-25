using System;

[Serializable]
public class QuestionData
{
    public string questionText;
    public string optionA;
    public string optionB;
    public string optionC;
    public int correctOption; // 1=A, 2=B, 3=C
}

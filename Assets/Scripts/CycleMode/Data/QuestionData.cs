// QuestionData.cs
[System.Serializable]
public class QuestionData
{
    public string word;
    public string sentence;
    public string explanation;
    public string correctAnswer;
}

[System.Serializable]
public class QuestionList
{
    public List<QuestionData> questions;
}
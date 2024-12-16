using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;

public struct MCQuestion
{
    public string Question;
    public AnswerOption[] AnswerOptions;
}

public struct AnswerOption
{
    public string Answer;
    public bool IsCorrect;
}

[Serializable]
public class Wrapper<T>
{
    public T[] Items;
}

public class MCQuestionManager : MonoBehaviour
{
    private Blackboard _blackboard;

    private List<MCQuestion> _mcQuestions;

    private int currentQuestion;

    private List<bool[]> currentlyCheckedAnswers;

    public MCQuestionManager(string jsonFilepath, Blackboard blackboard)
    {
        _mcQuestions = new List<MCQuestion>();

        //try loading Questions; there can be exercises without any questions, where the list remains empty
        try
        {
            string jsonString = Resources.Load<TextAsset>(jsonFilepath).text;
            _mcQuestions = JsonConvert.DeserializeObject<List<MCQuestion>>(jsonString);
        }
        catch(Exception e) { }

        currentlyCheckedAnswers = new List<bool[]>();
        foreach(var q in _mcQuestions)
        {
            currentlyCheckedAnswers.Add(new bool[q.AnswerOptions.Length]);
        }

        _blackboard = blackboard;

        _blackboard.onCheckedAnswersChanged = UpdateCheckedAnswers;
    }

    public void GoToQuestion(int index)
    {
        currentQuestion = index;

        string question = _mcQuestions[index].Question;
        string[] answerOptions = _mcQuestions[index].AnswerOptions.Select(o => o.Answer).ToArray();

        _blackboard.ShowMCQuestion(question, answerOptions, currentlyCheckedAnswers[index]);
    }

    public void ReviewQuestion(int index)
    {
        currentQuestion = index;

        string question = _mcQuestions[index].Question;
        string[] answerOptions = _mcQuestions[index].AnswerOptions.Select(o => o.Answer).ToArray();
        bool[] correctAnswers = _mcQuestions[index].AnswerOptions.Select(o => o.IsCorrect).ToArray();

        _blackboard.ShowMCQuestionReview(question, answerOptions, currentlyCheckedAnswers[index], correctAnswers);
    }

    public void UpdateCheckedAnswers(bool[] checkedAnswers)
    {
        currentlyCheckedAnswers[currentQuestion] = checkedAnswers;
    }

    public int CalculatePoints()
    {
        int points = 0;

        for(int i = 0; i < _mcQuestions.Count; i++)
        {
            for(int j = 0; j < _mcQuestions[i].AnswerOptions.Length; j++)
            {
                if (_mcQuestions[i].AnswerOptions[j].IsCorrect == currentlyCheckedAnswers[i][j])
                    points++;
            }
        }

        return points;
    }

    public int GetMaxPoints()
    {
        int points = 0;
        foreach(var q in _mcQuestions)
        {
            points += q.AnswerOptions.Length;
        }

        return points;
    }
}

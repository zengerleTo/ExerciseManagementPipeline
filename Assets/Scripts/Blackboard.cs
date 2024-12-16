using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Blackboard : MonoBehaviour
{
    [SerializeField] Text QuestionTextField;

    [SerializeField] Toggle[] AnswerCheckboxes;

    [SerializeField] Button NextButton;
    [SerializeField] Button PreviousButton;

    [SerializeField] Button FinishButton;
    [SerializeField] Text EndPanelTextField;

    [SerializeField] Text InfoTextField;

    [SerializeField] GameObject InfoView;
    [SerializeField] GameObject MCQuestionView;
    [SerializeField] GameObject EndPanelView;

    [SerializeField] Color StandardTextColor = Color.white;
    [SerializeField] Color CorrectAnswerColor = Color.green;
    [SerializeField] Color IncorrectAnswerColor = Color.red;

    public delegate void OnBlackboardButtonPressed();
    public OnBlackboardButtonPressed onNextPressed;
    public OnBlackboardButtonPressed onPreviousPressed;
    public OnBlackboardButtonPressed onFinishPressed;

    public delegate void OnCheckedAnswersChanged(bool[] checkedAnswers);
    public OnCheckedAnswersChanged onCheckedAnswersChanged;

    public enum BlackboardView { Info, MCQuestion, EndPanel }
    private BlackboardView view;

    private void Awake()
    {
        view = BlackboardView.MCQuestion;
        SwapToMCQuestionView();

        NextButton.onClick.AddListener(Next);
        PreviousButton.onClick.AddListener(Previous);
        FinishButton.onClick.AddListener(Finish);

        foreach(var ch in AnswerCheckboxes)
        {
            ch.onValueChanged.AddListener((bool b) => UpdateCheckedAnswers());
        }
    }

    public void ShowInfo(string infoText)
    {
        if (view != BlackboardView.Info)
            SwapToInfoView();

        InfoTextField.text = infoText;
    }

    public void ShowMCQuestion(string question, string[] answerOptions, bool[] presetCheckboxValues)
    {
        if (view != BlackboardView.MCQuestion)
            SwapToMCQuestionView();

        QuestionTextField.text = question;

        for(int i = 0; i < answerOptions.Length; i++)
        {
            AnswerCheckboxes[i].isOn = presetCheckboxValues[i];
            AnswerCheckboxes[i].GetComponentInChildren<Text>().text = answerOptions[i];

            AnswerCheckboxes[i].interactable = true;
            AnswerCheckboxes[i].GetComponentInChildren<Text>().color = StandardTextColor;
        }
    }

    public void ShowMCQuestionReview(string question, string[] answerOptions, bool[] presetCheckboxValues, bool[] correctAnswers)
    {
        ShowMCQuestion(question, answerOptions, presetCheckboxValues);

        for (int i = 0; i < AnswerCheckboxes.Length; i++)
        {
            AnswerCheckboxes[i].interactable = false;
            AnswerCheckboxes[i].GetComponentInChildren<Text>().color = correctAnswers[i] ? CorrectAnswerColor : IncorrectAnswerColor;
        }
    }

    public void ShowEndPanel(string endPanelText, bool finishButtonEnabled)
    {
        if (view != BlackboardView.EndPanel)
            SwapToEndPanelView();

        FinishButton.gameObject.SetActive(finishButtonEnabled);
        EndPanelTextField.text = endPanelText;
    }

    private void SwapToInfoView()
    {
        MCQuestionView.SetActive(false);
        EndPanelView.SetActive(false);

        InfoView.SetActive(true);
        view = BlackboardView.Info;
    }

    private void SwapToMCQuestionView()
    {
        InfoView.SetActive(false);
        EndPanelView.SetActive(false);

        MCQuestionView.SetActive(true);
        view = BlackboardView.MCQuestion;
    }

    private void SwapToEndPanelView()
    {
        InfoView.SetActive(false);
        MCQuestionView.SetActive(false);

        EndPanelView.SetActive(true);
        view = BlackboardView.EndPanel;
    }

    public void NextButtonSetActive(bool active)
    {
        NextButton.gameObject.SetActive(active);
    }

    public void PreviousButtonSetActive(bool active)
    {
        PreviousButton.gameObject.SetActive(active);
    }

    public BlackboardView GetCurrentView()
    {
        return view;
    }

    private void UpdateCheckedAnswers()
    {
        bool[] checkedAnswers = AnswerCheckboxes.Select(chb => chb.isOn).ToArray();

        onCheckedAnswersChanged(checkedAnswers);
    }

    public void Next()
    {
        onNextPressed();
    }

    public void Previous()
    {
        onPreviousPressed();
    }

    public void Finish()
    {
        onFinishPressed();
    }
}

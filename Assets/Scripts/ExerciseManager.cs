using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StepAllowed { Always, OnlyInWorkingPhase, OnlyInReviewPhase }

public struct ExerciseStep
{
    public Action action;
    public StepAllowed allowed;

    public ExerciseStep(Action a)
    {
        action = a;
        allowed = StepAllowed.Always;
    }

    public ExerciseStep(Action a, StepAllowed sa)
    {
        action = a;
        allowed = sa;
    }
}

public abstract class ExerciseManager : MonoBehaviour
{
    protected List<ExerciseStep> Steps;
    protected MCQuestionManager questionManager;

    [SerializeField] protected Blackboard blackboard;
    [SerializeField] protected string questionFilepath;

    protected enum ExercisePhase { Working, Review }
    protected ExercisePhase exercisePhase = ExercisePhase.Working;

    int currentStep;

    [SerializeField] protected float passingPercentage = 0.75f; //percentage of points that need to be reached before the student can move on to the next exercise
    protected bool firstTry = true;

    public void BeginExercise()
    {
        questionManager = new MCQuestionManager(questionFilepath, blackboard);

        PrepareSimulation();
        InitializeSteps();

        FirstStep(false);
    }

    public void EndExercise()
    {
        CleanupSimulation();
    }

    private void FirstStep(bool performReset)
    {
        if (performReset)
            ResetSimulation();

        int firstStep = 0;

        while (firstStep < Steps.Count)
        {
            if (StepIsCurrentlyAllowed(firstStep))
            {
                currentStep = firstStep;

                UpdateNavigationButtons();
                Steps[currentStep].action();
                return;
            }

            firstStep++;
        }
    }

    public void NextStep()
    {
        int nextStep = currentStep + 1;
        while (nextStep < Steps.Count)
        {
            if(StepIsCurrentlyAllowed(nextStep))
            {
                currentStep = nextStep;

                UpdateNavigationButtons();
                Steps[currentStep].action();
                return;
            }

            nextStep++;
        }
    }

    public void PreviousStep()
    {
        //if you're in the end panel, go back to the beginning
        if(blackboard.GetCurrentView() == Blackboard.BlackboardView.EndPanel)
        {
            FirstStep(true);
            return;
        }

        //otherwise go back one step
        int previousStep = currentStep - 1;
        while (previousStep >= 0)
        {
            if (StepIsCurrentlyAllowed(previousStep))
            {
                currentStep = previousStep;

                UpdateNavigationButtons();
                Steps[currentStep].action();
                return;
            }

            previousStep--;
        }
    }

    private bool StepIsCurrentlyAllowed(int index)
    {
        switch (Steps[index].allowed)
        {
            case StepAllowed.Always:
                return true;
            case StepAllowed.OnlyInWorkingPhase:
                return exercisePhase == ExercisePhase.Working;
            case StepAllowed.OnlyInReviewPhase:
                return exercisePhase == ExercisePhase.Review;
            default:
                return false;
        }
    }

    private void UpdateNavigationButtons()
    {
        //check if there are any steps left after the current one, that are allowed for the currently active exercise phase
        bool nextButtonActive = false;

        int potentialNextStep = currentStep + 1;
        while (potentialNextStep < Steps.Count)
        {
            if (StepIsCurrentlyAllowed(potentialNextStep))
            {
                nextButtonActive = true;
                break;
            }
            potentialNextStep++;
        }

        blackboard.NextButtonSetActive(nextButtonActive);

        //check if there are any steps before the current one, that are allowed for the currently active exercise phase
        bool previousButtonActive = false;

        int potentialPreviousStep = currentStep - 1;
        while (potentialPreviousStep >= 0)
        {
            if (StepIsCurrentlyAllowed(potentialPreviousStep))
            {
                previousButtonActive = true;
                break;
            }
            potentialPreviousStep--;
        }

        blackboard.PreviousButtonSetActive(previousButtonActive);
    }

    #region Basic Blackboard Actions

    protected void ShowInfo(string info)
    {
        blackboard.ShowInfo(info);
    }

    protected void GoToQuestion(int index)
    {
        //Debug.Log("GoToQuestion " + index);
        if (exercisePhase == ExercisePhase.Working)
            questionManager.GoToQuestion(index);

        else if (exercisePhase == ExercisePhase.Review)
            questionManager.ReviewQuestion(index);
    }

    public void GoToResults()
    {
        int points = questionManager.CalculatePoints();
        int maxPoints = questionManager.GetMaxPoints();
        bool passed = points >= maxPoints * passingPercentage;

        string resultText = "" + points + "/" + maxPoints + " Punkte erreicht\n\n";

        if (firstTry && passed)
        {
            resultText += "Sie haben die Bestehensgrenze für diese Aufgabe erreicht.\n\n\n\n\n\n\nMit einem Klick auf \"Zurück\" können Sie die vollständigen Lösungen einsehen oder sie können direkt zur nächsten Aufgabe übergehen.";
        }
        else if (firstTry && !passed)
        {
            resultText += "Sie haben die Bestehensgrenze für diese Aufgabe noch nicht erreicht.\n\n\n\n\n\n\nKlicken Sie auf \"Zurück\" um ihre Antworten noch einmal zu bearbeiten.";
        }
        else if (!firstTry)
        {
            //allow user to finish the question set after the second try, even when point threshold is still not met
            resultText += "Aufgabe abgeschlossen!\n\n\n\n\n\n\nMit einem Klick auf \"Zurück\" können Sie die vollständigen Lösungen einsehen oder sie können direkt zur nächsten Aufgabe übergehen.";
            passed = true;
        }

        if (passed)
            exercisePhase = ExercisePhase.Review;

        blackboard.ShowEndPanel(resultText, passed);

        firstTry = false;
    }

    #endregion

    #region abstract methods

    /// <summary>
    /// Populates the Step list with custom actions for this exercise
    /// </summary>
    protected abstract void InitializeSteps();

    /// <summary>
    /// Activates all neccessary components and sets all neccessary variables in the simulation in preparation for the exercise
    /// </summary>
    protected abstract void PrepareSimulation();

    /// <summary>
    /// Cleans up all changes made to the simulation and returns it to a neutral state after the end of the exercise
    /// </summary>
    protected abstract void CleanupSimulation();

    /// <summary>
    /// Resets the state of the simulation to the beginning of the exercise
    /// </summary>
    protected abstract void ResetSimulation();

    #endregion
}

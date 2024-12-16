using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WelcomeExerciseManager : ExerciseManager
{
    [SerializeField] Text FinishButtonText;

    protected override void PrepareSimulation()
    {
        FinishButtonText.text = "Start";
    }

    protected override void CleanupSimulation()
    {
        FinishButtonText.text = "Nächste Aufgabe";
    }

    protected override void ResetSimulation()
    {
        //Nothing to do
    }

    protected override void InitializeSteps()
    {
        Steps = new List<ExerciseStep>();

        //here you define all the steps of this exercise and add them to the step list
        Steps.Add(new ExerciseStep(() =>
        {
            string welcomeMessage = "Willkommen in der VR Anwendung des LIN Praktikums!\n\n\nZeigen Sie mit einem Ihrer Controller auf diese Tafel und drücken Sie den Knopf unter Ihrem Zeigefinger um den \"Start\" Button zu betätigen.";
            blackboard.ShowEndPanel(welcomeMessage, true);
        }
        ));
    }
}

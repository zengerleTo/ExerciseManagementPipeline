using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MCExplanationExercise : ExerciseManager
{
    [SerializeField] Text FinishButtonText;

    protected override void PrepareSimulation()
    {
        FinishButtonText.text = "Aufgaben beginnen";
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

        Steps.Add(new ExerciseStep(() =>
        {
            string message = "Im Folgenden werden MC-Fragen und Anwendungsaufgaben gestellt. Während einer Versuchsreihe kann zwischen den Aufgaben hin und her gesprungen werden. Bei den MC-Fragen ist stets mindestens eine Aussage richtig. Jedes richtig gesetzte Kreuz (oder nicht Kreuz) gibt einen Punkt, bis zu einem Maximum von 5 Punkten. Es müssen mindestens 70% der gesamt möglichen Punkte erreicht werden, um die Aufgabe zu bestehen. Wird diese Grenze nicht erreicht müssen die Fragen erneut beantwortet werden.";
            blackboard.ShowEndPanel(message, true);
        }
        ));
    }
}

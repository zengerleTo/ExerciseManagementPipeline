using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exercise0Manager : ExerciseManager
{

    float distanceThreshold = 0.001f;

    protected override void PrepareSimulation()
    {
        //todo
    }

    protected override void CleanupSimulation()
    {
        //todo
    }

    protected override void ResetSimulation()
    {
        //todo
    }

    protected override void InitializeSteps()
    {
        Steps = new List<ExerciseStep>();

        Steps.Add(new ExerciseStep(() =>
        {
            ShowInfo("Aufgabe 1 - Grundlegender Abbildungsvorgang im Auge");
        },
        StepAllowed.OnlyInWorkingPhase));

        bool firstVisit = true;

        string firstInstruction = "1.1 Verschieben Sie den Gegenstand auf der optischen Achse, bis auf der Netzhaut ein scharfes Bild entsteht.\n\nHierzu müssen Sie Ihre virtuelle Hand in den Gegenstand bewegen und den Zeigefinger-Knopf gedrückt halten um zuzugreifen.";
        string firstInstructionCompleted = "Abgeschlossen\n\n" + firstInstruction;


        Steps.Add(new ExerciseStep(() =>
        {
            if (firstVisit)
            {
                ShowInfo(firstInstruction);
                blackboard.PreviousButtonSetActive(false);
                blackboard.NextButtonSetActive(false);
                //lensManager.lensSimulationStepExecuted += SharpImageCheck;

                firstVisit = false;
            }
            else
            {
                ShowInfo(firstInstructionCompleted);
            }
        },
        StepAllowed.OnlyInWorkingPhase));

        Steps.Add(new ExerciseStep(() =>
        {
            //blackboard.PreviousButtonSetActive(false);

            ShowInfo("1.2 Verschieben Sie erneut den Gegenstand auf der optischen Achse. Stellen Sie die größtmögliche Gegenstandsweite ein. Was kann beobachtet werden?");
        },
        StepAllowed.OnlyInWorkingPhase));

        Steps.Add(new ExerciseStep(() =>
        {
            GoToQuestion(0);

        }));

        Steps.Add(new ExerciseStep(() =>
        {
            GoToQuestion(1);

        }));

        Steps.Add(new ExerciseStep(() =>
        {
            GoToQuestion(2);

        }));

        Steps.Add(new ExerciseStep(() =>
        {
            GoToQuestion(3);

        }));

        Steps.Add(new ExerciseStep(() =>
        {
            GoToQuestion(4);

        }));

        Steps.Add(new ExerciseStep(() =>
        {
            GoToQuestion(5);

        }));

        Steps.Add(new ExerciseStep(() =>
        {
            GoToResults();
        }
        ));
    }

    /*
    public void SharpImageCheck()
    {
        var dist = (theoreticalImage.position - retinalImage.position).magnitude;

        if (dist <= distanceThreshold)
        {
            blackboard.PreviousButtonSetActive(true);
            blackboard.NextButtonSetActive(true);
            lensManager.lensSimulationStepExecuted -= SharpImageCheck;
        }
    }
    */
}

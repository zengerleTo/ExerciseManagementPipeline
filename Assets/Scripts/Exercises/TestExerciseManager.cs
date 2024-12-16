using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestExerciseManager : ExerciseManager
{

    
    protected override void PrepareSimulation()
    {
        //TODO
    }

    protected override void CleanupSimulation()
    {
        //TODO
    }

    protected override void ResetSimulation()
    {
        //TODO
    }

    protected override void InitializeSteps()
    {
        Steps = new List<ExerciseStep>();

        //here you define all the steps of this exercise and add them to the step list
        Steps.Add(new ExerciseStep(() =>
        {
            ShowInfo("Test Exercise\n\nThis panel is only active during working phase");
        },
        StepAllowed.OnlyInWorkingPhase));

        Steps.Add(new ExerciseStep(() =>
        {
            GoToQuestion(0);
        }
        ));

        Steps.Add(new ExerciseStep(() =>
        {
            ShowInfo("Additional Information\n\nThis panel is only active during review phase");
        },
        StepAllowed.OnlyInReviewPhase));

        Steps.Add(new ExerciseStep(() =>
        {
            GoToQuestion(1);
        }
        ));

        Steps.Add(new ExerciseStep(() =>
        {
            GoToResults();
        }
        ));
    }
}

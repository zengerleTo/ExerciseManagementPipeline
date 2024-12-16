using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExerciseMaster : MonoBehaviour
{
    [SerializeField] Blackboard blackboard;
    [SerializeField] ExerciseManager[] exercises;

    int activeExercise;

    void Start()
    {
        blackboard.onFinishPressed += FinishCurrentExercise;

        exercises[0].BeginExercise();
        blackboard.onNextPressed += exercises[0].NextStep;
        blackboard.onPreviousPressed += exercises[0].PreviousStep;
        activeExercise = 0;
    }

    private void FinishCurrentExercise()
    {
        if (activeExercise == exercises.Length - 1)
            return;

        blackboard.onNextPressed -= exercises[activeExercise].NextStep;
        blackboard.onPreviousPressed -= exercises[activeExercise].PreviousStep;
        exercises[activeExercise].EndExercise();

        activeExercise++;

        exercises[activeExercise].BeginExercise();
        blackboard.onNextPressed += exercises[activeExercise].NextStep;
        blackboard.onPreviousPressed += exercises[activeExercise].PreviousStep;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarewellExerciseManager : ExerciseManager
{
    protected override void PrepareSimulation()
    {
        //nothing to do
    }

    protected override void CleanupSimulation()
    {
        //nothing to do
    }

    protected override void ResetSimulation()
    {
        //nothing to do
    }

    protected override void InitializeSteps()
    {
        Steps = new List<ExerciseStep>();

        Steps.Add(new ExerciseStep(() =>
        {
            string farewellMessage = "Sie haben alle Aufgaben erfolgreich abgeschlossen!\n\nWenn Sie möchten, können Sie noch in der Anwendung bleiben und sich weiter mit der Augensimulation beschäftigen.\nAlle Funktionen der einzelnen Aufgaben sind nun in der Simulation aktiviert.";
            blackboard.ShowEndPanel(farewellMessage, false);
        }
        ));
    }
}

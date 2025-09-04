using UnityEngine;

public class AiSimulationRunner : MonoBehaviour
{

    private void Update()
    {
        var boot = AiWorldBootstrap.Instance;
        if (boot != null) 
            boot.Simulation.Step(Time.deltaTime);
    }

}

using GaoalAi.Unity;
using GoalAI.Core;
using UnityEngine;
using UnityEngine.UIElements;

public class AiWorldBootstrap :MonoBehaviour
{
    public static AiWorldBootstrap Instance { get; private set; } = null;

    public World World { get; private set; } = null;

    public Simulation Simulation { get; private set; } = null;

    [Min(0.01f)]
    public float replanInterval = 0.25f;

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        {
            Destroy(gameObject); 
            return; 
        }
        Instance = this;

        World = new World();
        Simulation = new Simulation(World, replanInterval, new UnityAiLogger());
    }

}

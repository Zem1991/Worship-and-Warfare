using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSC : AbstractSingleton<GameSC>, ISceneController
{
    [Header("Local singletons")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ScenarioManager scenarioManager;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private UIManager uiManager;

    public override void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        scenarioManager = FindObjectOfType<ScenarioManager>();
        playerManager = FindObjectOfType<PlayerManager>();
        inputManager = FindObjectOfType<InputManager>();
        uiManager = FindObjectOfType<UIManager>();

        base.Awake();
    }

    public IEnumerator WaitForSceneLoad()
    {
        yield return
            GameManager.CheckReference(gameManager) &&
            ScenarioManager.CheckReference(scenarioManager) &&
            PlayerManager.CheckReference(playerManager) &&
            InputManager.CheckReference(inputManager) &&
            UIManager.CheckReference(uiManager);
    }

    public void HideScene()
    {
        Debug.LogWarning(GetType() + " doesn't have objects to hide");
    }

    public void ShowScene()
    {
        Debug.LogWarning(GetType() + " doesn't have objects to show");
    }
}

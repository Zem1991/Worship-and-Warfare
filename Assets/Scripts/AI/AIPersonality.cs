using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StrategicAI))]
[RequireComponent(typeof(FieldTacticalAI))]
[RequireComponent(typeof(FieldOperationalAI))]
[RequireComponent(typeof(CombatTacticalAI))]
[RequireComponent(typeof(CombatOperationalAI))]
[RequireComponent(typeof(TownOperationalAI))]
public class AIPersonality : MonoBehaviour
{
    [Header("Player data")]
    public Player player;

    [Header("Approach")]
    public int approachCautious;
    public int approachOpportunist;
    public int approachReckless;

    [Header("Economy")]
    public int economyTrader;
    public int economyExplorer;
    public int economyBuilder;
    public int economyRaider;

    [Header("Warfare")]
    public int warfareGuerrilla;
    public int warfareRusher;
    public int warfareTurtle;
    public int warfareSteamroll;

    [Header("AI status")]
    public bool isInitialized;
    public bool isRunning;

    private StrategicAI strategicAI;
    private FieldTacticalAI fieldTacticalAI;
    private FieldOperationalAI fieldOperationalAI;
    private CombatTacticalAI combatTacticalAI;
    private CombatOperationalAI combatOperationalAI;
    private TownOperationalAI townOperationalAI;

    public void Awake()
    {
        strategicAI = GetComponent<StrategicAI>();
        fieldTacticalAI = GetComponent<FieldTacticalAI>();
        fieldOperationalAI = GetComponent<FieldOperationalAI>();
        combatTacticalAI = GetComponent<CombatTacticalAI>();
        combatOperationalAI = GetComponent<CombatOperationalAI>();
        townOperationalAI = GetComponent<TownOperationalAI>();
    }

    public void Initialize(Player player)
    {
        this.player = player;

        Debug.Log("Pretend player " + player.playerName + " got its AI Personality initialized.");
        isInitialized = true;
    }

    public void RunAI()
    {
        Debug.Log("AI is running for player " + player.playerName);
        isRunning = true;
    }

    public void FieldRoutine()
    {
        strategicAI.FullRoutine();
        fieldTacticalAI.FullRoutine();
        fieldOperationalAI.FullRoutine();
    }

    public void CombatRoutine()
    {
        strategicAI.FullRoutine();
        combatTacticalAI.FullRoutine();
        combatOperationalAI.FullRoutine();
    }

    public void TownRoutine()
    {
        strategicAI.FullRoutine();
        fieldTacticalAI.FullRoutine();
        townOperationalAI.FullRoutine();
    }
}

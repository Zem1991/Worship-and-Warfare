using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : AbstractSingleton<SceneLoader>
{
    public const string MAIN = "Main";
    public const string DATABASE = "Database";
    public const string GAME = "Game";
    public const string FIELD = "Game - Field";
    public const string COMBAT = "Game - Combat";
    public const string TOWN = "Game - Town";

    [Header("Other scenes")]
    [SerializeField] private Scene sceneMain;
    [SerializeField] private Scene sceneDatabase;
    [SerializeField] private Scene sceneGame;
    [SerializeField] private Scene sceneField;
    [SerializeField] private Scene sceneCombat;
    [SerializeField] private Scene sceneTown;

    public IEnumerator LoadScene_Main()
    {
        sceneMain = SceneManager.GetSceneByName(MAIN);
        if (sceneMain.handle == 0)
        {
            yield return SceneManager.LoadSceneAsync(MAIN, LoadSceneMode.Additive);
            yield return Main.Instance.WaitForSceneLoad();
        }
        else
        {
            Debug.Log("Scene " + MAIN + " being reused.");
        }
        sceneMain = SceneManager.GetSceneByName(MAIN);
    }

    public IEnumerator LoadScene_Database()
    {
        sceneDatabase = SceneManager.GetSceneByName(DATABASE);
        if (sceneDatabase.handle == 0)
        {
            yield return SceneManager.LoadSceneAsync(DATABASE, LoadSceneMode.Additive);
            yield return DatabaseSC.Instance.WaitForSceneLoad();
        }
        else
        {
            Debug.Log("Scene " + DATABASE + " being reused.");
        }
        sceneDatabase = SceneManager.GetSceneByName(DATABASE);
    }

    public IEnumerator LoadScene_Game()
    {
        sceneGame = SceneManager.GetSceneByName(GAME);
        if (sceneGame.handle == 0)
        {
            yield return SceneManager.LoadSceneAsync(GAME, LoadSceneMode.Additive);
            yield return GameSC.Instance.WaitForSceneLoad();
        }
        else
        {
            Debug.Log("Scene " + GAME + " being reused.");
        }
        sceneGame = SceneManager.GetSceneByName(GAME);
    }

    public IEnumerator LoadScene_Field()
    {
        sceneField = SceneManager.GetSceneByName(FIELD);
        if (sceneField.handle == 0)
        {
            yield return SceneManager.LoadSceneAsync(FIELD, LoadSceneMode.Additive);
            yield return FieldSC.Instance.WaitForSceneLoad();
        }
        else
        {
            Debug.Log("Scene " + FIELD + " being reused.");
        }
        sceneField = SceneManager.GetSceneByName(FIELD);
    }

    public IEnumerator LoadScene_Combat()
    {
        sceneCombat = SceneManager.GetSceneByName(COMBAT);
        if (sceneCombat.handle == 0)
        {
            yield return SceneManager.LoadSceneAsync(COMBAT, LoadSceneMode.Additive);
            yield return CombatSC.Instance.WaitForSceneLoad();
        }
        else
        {
            Debug.Log("Scene " + COMBAT + " being reused.");
        }
        sceneCombat = SceneManager.GetSceneByName(COMBAT);
    }

    public IEnumerator LoadScene_Town()
    {
        sceneTown = SceneManager.GetSceneByName(TOWN);
        if (sceneTown.handle == 0)
        {
            yield return SceneManager.LoadSceneAsync(TOWN, LoadSceneMode.Additive);
            yield return TownSC.Instance.WaitForSceneLoad();
        }
        else
        {
            Debug.Log("Scene " + TOWN + " being reused.");
        }
        sceneTown = SceneManager.GetSceneByName(TOWN);
    }
}

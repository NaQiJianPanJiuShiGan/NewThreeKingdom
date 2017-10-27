using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class GameStateManager : MonoBehaviour {

    public Dictionary<string, GameState> m_GameStateDic = null;

    public GameState m_CurrentGameState;
    
    public void SetState(GameState gameState)
    {
        if (gameState != null && m_CurrentGameState != null && gameState != m_CurrentGameState)
        {
            m_CurrentGameState.Stop();
        }
        else return;
        m_CurrentGameState = gameState;
        m_CurrentGameState.Start();
    }
    public void LoadScene(int sceneID)
    {
        SceneData sceneDate = new SceneData();
        sceneDate.ID = 1;
        sceneDate.Name = "";
        sceneDate.LevelName = "PlayerScene";
        sceneDate.GameState = "PlayerState";

        if (sceneDate==null||string.IsNullOrEmpty(sceneDate.GameState)||string.IsNullOrEmpty(sceneDate.LevelName))
        {
            Debug.Log("场景信息有误");
            return;
        }

        GameState gamState;
        if (!m_GameStateDic.TryGetValue(sceneDate.GameState,out gamState))
        {
            gamState = Assembly.GetExecutingAssembly().CreateInstance(sceneDate.GameState) as GameState;
            m_GameStateDic.Add(sceneDate.GameState, gamState);
        }

        SetState(gamState);
        LoadSceneManager.Instance.LoadScene(sceneDate.LevelName, gamState.LoadComplete);
    }
}

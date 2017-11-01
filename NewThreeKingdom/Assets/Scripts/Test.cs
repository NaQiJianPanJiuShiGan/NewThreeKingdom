using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    GameStateManager gameStateManager;
    // Use this for initialization
    void Start () {
        gameStateManager = GameStateManager.GetInstance();
        gameStateManager.SetState(new LoginState());
        gameStateManager.LoadScene(1);
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

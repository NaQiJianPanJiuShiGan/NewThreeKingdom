using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour {

    void Awake()
    {
        AddComponent();
    }
    // Use this for initialization
    void Start () {
	}
	void AddComponent()
    {
        gameObject.AddComponent<LoadSceneManager>();
    }
    void DoSomeSetting()
    {
        Application.targetFrameRate = 30;
        Application.runInBackground = true;
    }

	// Update is called once per frame
	void Update () {
        GUIManager.Update();
	}
}

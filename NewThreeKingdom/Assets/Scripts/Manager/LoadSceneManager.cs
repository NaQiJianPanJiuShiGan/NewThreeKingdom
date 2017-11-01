using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour {
    public static LoadSceneManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public delegate void LoadSceneHander(params object[] args);

    public void LoadScene(string sceneName,LoadSceneHander loadSceneHander,params object[] args)
    {
        StartCoroutine(AsyLoadScene(sceneName, loadSceneHander, args));
    }
    AsyncOperation Asy;
    IEnumerator AsyLoadScene(string sceneName, LoadSceneHander loadSceneHander, params object[] args)
    {
        Asy= SceneManager.LoadSceneAsync(sceneName);
        yield return Asy;
        Resources.UnloadUnusedAssets();
        GC.Collect();
        if (loadSceneHander!=null)
        {
            loadSceneHander();
        }
        Asy = null;
    }
}

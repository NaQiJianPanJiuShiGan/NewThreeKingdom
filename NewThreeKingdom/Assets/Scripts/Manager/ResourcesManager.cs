using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager{
    private static ResourcesManager _ResourcesManager;
    private ResourcesManager()
    {

    }
    public static ResourcesManager GetInstance()
    {
        if (_ResourcesManager == null)
        {
            _ResourcesManager = new ResourcesManager();
        }
        return _ResourcesManager;
    }
    private string uiPanelPath = "UI/Panel";
	public GameObject GetUIPanel(string name)
    {

        return LoadPrefab(name, uiPanelPath); ;
    }
    public GameObject LoadPrefab(string name, string path)
    {
        string loadPath = path + "/" + name;
        GameObject prefab = Resources.Load<GameObject>(loadPath);
        if (prefab == null)
        {
            Debug.LogError("检查预制体路径");
        }
        return prefab;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class GUIManager {

    //LoginPanel string:"LoginPanel" GameObject:LoginPanel IVew:LoginPanel.cs
    //代码与预制体分离
    private static Dictionary<string, KeyValuePair<GameObject, IVew>> m_UIViewDic =
        new Dictionary<string, KeyValuePair<GameObject, IVew>>();

    private static GameObject InstantiatePanel(string name)
    {
        GameObject prefab = ResourcesManager.GetInstance().GetUIPanel(name);
        if (prefab ==null)
        {
            Debug.LogError("实例化为空");
        }
        GameObject uiPrefab = GameObject.Instantiate(prefab);
        GameObject uiRoot = GameObject.FindGameObjectWithTag("UIRoot");
        if (uiRoot==null)
        {
            Debug.LogError("UIRoot为空");
            return null;
        }
        uiPrefab.transform.SetParent(uiRoot.transform);
        uiPrefab.transform.localPosition = Vector3.one;

        return uiPrefab;
    }
    /// <summary>
    /// 通过封装，实现传进一个字符串打开一个界面，并对界面做一定的管理与储存
    /// </summary>
    /// <param name="name"></param>
    public static void ShowView(string name)
    {
        IVew view;
        GameObject panel;
        KeyValuePair<GameObject, IVew> found;
        if (!m_UIViewDic.TryGetValue(name,out found))
        {
            view = Assembly.GetExecutingAssembly().CreateInstance(name) as IVew;
            panel = InstantiatePanel(name);
            m_UIViewDic.Add(name,new KeyValuePair<GameObject, IVew>(panel,view));
            view.Start();//第一次加载要对view进行初始化赋值 
        }
        else
        {
            view = found.Value;
            panel = found.Key;
        }
        if (view == null || panel == null)
        {
            Debug.LogError("View或Panel为空");
            return;
        }
        //关闭同UILayer的UI界面
        foreach (var item in m_UIViewDic)
        {
            if (view.UILayer!=item.Value.Value.UILayer)//当前ShowView的界面的Layer和字典中的view的layer是否一个层级，不是同一层级就跳过
            {
                continue;
            }
            if (!item.Value.Key.activeSelf)//遍历到被关闭的
            {
                continue;
            }
            HideView(item.Key); //同一个layer下不能有两个View同时出现
        }
        panel.SetActive(true);
        view.Show();
    }
    /// <summary>
    /// 通过一个名字实现隐藏一个界面
    /// </summary>
    /// <param name="name"></param>
    public static void HideView(string name)
    {
        KeyValuePair<GameObject, IVew> pair;
        if (!m_UIViewDic.TryGetValue(name,out pair))
        {
            Debug.LogError(name+"界面有错误，没有经过字典管理进行加载");
        }
        pair.Key.SetActive(false);
        pair.Value.Hide();
    }
    public static void Update()
    {
        foreach (var item in m_UIViewDic)
        {

            if (item.Value.Key.activeInHierarchy)
            {
                item.Value.Value.Update();
            }
        }
    }
    /// <summary>
    /// 通过GUIManger中的字典获取GameObject下的某个节点
    /// </summary>
    /// <param name="view">当前View</param>
    /// <param name="name">子节点名称</param>
    /// <returns></returns>
    public static GameObject GetChild(this IVew view ,string name)
    {
        GameObject prefab = null;
        foreach (var item in m_UIViewDic)
        {
            if (item.Value.Value==view)
            {
                prefab = item.Value.Key;
                break;
            }
        }
        if (prefab==null)
        {
            Debug.LogError("所查找的预制体为空，检查");
            return null;
        }
        Transform child = prefab.transform.FindClidNote(name);
        if (!child)
        {
            Debug.LogError("没有在预制体"+view+"下找到该节点"+name);
            return null;
        }
        return child.gameObject;
    }
    public static T GetChild<T>(this IVew view,string name)
    {
        GameObject child = GetChild(view,name);
        if (child==null)
        {
            Debug.LogError(name+"is not child of"+view);
            return default(T);
        }
        T t = child.GetComponent<T>();
        if (t==null)
        {
            Debug.LogError(name+"子物体没有这个组件");
            return default(T);
        }
        return t;
    }
}

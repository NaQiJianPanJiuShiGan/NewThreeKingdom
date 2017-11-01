using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tools {
    /// <summary>
    /// 对Transform类添加查找子节点的扩展方法
    /// </summary>
    /// <param name="go"></param>
    /// <param name="name"></param>
    /// <returns></returns>
	public static Transform FindClidNote(this Transform go ,string name)
    {
        if (go.name ==name)
        {
            return go;
        }
        for (int i = 0; i < go.childCount; i++)
        {
            Transform t = FindClidNote(go.GetChild(i), name);
            if (t!=null)
            {
                return t;
            }
        }
        return null;
    }
    /// <summary>
    /// 通过递归调用实现子物体下查找name下的T组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static T FindClidNote<T>(this Transform go, string name) where T:MonoBehaviour
    {
        Transform t = go.FindClidNote(name);
        if (t==null)
        {
            Debug.LogError(go+"下面没有子物体"+name);
            return null;
        }
        T t2 = t.GetComponent<T>();
        if (t2==null)
        {
            Debug.LogError(go+"下"+name+"没有组件");
        }
        return t2;
    }
}

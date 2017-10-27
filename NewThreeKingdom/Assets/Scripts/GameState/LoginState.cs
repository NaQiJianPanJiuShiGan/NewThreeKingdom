using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginState : GameState
{
    protected override void OnLoadComplete(params object[] args)
    {
        Debug.Log("开始加载LoginState");
    }

    protected override void OnStart()
    {
        Debug.Log("进入LoginState");
    }

    protected override void OnStop()
    {
        Debug.Log("LoginScene停止");
    }
}

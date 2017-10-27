using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : GameState {
    protected override void OnLoadComplete(params object[] args)
    {
        Debug.Log("开始加载PlayerState");
    }

    protected override void OnStart()
    {
        Debug.Log("进入PlayerState");
    }

    protected override void OnStop()
    {
        Debug.Log("PlayerState停止");
    }


}

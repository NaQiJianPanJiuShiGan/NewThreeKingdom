using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState {

    public void Start()
    {
        OnStart();
    }
    public void Stop()
    {
        OnStop();
    }
    public void LoadComplete(params object[] args)
    {
        OnLoadComplete(args);
    }

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract void OnLoadComplete(params object[] args);

}

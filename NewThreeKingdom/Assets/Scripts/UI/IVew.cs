using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum UIPanelLayers
{
    BackgroundLayer=0,
    DefaultLayer=10,
    NormalLayer=20,
    MainLayer =30,
    MaskLayer =40,
    PopupLayer =50,
    TipsLayer =60,
    LoadingLayer =80
}

public abstract class IVew {

    public UIPanelLayers UILayer = UIPanelLayers.NormalLayer;
	public void Start () {
        OnStart();
	}
	public void Destory()
    {
        OnDestory();
    }
    public void Show()
    {
        OnShow();
    }

    public void Hide()
    {
        OnHide();
    }
    
	public virtual void Update () { }
    protected abstract void OnStart();
    protected abstract void OnShow();
    protected abstract void OnHide();
    protected abstract void OnDestory();
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : IVew {
    private Text m_testLable;
    protected override void OnDestory()
    {
    }

    protected override void OnHide()
    {

    }

    protected override void OnShow()
    {

    }

    protected override void OnStart()
    {
        m_testLable = this.GetChild<Text>("Test");
        if (m_testLable!=null)
        {
            m_testLable.text = "LoginPanel";
        }
        Button loginbtn = this.GetChild<Button>("Click");
        loginbtn.onClick.AddListener(LoginClick);
    }
    void LoginClick()
    {
        m_testLable.text = "登陆成功";
    }
}

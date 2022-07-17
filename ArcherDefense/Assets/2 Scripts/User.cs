using UnityEngine;
using System;

[Serializable]
public class User
{
    [Header("玩家帳號")] public string userAccount;
    [Header("玩家密碼")] public string userPassword;
    [Header("玩家信箱")] public string userEmail;

    public User()
    {
        userAccount = StaticVar.userAccount;
        userPassword = StaticVar.userPassword;
        userEmail = StaticVar.userEmail;
    }
}

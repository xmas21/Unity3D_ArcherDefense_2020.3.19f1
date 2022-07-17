using UnityEngine;
using System;

[Serializable]
public class User
{
    [Header("���a�b��")] public string userAccount;
    [Header("���a�K�X")] public string userPassword;
    [Header("���a�H�c")] public string userEmail;

    public User()
    {
        userAccount = StaticVar.userAccount;
        userPassword = StaticVar.userPassword;
        userEmail = StaticVar.userEmail;
    }
}

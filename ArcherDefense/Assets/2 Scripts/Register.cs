using UnityEngine;
using UnityEngine.UI;
using Proyecto26;       // RestClient
using System.Collections;

public class Register : MonoBehaviour
{
    [Header("帳號輸入欄")] public InputField account_Inputfield;
    [Header("密碼輸入欄")] public InputField password_Inputfield;
    [Header("信箱輸入欄")] public InputField email_Inputfield;

    [Header("錯誤訊息")] public Text hintMessage;

    User downloadUser = new User();

    void Start()
    {
        InitializeValue();
    }

    /// <summary>
    /// 註冊帳號
    /// </summary>
    public void RegisterAccount()
    {
        hintMessage.text = "";

        // 確認帳號是否已經註冊過
        // 1. 先抓取帳號資訊
        RestClient.Get<User>("https://archerdefense-382fd-default-rtdb.firebaseio.com/" + account_Inputfield.text + ".json").Then(
            response =>
            {
                downloadUser = response;
            }
            );

        // 2. 資訊比對
        StartCoroutine(AccountCheck(1f));
    }

    /// <summary>
    /// 資訊確認
    /// </summary>
    /// <param name="_time">等待時間</param>
    /// <returns></returns>
    IEnumerator AccountCheck(float _time)
    {
        yield return new WaitForSeconds(_time);

        if (downloadUser.userAccount == account_Inputfield.text)
        {
            hintMessage.text = "此帳號已經註冊，請重新註冊";
        }
        else
        {
            // 資訊暫存
            StaticVar.userAccount = account_Inputfield.text;
            StaticVar.userPassword = password_Inputfield.text;
            StaticVar.userEmail = email_Inputfield.text;
            // 上傳資料
            PostToFirebase();

            hintMessage.text = "註冊成功";
        }
    }

    /// <summary>
    /// 提交資料到 Firebase
    /// </summary>
    void PostToFirebase()
    {
        User UploadUser = new User();
        RestClient.Put("https://archerdefense-382fd-default-rtdb.firebaseio.com/" + account_Inputfield.text + ".json", UploadUser);
    }

    /// <summary>
    /// 初始化值
    /// </summary>
    void InitializeValue()
    {
    }
}

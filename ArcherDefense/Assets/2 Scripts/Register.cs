using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Proyecto26;       // RestClient
using System.Collections;

public class Register : MonoBehaviour
{
    [SerializeField] [Header("帳號輸入欄")] InputField account_Inputfield;
    [SerializeField] [Header("密碼輸入欄")] InputField password_Inputfield;
    [SerializeField] [Header("信箱輸入欄")] InputField email_Inputfield;

    [SerializeField] [Header("錯誤訊息")] Text hintMessage;

    string downloadUserData;        // 下載的玩家資訊

    /// <summary>
    /// 註冊帳號
    /// </summary>
    [System.Obsolete]
    public void RegisterAccount()
    {
        hintMessage.text = "";

        // 資訊比對
        StartCoroutine(AccountCheck("https://archerdefense-382fd-default-rtdb.firebaseio.com/" + account_Inputfield.text + ".json"));
    }

    /// <summary>
    /// 顯示密碼
    /// </summary>
    public void ShowPassword()
    {
        if (password_Inputfield.contentType == InputField.ContentType.Standard) password_Inputfield.contentType = InputField.ContentType.Password;

        else password_Inputfield.contentType = InputField.ContentType.Standard;

        // 讓文字做更新
        password_Inputfield.ForceLabelUpdate();
    }

    /// <summary>
    /// 帳號確認
    /// </summary>
    /// <param name="_URL">玩家帳號資訊</param>
    /// <returns></returns>
    [System.Obsolete]
    IEnumerator AccountCheck(string _URL)
    {
        // UnityWebRequest 可使用 HTTP協定與伺服器連結，上傳或下載文字圖片
        using (UnityWebRequest request = UnityWebRequest.Get(_URL))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError) Debug.Log(request.error);

            // 回傳帳號資料
            else downloadUserData = request.downloadHandler.text;
        }

        if (downloadUserData != "null")
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
}

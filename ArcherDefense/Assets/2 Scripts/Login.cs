using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Login : MonoBehaviour
{
    [SerializeField] [Header("帳號輸入欄")] InputField account_Inputfield;
    [SerializeField] [Header("密碼輸入欄")] InputField password_Inputfield;

    [SerializeField] [Header("提示訊息")] Text hintMessage;

    [SerializeField] [Header("登入 頁面")] GameObject login_Page;
    [SerializeField] [Header("進入遊戲 頁面")] GameObject begin_Page;

    [SerializeField] [Header("BrowserOpener")] BrowserOpener browserOpenerScript;
    [SerializeField] [Header("隱私權政策打勾")] Toggle privacy_Toggle;

    [SerializeField] [Header("切割完的玩家資訊")] string[] downloadUserDatas;

    string downloadUserData;           // 下載的玩家資訊

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
    /// 註冊帳號
    /// </summary>
    [System.Obsolete]
    public void LoginAccount()
    {
        if (account_Inputfield.text == "" || password_Inputfield.text == "") hintMessage.text = "請輸入帳號密碼";

        else
        {
            hintMessage.text = "";

            // 資訊比對
            StartCoroutine(AccountCheck("https://archerdefense-382fd-default-rtdb.firebaseio.com/" + account_Inputfield.text + ".json"));
        }
    }

    /// <summary>
    /// 開啟網頁
    /// </summary>
    public void OpenPrivacyWeb()
    {
#if UNITY_STANDALONE_WIN
        Application.OpenURL("http://www.shinestarar.com/3857731169274022591931574.html");
#endif
#if UNITY_ANDROID
        browserOpenerScript.OnButtonClicked();
#endif
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

        // 帳號確認
        if (downloadUserData != "null")
        {
            downloadUserDatas = downloadUserData.Split('"', ',', ':');

            // 密碼確認
            if (downloadUserDatas[16] == password_Inputfield.text)
            {
                // 隱私權政策
                if (privacy_Toggle.isOn)
                {
                    login_Page.SetActive(false);
                    begin_Page.SetActive(true);
                }
                else hintMessage.text = "請勾選隱私權政策";
            }
            else hintMessage.text = "密碼輸入錯誤";
        }
        else hintMessage.text = "此帳號尚未註冊";
    }
}

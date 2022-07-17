using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Password : MonoBehaviour
{
    [SerializeField] [Header("帳號輸入欄")] InputField account_Inputfield;
    [SerializeField] [Header("密碼輸入欄")] InputField email_Inputfield;

    [SerializeField] [Header("提示訊息")] Text hintMessage;

    [SerializeField] [Header("切割完的玩家資訊")] string[] downloadUserDatas;

    string downloadUserData;           // 下載的玩家資訊

    /// <summary>
    /// 忘記密碼
    /// </summary>
    [System.Obsolete]
    public void ForgotPassword()
    {
        if (account_Inputfield.text == "" || email_Inputfield.text == "") hintMessage.text = "請輸入帳號信箱";

        else
        {
            hintMessage.text = "";

            // 資訊比對
            StartCoroutine(AccountCheck("https://archerdefense-382fd-default-rtdb.firebaseio.com/" + account_Inputfield.text + ".json"));
        }
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
            if (downloadUserDatas[4] == account_Inputfield.text)
            {
                if (downloadUserDatas[10] == email_Inputfield.text)
                {
                    // 寄信
                }
                else hintMessage.text = "信箱輸入錯誤";
            }
            else hintMessage.text = "帳號輸入錯誤";
        }
        else hintMessage.text = "沒有此帳號";
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

using System.Net;
using System.Net.Mail;

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

            // 驗證資訊欄 4 跟輸入帳號是否一樣
            if (downloadUserDatas[4] == account_Inputfield.text)
            {
                // 驗證資訊欄 10 跟輸入信箱是否一樣
                if (downloadUserDatas[10] == email_Inputfield.text)
                {
                    // 寄信
                    SendEmail();
                }
                else hintMessage.text = "信箱輸入錯誤";
            }
            else hintMessage.text = "帳號輸入錯誤";
        }
        else hintMessage.text = "沒有此帳號";
    }

    void SendEmail()
    {
        MailMessage mail = new MailMessage();

        // 如果有夾帶檔案再打開註解
        // Attachment attachment = new Attachment(@"資料路徑");

        // 寄件者資訊
        mail.From = new MailAddress("qaz431220@gmail.com", "人生無限遊戲工作室", System.Text.Encoding.UTF8);

        // 收件者資訊
        mail.To.Add(downloadUserDatas[10]);

        // 如果 mail 要 cc 給其他人在打開註解
        // mail.CC.Add("收件者 Email");

        // Mail 主旨
        mail.Subject = "弓箭手防禦 : 遊戲密碼補件";

        // Mail 內容
        mail.Body = "親愛的玩家您好，您遺失的遊戲密碼為 \n"  + downloadUserDatas[16] + "\n 避免再度遺失密碼，建議您使用備忘錄記住本次通知的密碼";

        // Mail 文字編碼格式
        mail.SubjectEncoding = System.Text.Encoding.UTF8;
        mail.BodyEncoding = System.Text.Encoding.UTF8;

        // Mail 優先程度
        mail.Priority = MailPriority.High;

        // Mail 都已 SMTP 方式寄送
        SmtpClient smtpClient = new SmtpClient();

        // 指定外送電子信箱處理方式
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

        // 每個 Mail 伺服器的 SMTP 寫法不同，如果不是使用 Gmail 需另外查，以 QQ 為例是 : Smtp.qq.com
        smtpClient.Host = "smtp.gmail.com";
        smtpClient.Port = 587;
        smtpClient.EnableSsl = true;

        // 登入你的 Gmail
        smtpClient.Credentials = new NetworkCredential("qaz431220@gmail.com", "fsafsglocgdzdwnb") as ICredentialsByHost;

        // 安全證書
        ServicePointManager.ServerCertificateValidationCallback = delegate (object sender,
            System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            System.Security.Cryptography.X509Certificates.X509Chain chain,
            System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        };

        // 寄送 Mail
        smtpClient.Send(mail);

        // 提示信件已經送出
        hintMessage.text = "信件已為您送出~";
    }
}

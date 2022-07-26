using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

using System.Net;
using System.Net.Mail;

public class Password : MonoBehaviour
{
    [SerializeField] [Header("�b����J��")] InputField account_Inputfield;
    [SerializeField] [Header("�K�X��J��")] InputField email_Inputfield;

    [SerializeField] [Header("���ܰT��")] Text hintMessage;

    [SerializeField] [Header("���Χ������a��T")] string[] downloadUserDatas;

    string downloadUserData;           // �U�������a��T

    /// <summary>
    /// �ѰO�K�X
    /// </summary>
    [System.Obsolete]
    public void ForgotPassword()
    {
        if (account_Inputfield.text == "" || email_Inputfield.text == "") hintMessage.text = "�п�J�b���H�c";

        else
        {
            hintMessage.text = "";

            // ��T���
            StartCoroutine(AccountCheck("https://archerdefense-382fd-default-rtdb.firebaseio.com/" + account_Inputfield.text + ".json"));
        }
    }

    /// <summary>
    /// �b���T�{
    /// </summary>
    /// <param name="_URL">���a�b����T</param>
    /// <returns></returns>
    [System.Obsolete]
    IEnumerator AccountCheck(string _URL)
    {
        // UnityWebRequest �i�ϥ� HTTP��w�P���A���s���A�W�ǩΤU����r�Ϥ�
        using (UnityWebRequest request = UnityWebRequest.Get(_URL))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError) Debug.Log(request.error);

            // �^�Ǳb�����
            else downloadUserData = request.downloadHandler.text;
        }

        // �b���T�{
        if (downloadUserData != "null")
        {
            downloadUserDatas = downloadUserData.Split('"', ',', ':');

            // ���Ҹ�T�� 4 ���J�b���O�_�@��
            if (downloadUserDatas[4] == account_Inputfield.text)
            {
                // ���Ҹ�T�� 10 ���J�H�c�O�_�@��
                if (downloadUserDatas[10] == email_Inputfield.text)
                {
                    // �H�H
                    SendEmail();
                }
                else hintMessage.text = "�H�c��J���~";
            }
            else hintMessage.text = "�b����J���~";
        }
        else hintMessage.text = "�S�����b��";
    }

    void SendEmail()
    {
        MailMessage mail = new MailMessage();

        // �p�G�����a�ɮצA���}����
        // Attachment attachment = new Attachment(@"��Ƹ��|");

        // �H��̸�T
        mail.From = new MailAddress("qaz431220@gmail.com", "�H�͵L���C���u�@��", System.Text.Encoding.UTF8);

        // ����̸�T
        mail.To.Add(downloadUserDatas[10]);

        // �p�G mail �n cc ����L�H�b���}����
        // mail.CC.Add("����� Email");

        // Mail �D��
        mail.Subject = "�}�b�⨾�m : �C���K�X�ɥ�";

        // Mail ���e
        mail.Body = "�˷R�����a�z�n�A�z�򥢪��C���K�X�� \n"  + downloadUserDatas[16] + "\n �קK�A�׿򥢱K�X�A��ĳ�z�ϥγƧѿ��O�����q�����K�X";

        // Mail ��r�s�X�榡
        mail.SubjectEncoding = System.Text.Encoding.UTF8;
        mail.BodyEncoding = System.Text.Encoding.UTF8;

        // Mail �u���{��
        mail.Priority = MailPriority.High;

        // Mail ���w SMTP �覡�H�e
        SmtpClient smtpClient = new SmtpClient();

        // ���w�~�e�q�l�H�c�B�z�覡
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

        // �C�� Mail ���A���� SMTP �g�k���P�A�p�G���O�ϥ� Gmail �ݥt�~�d�A�H QQ ���ҬO : Smtp.qq.com
        smtpClient.Host = "smtp.gmail.com";
        smtpClient.Port = 587;
        smtpClient.EnableSsl = true;

        // �n�J�A�� Gmail
        smtpClient.Credentials = new NetworkCredential("qaz431220@gmail.com", "fsafsglocgdzdwnb") as ICredentialsByHost;

        // �w���Ү�
        ServicePointManager.ServerCertificateValidationCallback = delegate (object sender,
            System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            System.Security.Cryptography.X509Certificates.X509Chain chain,
            System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        };

        // �H�e Mail
        smtpClient.Send(mail);

        // ���ܫH��w�g�e�X
        hintMessage.text = "�H��w���z�e�X~";
    }
}

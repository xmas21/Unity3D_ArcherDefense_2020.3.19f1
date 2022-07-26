using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Login : MonoBehaviour
{
    [SerializeField] [Header("�b����J��")] InputField account_Inputfield;
    [SerializeField] [Header("�K�X��J��")] InputField password_Inputfield;

    [SerializeField] [Header("���ܰT��")] Text hintMessage;

    [SerializeField] [Header("�n�J ����")] GameObject login_Page;
    [SerializeField] [Header("�i�J�C�� ����")] GameObject begin_Page;

    [SerializeField] [Header("BrowserOpener")] BrowserOpener browserOpenerScript;
    [SerializeField] [Header("���p�v�F������")] Toggle privacy_Toggle;

    [SerializeField] [Header("���Χ������a��T")] string[] downloadUserDatas;

    string downloadUserData;           // �U�������a��T

    /// <summary>
    /// ��ܱK�X
    /// </summary>
    public void ShowPassword()
    {
        if (password_Inputfield.contentType == InputField.ContentType.Standard) password_Inputfield.contentType = InputField.ContentType.Password;

        else password_Inputfield.contentType = InputField.ContentType.Standard;

        // ����r����s
        password_Inputfield.ForceLabelUpdate();
    }

    /// <summary>
    /// ���U�b��
    /// </summary>
    [System.Obsolete]
    public void LoginAccount()
    {
        if (account_Inputfield.text == "" || password_Inputfield.text == "") hintMessage.text = "�п�J�b���K�X";

        else
        {
            hintMessage.text = "";

            // ��T���
            StartCoroutine(AccountCheck("https://archerdefense-382fd-default-rtdb.firebaseio.com/" + account_Inputfield.text + ".json"));
        }
    }

    /// <summary>
    /// �}�Һ���
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

            // �K�X�T�{
            if (downloadUserDatas[16] == password_Inputfield.text)
            {
                // ���p�v�F��
                if (privacy_Toggle.isOn)
                {
                    login_Page.SetActive(false);
                    begin_Page.SetActive(true);
                }
                else hintMessage.text = "�ФĿ����p�v�F��";
            }
            else hintMessage.text = "�K�X��J���~";
        }
        else hintMessage.text = "���b���|�����U";
    }
}

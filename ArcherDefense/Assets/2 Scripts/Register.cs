using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Proyecto26;       // RestClient
using System.Collections;

public class Register : MonoBehaviour
{
    [SerializeField] [Header("�b����J��")] InputField account_Inputfield;
    [SerializeField] [Header("�K�X��J��")] InputField password_Inputfield;
    [SerializeField] [Header("�H�c��J��")] InputField email_Inputfield;

    [SerializeField] [Header("���~�T��")] Text hintMessage;

    string downloadUserData;        // �U�������a��T

    /// <summary>
    /// ���U�b��
    /// </summary>
    [System.Obsolete]
    public void RegisterAccount()
    {
        hintMessage.text = "";

        // ��T���
        StartCoroutine(AccountCheck("https://archerdefense-382fd-default-rtdb.firebaseio.com/" + account_Inputfield.text + ".json"));
    }

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

        if (downloadUserData != "null")
        {
            hintMessage.text = "���b���w�g���U�A�Э��s���U";
        }
        else
        {
            // ��T�Ȧs
            StaticVar.userAccount = account_Inputfield.text;
            StaticVar.userPassword = password_Inputfield.text;
            StaticVar.userEmail = email_Inputfield.text;

            // �W�Ǹ��
            PostToFirebase();

            hintMessage.text = "���U���\";
        }
    }

    /// <summary>
    /// �����ƨ� Firebase
    /// </summary>
    void PostToFirebase()
    {
        User UploadUser = new User();
        RestClient.Put("https://archerdefense-382fd-default-rtdb.firebaseio.com/" + account_Inputfield.text + ".json", UploadUser);
    }
}

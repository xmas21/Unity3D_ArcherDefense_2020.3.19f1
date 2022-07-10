using UnityEngine;
using UnityEngine.UI;
using Proyecto26;       // RestClient
using System.Collections;

public class Register : MonoBehaviour
{
    [Header("�b����J��")] public InputField account_Inputfield;
    [Header("�K�X��J��")] public InputField password_Inputfield;
    [Header("�H�c��J��")] public InputField email_Inputfield;

    [Header("���~�T��")] public Text hintMessage;

    User downloadUser = new User();

    void Start()
    {
        InitializeValue();
    }

    /// <summary>
    /// ���U�b��
    /// </summary>
    public void RegisterAccount()
    {
        hintMessage.text = "";

        // �T�{�b���O�_�w�g���U�L
        // 1. ������b����T
        RestClient.Get<User>("https://archerdefense-382fd-default-rtdb.firebaseio.com/" + account_Inputfield.text + ".json").Then(
            response =>
            {
                downloadUser = response;
            }
            );

        // 2. ��T���
        StartCoroutine(AccountCheck(1f));
    }

    /// <summary>
    /// ��T�T�{
    /// </summary>
    /// <param name="_time">���ݮɶ�</param>
    /// <returns></returns>
    IEnumerator AccountCheck(float _time)
    {
        yield return new WaitForSeconds(_time);

        if (downloadUser.userAccount == account_Inputfield.text)
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

    /// <summary>
    /// ��l�ƭ�
    /// </summary>
    void InitializeValue()
    {
    }
}

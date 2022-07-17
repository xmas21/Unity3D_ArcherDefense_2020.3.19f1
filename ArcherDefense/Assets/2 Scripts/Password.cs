using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

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

            // �K�X�T�{
            if (downloadUserDatas[4] == account_Inputfield.text)
            {
                if (downloadUserDatas[10] == email_Inputfield.text)
                {
                    // �H�H
                }
                else hintMessage.text = "�H�c��J���~";
            }
            else hintMessage.text = "�b����J���~";
        }
        else hintMessage.text = "�S�����b��";
    }
}

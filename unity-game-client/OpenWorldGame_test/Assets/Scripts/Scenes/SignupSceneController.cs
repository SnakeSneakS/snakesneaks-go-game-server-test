using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignupSceneController : MonoBehaviour
{
    [Header("Form")]
    [SerializeField] InputField UsernameInputField;
    [SerializeField] InputField EmailInputField;
    [SerializeField] InputField PasswordInputField;
    [SerializeField] Button Signup_RequestButton;
    [SerializeField] Button Login_SceneLoadButton;

    [Header("Result")]
    [SerializeField] GameObject ResultPanel;
    [SerializeField] Text ResultText;

    [Header("WebClient")]
    [SerializeField] private SignupWebClient signupWebClient;

    private bool isSignupChallenge = false;

    private void Awake()
    {
        SetUpButtonEvent();
        ClientManager.LoadSession();
    }

    /// <summary>
    /// Set up button event: Onclick
    /// </summary>
    private void SetUpButtonEvent()
    {
        //Signup
        Signup_RequestButton.onClick.AddListener(() => {
            if (!isSignupChallenge)
            {
                StartCoroutine(SignupRequest());
            }
        });
        //Login
        Login_SceneLoadButton.onClick.AddListener(() =>
        {
            if (!isSignupChallenge)
            {
                SceneManager.LoadScene(SceneManager.SceneName.Login);
            }
        });
    }


    /// <summary>
    /// Signup Request
    /// </summary>
    /// <returns></returns>
    IEnumerator SignupRequest()
    {
        isSignupChallenge = true;

        string username = UsernameInputField.text;
        string email = EmailInputField.text;
        string password = PasswordInputField.text;
        ClientManager.UpdateUsername(username);
        signupWebClient.SetData(username, email, password);
        yield return StartCoroutine(signupWebClient.Send());

        //処理
        if (signupWebClient.result == WebClient.ResultType.Success)
        {
            //成功した時
            //LoginWebClientはひとまずLoginResponseDataをdataに保存するとする 
            SignupWebClient.SignupResponseData srd = (SignupWebClient.SignupResponseData)signupWebClient.data;
            Debug.Log($"ParsedResponseData: \nStatus: {srd.status}, user_id: {srd.session.UserID}, session_id: {srd.session.SessionID}");
            if (srd.status == Model.ConnStatus.success)
            {
                SignupSucces(srd.session);
            }
            else
            {
                SignupFailed();
            }
        }
        else
        {
            switch (signupWebClient.result)
            {
                case LoginWebClient.ResultType.RequestDataError:
                    RequestDataError(signupWebClient);
                    break;
                case LoginWebClient.ResultType.ResponseDataError:
                case LoginWebClient.ResultType.ConnectionError:
                case LoginWebClient.ResultType.ConnInProgress:
                    ConnError();
                    break;
            }

        }
        isSignupChallenge = false;
    }


    public void SignupSucces(Model.Session session)
    {
        ResultText.text = "登録に成功しました。";
        StartCoroutine(ShowAlertForWhile(ResultPanel, 2.0f));
        ClientManager.UpdateSession(session);
        StartCoroutine(LoadScene());
    }

    public void SignupFailed()
    {
        ResultText.text = "登録に失敗しました。";
        StartCoroutine(ShowAlertForWhile(ResultPanel, 2.0f));
    }

    


    public void ConnError()
    {
        ResultText.text = "通信に失敗しました。";
        StartCoroutine(ShowAlertForWhile(ResultPanel, 2.0f));
    }

    public void RequestDataError(WebClient wc)
    {
        ResultText.text = wc.message;
        StartCoroutine(ShowAlertForWhile(ResultPanel, 2.0f));
    }

    public IEnumerator ShowAlertForWhile(GameObject gm, float duration)
    {
        gm.SetActive(true);
        yield return new WaitForSeconds(duration);
        gm.SetActive(false);
        yield break;
    }

    public IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(SceneManager.SceneName.LoadGame);
    }
}

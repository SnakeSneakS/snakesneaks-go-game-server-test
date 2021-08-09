using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LoginWebClient))]
[RequireComponent(typeof(SignupWebClient))]
public class LoginSceneController : MonoBehaviour
{
    [Header("Form")]
    [SerializeField] InputField EmailInputField;
    [SerializeField] InputField PasswordInputField;
    [SerializeField] Button     Login_RequestButton;
    [SerializeField] Button     Signup_SceneLoadButton;

    [Header("Result")]
    [SerializeField] GameObject ResultPanel;
    [SerializeField] Text       ResultText;

    [Header("WebClient")]
    [SerializeField] private LoginWebClient loginWebClient;
    [SerializeField] private SignupWebClient signupWebClient; 

    private bool isLoginChallenge = false;

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
        //Login
        Login_RequestButton.onClick.AddListener(() => {
            if (!isLoginChallenge)
            {
                StartCoroutine(LoginRequest());
            }
        });
        //Signup
        Signup_SceneLoadButton.onClick.AddListener(() =>
        {
            if (!isLoginChallenge)
            {
                StartCoroutine(SignupRequest());
            }
        });
    }


    /// <summary>
    /// Login Request
    /// </summary>
    /// <returns></returns>
    IEnumerator LoginRequest()
    {
        isLoginChallenge = true;

        string email = EmailInputField.text;
        string password = PasswordInputField.text;
        loginWebClient.SetData(email, password);
        yield return StartCoroutine(loginWebClient.Send());

        //処理
        if (loginWebClient.result == WebClient.ResultType.Success)
        {
            //成功した時
            //LoginWebClientはひとまずLoginResponseDataをdataに保存するとする 
            LoginWebClient.LoginResponseData lrd = (LoginWebClient.LoginResponseData)loginWebClient.data;
            Debug.Log($"ParsedResponseData: \nStatus: {lrd.status}, user_id: {lrd.session.UserID}, session_id: {lrd.session.SessionID}");
            if (lrd.status == Model.ConnStatus.success)
            {
                LoginSucces(lrd.session);
            }
            else
            {
                LoginFailed();
            }
        }
        else
        {
            switch (loginWebClient.result)
            {
                case LoginWebClient.ResultType.RequestDataError:
                    RequestDataError(loginWebClient);
                    break;
                case LoginWebClient.ResultType.ResponseDataError:
                case LoginWebClient.ResultType.ConnectionError:
                case LoginWebClient.ResultType.ConnInProgress:
                    ConnError();
                    break;
            }
            
        }
        isLoginChallenge = false;
    }


    public void LoginSucces(Model.Session session)
    {
        ResultText.text = "ログイン成功!";
        StartCoroutine(ShowAlertForWhile(ResultPanel, 2.0f));
        ClientManager.UpdateSession(session);
        StartCoroutine(LoadScene());
    }

    public void LoginFailed()
    {
        ResultText.text = "ログイン失敗!";
        StartCoroutine(ShowAlertForWhile(ResultPanel,2.0f));
    }

    /// <summary>
    /// Signup Request
    /// </summary>
    /// <returns></returns>
    IEnumerator SignupRequest()
    {
        isLoginChallenge = true;

        string email = EmailInputField.text;
        string password = PasswordInputField.text;
        signupWebClient.SetData(email, password);
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
                LoginSucces(srd.session);
            }
            else
            {
                LoginFailed();
            }
        }
        else
        {
            switch (signupWebClient.result)
            {
                case LoginWebClient.ResultType.RequestDataError:
                    RequestDataError(loginWebClient);
                    break;
                case LoginWebClient.ResultType.ResponseDataError:
                case LoginWebClient.ResultType.ConnectionError:
                case LoginWebClient.ResultType.ConnInProgress:
                    ConnError();
                    break;
            }

        }
        isLoginChallenge = false;
    }


    public void ConnError()
    {
        ResultText.text = "通信失敗!";
        StartCoroutine(ShowAlertForWhile(ResultPanel, 2.0f));
    }

    public void RequestDataError(LoginWebClient lwc)
    {
        ResultText.text = lwc.message;
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

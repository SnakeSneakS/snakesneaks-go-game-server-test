using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private LoginWebClient loginWebClient;

    private bool isLoginChallenge = false;

    private void Awake()
    {
        SetUpButtonEvent();
        ClientManager.LoadSession();

        if (EnvManager.Read("USE_TLS") == "True")
        {
            this.loginWebClient = new LoginWebClient(WebClient.ProtocolType.https, WebClient.HttpRequestMethod.Post, EnvManager.Read("HOST_NAME"), EnvManager.Read("GO_GAME_SERVER_PORT_TLS"), "/api/auth/login", EnvManager.Read("ALLOW_ALL_CERT") == "True");
        }
        else
        {
            this.loginWebClient = new LoginWebClient(WebClient.ProtocolType.http, WebClient.HttpRequestMethod.Post, EnvManager.Read("HOST_NAME"), EnvManager.Read("GO_GAME_SERVER_PORT"), "/api/auth/login", EnvManager.Read("ALLOW_ALL_CERT") == "True");
        }
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
                SceneManager.LoadScene(SceneManager.SceneName.Signup);
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
        ResultText.text = "ログインに成功しました。";
        StartCoroutine(ShowAlertForWhile(ResultPanel, 2.0f));
        ClientManager.UpdateSession(session);
        StartCoroutine(LoadScene());
    }

    public void LoginFailed()
    {
        ResultText.text = "ログインに失敗しました。";
        StartCoroutine(ShowAlertForWhile(ResultPanel,2.0f));
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

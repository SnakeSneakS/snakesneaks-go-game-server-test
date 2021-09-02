using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleSceneManager : MonoBehaviour
{
    [Header("Start Button")]
    [SerializeField] Button Game_StartButton;

    [Header("Logout Button")]
    [SerializeField] Button Logout_RequestButton;

    [Header("Result")]
    [SerializeField] private GameObject session_resultPanel;
    [SerializeField] private Text       session_resultText;

    [Header("WebClient")]
    private SessionWebClient sessionWebClient;
    private LogoutWebClient logoutWebClient;
   

    private bool isGameStart = false;
    private bool isLogoutRequest = false;

    private void Awake()
    {
        SetUpButtonEvent();
        ClientManager.LoadLocalData();

        if (EnvManager.Read("USE_TLS") == "True")
        {
            this.sessionWebClient = new SessionWebClient(WebClient.ProtocolType.https, WebClient.HttpRequestMethod.Post, EnvManager.Read("HOST_NAME"), EnvManager.Read("GO_GAME_SERVER_PORT_TLS"), "/api/auth/session", EnvManager.Read("ALLOW_ALL_CERT") == "True");
            this.logoutWebClient = new LogoutWebClient(WebClient.ProtocolType.https, WebClient.HttpRequestMethod.Post, EnvManager.Read("HOST_NAME"), EnvManager.Read("GO_GAME_SERVER_PORT_TLS"), "/api/auth/logout", EnvManager.Read("ALLOW_ALL_CERT") == "True");
        }
        else
        {
            this.sessionWebClient = new SessionWebClient(WebClient.ProtocolType.http, WebClient.HttpRequestMethod.Post, EnvManager.Read("HOST_NAME"), EnvManager.Read("GO_GAME_SERVER_PORT"), "/api/auth/session", EnvManager.Read("ALLOW_ALL_CERT") == "True");
            this.logoutWebClient = new LogoutWebClient(WebClient.ProtocolType.http, WebClient.HttpRequestMethod.Post, EnvManager.Read("HOST_NAME"), EnvManager.Read("GO_GAME_SERVER_PORT"), "/api/auth/logout", EnvManager.Read("ALLOW_ALL_CERT") == "True");
        }
        
    }

    private void Start()
    {
        if (!string.IsNullOrEmpty(ClientManager.Session.SessionID))
        {
            this.Logout_RequestButton.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Set up button event: Onclick
    /// </summary>
    private void SetUpButtonEvent()
    {
        //GameStart
        Game_StartButton.onClick.AddListener(() => {
            if (!isGameStart)
            {
                isGameStart = true;
                StartCoroutine( GameStart() );
            }
        });
        //Logout For Debug (This will be replaced to "User Info"
        Logout_RequestButton.onClick.AddListener(() => {
            StartCoroutine(LogoutRequest());
        });
    }

    public IEnumerator GameStart()
    {
        yield return StartCoroutine( SessionCheckRequest() );
        yield break;
    }

    /// <summary>
    /// Session Check Request
    /// </summary>
    /// <returns></returns>
    public IEnumerator SessionCheckRequest()
    {
        Model.Session session = ClientManager.Session;
        
        this.sessionWebClient.SetData(session);
        yield return StartCoroutine(sessionWebClient.Send());

        //処理
        if (sessionWebClient.result == WebClient.ResultType.Success)
        {
            //成功した時
            //LoginWebClientはひとまずLoginResponseDataをdataに保存するとする 
            SessionWebClient.SessionResponseData srd = (SessionWebClient.SessionResponseData)sessionWebClient.data;
            Debug.Log($"ParsedResponseData: \nStatus: {srd.status}");
            if (srd.status == Model.ConnStatus.success)
            {
                SessionSuccess();
            }
            else
            {
                SessionFailed();
            }
        }
        else
        {
            switch (sessionWebClient.result)
            {
                case LoginWebClient.ResultType.RequestDataError:
                    RequestDataError();
                    break;
                case LoginWebClient.ResultType.ResponseDataError:
                case LoginWebClient.ResultType.ConnectionError:
                case LoginWebClient.ResultType.ConnInProgress:
                    ConnError();
                    break;
            }
            isGameStart = false;

        }

    }

    public void SessionSuccess()
    {
        Debug.Log("Session Success!");
        SceneManager.LoadScene(SceneManager.SceneName.LoadGame);
    }

    public void SessionFailed()
    {
        Debug.Log("Session Failed!");
        SceneManager.LoadScene(SceneManager.SceneName.Signup);
    }


    public IEnumerator LogoutRequest()
    {
        if (isLogoutRequest)
        {
            Debug.Log("Stop to request because already requested!");
            yield break;
        }
        isLogoutRequest = true;

        Model.Session session = ClientManager.Session;

        this.logoutWebClient.SetData(session.UserID,session.SessionID);
        yield return StartCoroutine(logoutWebClient.Send());

        //処理
        if (logoutWebClient.result == WebClient.ResultType.Success)
        {
            //成功した時
            //LoginWebClientはひとまずLoginResponseDataをdataに保存するとする 
            LogoutWebClient.LogoutResponseData srd = (LogoutWebClient.LogoutResponseData)logoutWebClient.data;
            Debug.Log($"ParsedResponseData: \nStatus: {srd.status}");
            if (srd.status == Model.ConnStatus.success)
            {
                LogoutSuccess();
            }
            else
            {
                LogoutFailed();
            }
        }
        else
        {
            switch (logoutWebClient.result)
            {
                case LoginWebClient.ResultType.RequestDataError:
                    RequestDataError();
                    break;
                case LoginWebClient.ResultType.ResponseDataError:
                case LoginWebClient.ResultType.ConnectionError:
                case LoginWebClient.ResultType.ConnInProgress:
                    ConnError();
                    break;
            }
        }

        isLogoutRequest = false;
        yield break;
    }

    public void LogoutSuccess()
    {
        Debug.Log("Logout Success!");
        ClientManager.DeleteLocalData();
        SceneManager.LoadScene(SceneManager.SceneName.Title);
    }

    public void LogoutFailed()
    {
        Debug.Log("Logout Failed!");
    }


    public void ConnError()
    {
        Debug.Log("Connection Error!");
        session_resultText.text = "通信失敗!";
        StartCoroutine(ShowAlertForWhile(session_resultPanel, 2.0f));
    }

    public void RequestDataError()
    {
        Debug.Log("RequestData Error!");
        session_resultText.text = "データ失敗!";
        SceneManager.LoadScene(SceneManager.SceneName.Signup);
    }

    public IEnumerator ShowAlertForWhile(GameObject gm, float duration)
    {
        gm.SetActive(true);
        yield return new WaitForSeconds(duration);
        gm.SetActive(false);
        yield break;
    }

}

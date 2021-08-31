using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SessionWebClient))]
[RequireComponent(typeof(LogoutWebClient))]
public class TitleSceneManager : MonoBehaviour
{
    [Header("Start Button")]
    [SerializeField] Button Game_StartButton;

    [Header("Logout Button")]
    [SerializeField] Button Logout_RequestButton;

    [Header("WebClient")]
    [SerializeField] private SessionWebClient sessionWebClient;
    [SerializeField] private LogoutWebClient logoutWebClient;

    [Header("Result")]
    [SerializeField] private GameObject session_resultPanel;
    [SerializeField] private Text       session_resultText;

    private bool isGameStart = false;
    private bool isLogoutRequest = false;

    private void Awake()
    {
        SetUpButtonEvent();
        ClientManager.LoadLocalData();
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
        SceneManager.LoadScene(SceneManager.SceneName.Login);
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
        SceneManager.LoadScene(SceneManager.SceneName.Login);
    }

    public IEnumerator ShowAlertForWhile(GameObject gm, float duration)
    {
        gm.SetActive(true);
        yield return new WaitForSeconds(duration);
        gm.SetActive(false);
        yield break;
    }

}

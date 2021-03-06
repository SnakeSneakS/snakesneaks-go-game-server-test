using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SignupWebClient))]
[RequireComponent(typeof(LoginWebClient))]
[RequireComponent(typeof(LogoutWebClient))]
public class HttpPostTestScene : MonoBehaviour
{
    [Header("Show Host Uri")]
    [SerializeField] ButtonTextPair DebugHostUriButtonTextPair;

    [Header("Signup Form")]
    [SerializeField] InputField Signup_UsernameInputField;
    [SerializeField] InputField Signup_EmailInputField;
    [SerializeField] InputField Signup_PasswordInputField;
    [SerializeField] ButtonTextPair Signup_RequestButtonTextPair;

    [Header("Login Form")]
    [SerializeField] InputField Login_UsernameInputField;
    [SerializeField] InputField Login_EmailInputField;
    [SerializeField] InputField Login_PasswordInputField;
    [SerializeField] ButtonTextPair Login_RequestButtonTextPair;

    [Header("Logout Form")]
    [SerializeField] InputField Logout_UserIdInputField;
    [SerializeField] InputField Logout_SessionIdInputField;
    [SerializeField] ButtonTextPair Logout_RequestButtonTextPair;


    [Header("WebClient")]
    [SerializeField] private SignupWebClient signupWebClient;
    [SerializeField] private LoginWebClient loginWebClient;
    [SerializeField] private LogoutWebClient logoutWebClient;


    Model.User m_user;
    Model.Session m_session;


    [System.Serializable]
    private struct ButtonTextPair
    {
        public Button  button;
        public Text    result;
    }


    private void Awake()
    {
        m_user = new Model.User();
        m_session = new Model.Session();
        SetUpButtonEvent();
    }

    private void Start()
    {
    }

    /// <summary>
    /// Set up button event: Onclick
    /// </summary>
    private void SetUpButtonEvent()
    {
        //Signup
        Signup_RequestButtonTextPair.button.onClick.AddListener(() => {
            StartCoroutine(SignupRequest());
        });
        //Login
        Login_RequestButtonTextPair.button.onClick.AddListener(() => {
            StartCoroutine(LoginRequest());
        });
        //Logout
        Logout_RequestButtonTextPair.button.onClick.AddListener(() =>
        {
            StartCoroutine(LogoutRequest());
        });
    }

    /// <summary>
    /// Signup Request
    /// </summary>
    /// <returns></returns>
    IEnumerator SignupRequest()
    {
        string username = Signup_UsernameInputField.text;
        string email = Signup_EmailInputField.text;
        string password = Signup_PasswordInputField.text;
        signupWebClient.SetData(username, email, password);
        yield return StartCoroutine(signupWebClient.Send());

        //??????
        if (loginWebClient.result == WebClient.ResultType.Success)
        {
            //???????????????
            //LoginWebClient???????????????LoginResponseData???data???????????????????????? 
            SignupWebClient.SignupResponseData lrd = (SignupWebClient.SignupResponseData)signupWebClient.data;
            Debug.Log($"ParsedResponseData: \nStatus: {lrd.status}, user_id: {lrd.session.UserID}, session_id: {lrd.session.SessionID}");
            Debug.Log(signupWebClient.message);
        }
        else
        {
            //???????????????
            Debug.Log(signupWebClient.message);
        }

        Signup_RequestButtonTextPair.result.text = signupWebClient.message;
    }


    /// <summary>
    /// Login Request
    /// </summary>
    /// <returns></returns>
    IEnumerator LoginRequest()
    {
        string email = Login_EmailInputField.text;
        string password = Login_PasswordInputField.text;
        loginWebClient.SetData(email, password);
        yield return StartCoroutine(loginWebClient.Send());

        //??????
        if (loginWebClient.result==WebClient.ResultType.Success)
        {
            //???????????????
            //LoginWebClient???????????????LoginResponseData???data???????????????????????? 
            LoginWebClient.LoginResponseData lrd = (LoginWebClient.LoginResponseData)loginWebClient.data;
            Debug.Log($"ParsedResponseData: \nStatus: {lrd.status}, user_id: {lrd.session.UserID}, session_id: {lrd.session.SessionID}");
            Debug.Log(loginWebClient.message);
        }
        else
        {
            //???????????????
            Debug.Log(loginWebClient.message);
        }

        Login_RequestButtonTextPair.result.text = loginWebClient.message;
    }

    
    /// <summary>
    /// LogoutRequest 
    /// </summary>
    /// <returns></returns>
    IEnumerator LogoutRequest()
    {
        string session_id = Logout_SessionIdInputField.text;
        uint user_id;
        try{ user_id = System.Convert.ToUInt32(Logout_UserIdInputField.text);
        }catch{ user_id = 0;
        }
        logoutWebClient.SetData(user_id,session_id);
        yield return StartCoroutine(logoutWebClient.Send());

        //??????
        if (logoutWebClient.result == WebClient.ResultType.Success)
        {
            //???????????????
            //LoginWebClient???????????????LoginResponseData???data???????????????????????? 
            LogoutWebClient.LogoutResponseData lrd = (LogoutWebClient.LogoutResponseData)logoutWebClient.data;
            Debug.Log($"ParsedResponseData: \nStatus: {lrd.status}");
            Debug.Log(logoutWebClient.message);
        }
        else
        {
            //???????????????
            Debug.Log(logoutWebClient.message);
        }

        Logout_RequestButtonTextPair.result.text = logoutWebClient.message;
    }

}

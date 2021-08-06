using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        //DebugHostUri
        DebugHostUriButtonTextPair.button.onClick.AddListener(() => {
            DebugHostUriButtonTextPair.result.text = $"{Model.hostUri}";
        });
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
        HttpAuthClient httpAuthClient = new HttpAuthClient(Model.signupUri);

        m_user = new Model.User(Signup_UsernameInputField.text, Signup_EmailInputField.text, Signup_PasswordInputField.text);
        Debug.Log($"~SendData~ \n username: {m_user.Username}, email: {m_user.Email}, password: {m_user.Password}");
        Debug.Log($"~SendDataError~ \n Error?: {m_user.Error}");
        if (m_user.Error)
        {
            Signup_RequestButtonTextPair.result.text = "Error Found in Request Data.";
            yield break;
        }

        Signup_RequestButtonTextPair.result.text = "Send data...";
        yield return StartCoroutine(httpAuthClient.Signup(m_user));

        if (httpAuthClient.www.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
        {
            Signup_RequestButtonTextPair.result.text = $"Connection Failed: \n[{httpAuthClient.www.responseCode}] {httpAuthClient.www.error}";
            yield break;
        }

        Signup_RequestButtonTextPair.result.text = httpAuthClient.www.downloadHandler.text;
        Model.SessionRes sessionRes = JsonUtility.FromJson<Model.SessionRes>(httpAuthClient.www.downloadHandler.text);
        if (sessionRes.status != Model.Status.failed.ToString())
        {
            m_session = new Model.Session(sessionRes.session.UserID, sessionRes.session.SessionID);
            Signup_RequestButtonTextPair.result.text += "\n<color=blue>Signup Success!</color>";
        }
        else
        {
            m_session = new Model.Session();
            Signup_RequestButtonTextPair.result.text += "\n<color=red>Signup Failed!</color>";
        }

        yield break;
    }

    /// <summary>
    /// Login Request
    /// </summary>
    /// <returns></returns>
    IEnumerator LoginRequest()
    {
        HttpAuthClient httpAuthClient = new HttpAuthClient(Model.loginUri);

        m_user = new Model.User(Login_UsernameInputField.text, Login_EmailInputField.text, Login_PasswordInputField.text);
        Debug.Log($"~SendData~ \n username: {m_user.Username}, email: {m_user.Email}, password: {m_user.Password}");
        Debug.Log($"~SendDataError~ \n Error?: {m_user.Error}");
        if (m_user.Error)
        {
            Login_RequestButtonTextPair.result.text = "Error Found in Request Data.";
            yield break;
        }

        Login_RequestButtonTextPair.result.text = "Send data...";
        StartCoroutine(httpAuthClient.Login(m_user));

        while (!httpAuthClient.www.isDone)
        {
            continue;
        }

        if (httpAuthClient.www.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
        {
            Login_RequestButtonTextPair.result.text = $"Connection Failed: \n[{httpAuthClient.www.responseCode}] {httpAuthClient.www.error}";
            yield break;
        }

        Login_RequestButtonTextPair.result.text= httpAuthClient.www.downloadHandler.text;
        Model.SessionRes sessionRes = JsonUtility.FromJson<Model.SessionRes>(httpAuthClient.www.downloadHandler.text);
        if (sessionRes.status != Model.Status.failed.ToString())
        {
            m_session = new Model.Session(sessionRes.session.UserID, sessionRes.session.SessionID);
            Login_RequestButtonTextPair.result.text += "\n<color=blue>Login Success!</color>";
        }
        else
        {
            m_session = new Model.Session();
            Login_RequestButtonTextPair.result.text += "\n<color=red>Login Failed!</color>";
        }

        yield break;
    }

    /// <summary>
    /// LogoutRequest 
    /// </summary>
    /// <returns></returns>
    IEnumerator LogoutRequest()
    {
        HttpAuthClient httpAuthClient = new HttpAuthClient(Model.logoutUri);

        m_session = new Model.Session(Logout_UserIdInputField.text,Logout_SessionIdInputField.text);
        Debug.Log($"~SendData~ \n user_id: {m_session.UserID}, session_id: {m_session.SessionID}");
        Debug.Log($"~SendDataError~ \n Error?: {m_session.Error}");
        if (m_user.Error)
        {
            Logout_RequestButtonTextPair.result.text = "Error Found in Request Data.";
            yield break;
        }

        Logout_RequestButtonTextPair.result.text = "Send data...";
        StartCoroutine(httpAuthClient.Logout(m_session));

        while (!httpAuthClient.www.isDone)
        {
            continue;
        }

        if (httpAuthClient.www.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
        {
            Logout_RequestButtonTextPair.result.text = $"Connection Failed: \n[{httpAuthClient.www.responseCode}] {httpAuthClient.www.error}";
            yield break;
        }

        Logout_RequestButtonTextPair.result.text = httpAuthClient.www.downloadHandler.text;
        Model.StatusRes statusRes = JsonUtility.FromJson<Model.StatusRes>(httpAuthClient.www.downloadHandler.text);
        if (statusRes.status != Model.Status.failed.ToString())
        {
            Logout_RequestButtonTextPair.result.text += "\n<color=blue>Logout Success!</color>";
        }
        else
        {
            Logout_RequestButtonTextPair.result.text += "\n<color=red>Logout Failed!</color>";
        }

        yield break;
    }



}

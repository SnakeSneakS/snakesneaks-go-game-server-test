using System;
using System.IO;

//Model.Connection 
public partial class Model 
{
    const string hostname= "localhost";
    const string port = "8080";
    public static readonly Uri hostUri = new Uri($"{hostname}:{port}");

    const string signupPath="api/auth/signup";
    public static readonly Uri signupUri = new Uri( $"{hostname}:{port}/{signupPath}");

    const string loginPath = "api/auth/login";
    public static readonly Uri loginUri = new Uri($"{hostname}:{port}/{loginPath}");

    const string logoutPath = "api/auth/logout";
    public static readonly Uri logoutUri = new Uri($"{hostname}:{port}/{logoutPath}");

    const string gamePath = "api/game";
    public static readonly Uri gameUri = new Uri($"{hostname}:{port}/{gamePath}");

}

using System;
using System.IO;

//Model.Connection 
public partial class Model 
{
    const string hostname= "localhost";
    const string port = "8080";
    public static Uri hostUri = new Uri($"{hostname}:{port}");

    const string signupPath="api/auth/signup";
    public static Uri signupUri = new Uri( $"{hostname}:{port}/{signupPath}");

    const string loginPath = "api/auth/login";
    public static Uri loginUri = new Uri($"{hostname}:{port}/{loginPath}");

    const string logoutPath = "api/auth/logout";
    public static Uri logoutUri = new Uri($"{hostname}:{port}/{logoutPath}");

}

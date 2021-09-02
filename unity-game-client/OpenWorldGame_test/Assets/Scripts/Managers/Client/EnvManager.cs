using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvManager : MonoBehaviour
{
    private const string env_file = "Env/env"; //in resorces folder 

    private static bool m_loaded = false;
    public static Dictionary<string, string> m_dict=new Dictionary<string, string>();

    private static void Load()
    {
        string text="";
        try {
            text = (Resources.Load(env_file, typeof(TextAsset)) as TextAsset).text;
        }
        catch
        {
            Debug.LogError("Make sure tha your \"env.txt\" is located in /Resources/ folder.");
        }
        
        Debug.Log("Loaded Env/env.txt");
        foreach(string line in text.Split('\n'))
        {
            if (string.IsNullOrEmpty(line)) continue; //empty line
            if (line[0] == '#') continue; //comment out
            int index = line.IndexOf('=');
            if (index <= 0) continue; //no key

            string key = line.Substring(0, index).Trim();
            string value = line.Substring(index + 1).Trim();
            if ( (value.StartsWith("\"") && value.EndsWith("\"")) || (value.StartsWith("\'") && value.EndsWith("\'")) )
            {
                value.Trim('\'').Trim('\"');
            }

            if (string.IsNullOrEmpty(key)) continue;
            m_dict[key] = value;
        }
        foreach(KeyValuePair<string,string> d in m_dict)
        {
            Debug.Log($"{d.Key}: {d.Value}");
        }

        m_loaded = true;
    }

    public static string Read(string key)
    {
        if (!m_loaded)
        {
            Load();
        }
        if (m_dict.ContainsKey(key))
        {
            return m_dict[key];
        }
        else
        {
            Debug.LogAssertion("No value was found! Please check your \"Resources/Env/env.txt\" file again.");
            return "";
        }
    }
}

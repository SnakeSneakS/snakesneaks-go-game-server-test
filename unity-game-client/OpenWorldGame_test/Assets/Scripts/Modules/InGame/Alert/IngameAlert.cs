using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameAlert : MonoBehaviour
{
#if false
    public const float AlertShowSeconds=0.0f;

    public static IEnumerator ShowOnceAlert(GameObject ui_obj, Text ui_text, string text)
    {
        ui_obj.SetActive(true);
        yield return new WaitForSeconds(AlertShowSeconds);
        ui_obj.SetActive(false);
        yield break;
    }

    public IEnumerator YesConfigureAlert(GameObject ui_obj, Text ui_text, string text_text, Button ui_yesButton, string button_text, Action yes_act)
    {
        ui_obj.SetActive(true);
        ui_yesButton.onClick.AddListener(() =>
        {
            yes_act();
            ui_obj.SetActive(false);
        });
        yield break;
    }
#endif

}

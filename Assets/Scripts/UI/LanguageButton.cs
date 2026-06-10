using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageButton : MonoBehaviour
{
    [SerializeField] private string language = "ENG";
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => LocalizationManager.Instance.SetLanguage(language));
    }

    
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HideButton : MonoBehaviour
{
    public GameObject ui;

    void Start()
    {
        var btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(OnMouseClick);
    }
    
    private void OnMouseClick()
    {
        var setting = GameObject.Find("Settings Menu");
        setting.SetActive(false);
        ui.SetActive(true);
    }
}

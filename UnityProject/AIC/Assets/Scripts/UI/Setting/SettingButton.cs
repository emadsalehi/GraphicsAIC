using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingButton : MonoBehaviour
{
    public GameObject setting;
    // Use this for initialization
    void Start()
    {
        
        var btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(OnMouseClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseClick()
    {
        var ui = GameObject.Find("In-Game UI");
        //ui.SetActive(false);
        setting.SetActive(true);
    }
}

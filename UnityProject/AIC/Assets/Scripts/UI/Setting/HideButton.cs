using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HideButton : MonoBehaviour
{
    public GameObject ui;
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
        var setting = GameObject.Find("Settings Menu");
        setting.SetActive(false);
        ui.SetActive(true);
    }
}

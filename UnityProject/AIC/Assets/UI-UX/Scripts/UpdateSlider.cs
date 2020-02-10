using UnityEngine;
using UnityEngine.UI;

public class UpdateSlider : MonoBehaviour
{
    public void UpdateSliderValue(float value)    
    {
        var scale = gameObject.GetComponent<Image>().rectTransform.localScale;
        scale.x = value;
        gameObject.GetComponent<Image>().rectTransform.localScale = scale;
    }
}

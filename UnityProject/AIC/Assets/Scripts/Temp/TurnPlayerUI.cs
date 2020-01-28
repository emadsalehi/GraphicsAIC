using UnityEngine;
using UnityEngine.UI;

public class TurnPlayerUI : MonoBehaviour
{
    public void UpdateTurnNumber(int turnNumber)
    {
        GetComponent<Text>().text = turnNumber.ToString();
    }
}

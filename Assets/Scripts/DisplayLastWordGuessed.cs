using TMPro;
using UnityEngine;

public class DisplayLastWordGuessed : MonoBehaviour
{
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text =
            PlayerPrefs.GetString("lastWordGuessed");
    }
}

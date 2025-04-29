using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class WordButton : MonoBehaviour
{
    public Button button;

    private void Awake()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }
    }

    public void SetListener(UnityAction callback)
    {
        if (button != null && callback != null)
        {
            button.onClick.AddListener(callback);
        }
        else
        {
            Debug.LogError("Button component is null in WordButton prefab!");
        }
    }
}
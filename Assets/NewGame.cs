using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGame : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        
        _button.onClick.AddListener(CreateNewGame);
    }

    private static void CreateNewGame()
    {
        SceneManager.LoadScene("Kamil");
    }
}
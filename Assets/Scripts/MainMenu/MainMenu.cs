using UnityEngine;
using UnityEngine.SceneManagement; // Обязательно для управления сценами

public class MainMenu : MonoBehaviour
{
    // Поле появится в инспекторе, туда пишем имя сцены (например: Level1)
    [SerializeField] private string sceneToLoad;

    // Этот метод мы привяжем к кнопке
    public void ChangeScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("Имя сцены не указано в Инспекторе!");
        }
    }
}

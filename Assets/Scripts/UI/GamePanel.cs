using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    public TextMeshProUGUI LikeText;
    public TextMeshProUGUI HypeTimeText;
    public Slider HypeSlider;
    public Button RestartButton;


    private void Start()
    {
        RestartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        });
    }

    private void Update()
    {
        if (GameManager.Instance.hypeTimer.IsRunning()) HypeTimeText.text = GameManager.Instance.hypeTimer.getTime();
        else HypeTimeText.text = "";
    }

    public void UpdateLikesUI()
    {
        LikeText.text = $"Likes: {GameManager.Instance.Likes}";
    }

    public void UpdateHypeUI()
    {
        HypeSlider.value = GameManager.Instance.Hype;
    }
}

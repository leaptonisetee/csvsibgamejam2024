using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int Likes { get; private set; }
    public int Hype { get; private set; }
    public Timer hypeTimer;
    public float hypeTimerLength = 7f;

    public GamePanel GamePanel;
    private void Start()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        hypeTimer = new Timer(hypeTimerLength);
        SoundManager.Instance.PlayMusic("Music");
    }

    private void Update() 
    {
        hypeTimer.Update(Time.deltaTime);
        if (hypeTimer.IsRunning() && hypeTimer.IsElapsed())
        {
            Hype = 0;
            GamePanel.UpdateHypeUI();
            hypeTimer.Stop();
        }
    }

    public void AddTempLikes(int tempLikes)
    {
        int receive = tempLikes;
        switch (Hype)
        {
            case >= 70:
                receive = tempLikes * 6;
                break;

            case >= 50:
                receive = tempLikes * 4;
                break;

            case >= 20:
                receive = tempLikes * 2;
                break;
        }
        Likes += receive;
        GamePanel.UpdateLikesUI();
    }

    public void AddHype()
    {
        Hype += 10;
        GamePanel.UpdateHypeUI();
        SoundManager.Instance.PlaySound("HypeUp");
        if (!hypeTimer.IsRunning())
        {
            hypeTimer.Reset();
            hypeTimer.Start();
        }
        else
        {
            hypeTimer.AddTime(3);
        }
    }

    public void AntiHype()
    {
        Hype = 0;
        GamePanel.UpdateHypeUI();
        hypeTimer.Stop();
    }
}

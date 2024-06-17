
public class Timer
{
    private float currTime = 0f;
    private float destTime = 0f;
    private bool isRunning = false;

    public Timer(float destTime)
    {
        this.destTime = destTime;
    }

    public void Start()
    {
        isRunning = true;
    }

    public void Stop()
    {
        isRunning = false;
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    public bool IsElapsed()
    {
        return currTime >= destTime;
    }

    public void Reset()
    {
        Stop();
        currTime = 0f;
    }

    public void Update(float deltaTime)
    {
        if (isRunning)
            currTime += deltaTime;
    }

    public string getTime()
    {
        int time = (int)(destTime - currTime);

        return string.Format("{0:00}:{1:00}", time / 60, time % 60);
    }

    public void AddTime(float value)
    {
        currTime -= value;
    }
}

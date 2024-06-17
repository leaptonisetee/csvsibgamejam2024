using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D RB;
    public float MoveAcceleration;
    public float MaxSpeed;
    public float JumpForce;
    public float RotateForce;

    public SpriteRenderer SpriteRenderer;
    private Dictionary<string, Sprite> AnimationDict = new Dictionary<string, Sprite>();
    private float XAxis;

    private int TempLikes;

    private bool onGround;
    private bool inTrick;
    private bool fell;

    private int[] QTEArray;
    private byte QTEHypeCount;
    public TextMeshProUGUI QTEText;
    private bool QTECompleted;

    public float trickCooldown = 0.4f;
    private int trickBufferIndex;
    public float trickBufferLength = 0.2f;
    private Timer trickBufferTimer;

    public float jumpBufferLength = 0.1f;
    private Timer jumpBufferTimer;

    private Timer jumpRestoreTimer;

    private void Awake()
    {
        Sprite[] animSprites = Resources.LoadAll<Sprite>("Images/Character");

        foreach(var sprite in animSprites)
        {
            AnimationDict.Add(sprite.name, sprite);
        }

        trickBufferTimer = new Timer(trickBufferLength);
        jumpBufferTimer = new Timer(jumpBufferLength);
        jumpRestoreTimer = new Timer(0.1f);
    }

    private void Update()
    {
        trickBufferTimer.Update(Time.deltaTime);
        jumpBufferTimer.Update(Time.deltaTime);
        jumpRestoreTimer.Update(Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            jumpBufferTimer.Reset();
            if (onGround && jumpRestoreTimer.IsElapsed())
                Jump();
            else 
            {
                jumpBufferTimer.Start();
            }
        }
        else if (onGround && jumpBufferTimer.IsRunning() && !jumpBufferTimer.IsElapsed() && jumpRestoreTimer.IsElapsed())
        {
            Jump();
            jumpBufferTimer.Stop();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) Trick(3);
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) Trick(4);
        else if (Input.GetKeyDown(KeyCode.DownArrow)) Trick(6);
        else if (Input.GetKeyDown(KeyCode.RightArrow)) Trick(5);
        else if (trickBufferTimer.IsRunning() && !inTrick && !trickBufferTimer.IsElapsed()) 
        {
            Trick(trickBufferIndex);
            trickBufferTimer.Stop();
        }
    }

    private void FixedUpdate()
    {
        if (onGround) Movement();
        else Rotate();
    }

    private void Movement()
    {
        XAxis = Input.GetAxis("Horizontal");
        if (XAxis == 0) return;
        if (RB.velocity.magnitude >= MaxSpeed && Mathf.Sign(RB.velocity.x) == Mathf.Sign(XAxis)) return;

        if (XAxis > 0) SpriteRenderer.transform.localScale = new Vector3(Mathf.Abs(SpriteRenderer.transform.localScale.x), SpriteRenderer.transform.localScale.y, SpriteRenderer.transform.localScale.z);
        else if (XAxis < 0) SpriteRenderer.transform.localScale = new Vector3(-Mathf.Abs(SpriteRenderer.transform.localScale.x), SpriteRenderer.transform.localScale.y, SpriteRenderer.transform.localScale.z);

        Vector2 moveVector = new Vector2(XAxis * MoveAcceleration * Time.deltaTime, 0);
        RB.AddForce(moveVector * transform.right, ForceMode2D.Impulse);
    }

    private void Rotate()
    {
        XAxis = Input.GetAxis("Horizontal");
        if (XAxis == 0) return;

        RB.transform.Rotate(Vector3.forward * -XAxis * RotateForce * Time.deltaTime, Space.Self);
    }

    private void Jump()
    {
        SoundManager.Instance.PlaySound("Jump");
        RB.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
    }

    private void QTEStart()
    {
        QTEArray = new int[Random.Range(2, 4)];
        QTEText.text = "";
        QTEHypeCount = 0;
        QTECompleted = false;
        for (int i = 0; i < QTEArray.Length; i++)
        {
            int trick = Random.Range(3, 7);
            QTEArray[i] = trick;
        }

        QTEText.text = QTEGetString();
    }

    private void QTEPerform(int trickID)
    {
        if (trickID == QTEArray[QTEHypeCount]) QTEHypeCount++;
        else
        {
            QTECompleted = true;
            GameManager.Instance.AntiHype();
        }
        QTEText.text = QTEGetString();
        if (QTEHypeCount >= QTEArray.Length) QTESuccess();
    }

    private void QTESuccess()
    {
        GameManager.Instance.AddHype();
        QTEHypeCount = 0;
        QTECompleted = true;
        QTEText.text = QTEGetString();
    }

    private string QTEGetString()
    {
        string result = "";
        if (!QTECompleted)
        {
            for (int i = 0; i < QTEArray.Length; i++)
            {
                string symbol = "";

                switch (QTEArray[i])
                {
                    case 3:
                        symbol += "\u2191";
                        break;

                    case 4:
                        symbol += "\u2190";
                        break;

                    case 5:
                        symbol += "\u2192";
                        break;

                    case 6:
                        symbol += "\u2193";
                        break;
                }
                if (i < QTEHypeCount) symbol = $"<color=green>{symbol}</color>";
                result += symbol;
            }
        }

        return result;
    }

    private void Trick(int index)
    {
        if(inTrick)
        {
            trickBufferIndex = index;
            trickBufferTimer.Reset();
            trickBufferTimer.Start();
        }
        else if(!inTrick && !fell && !onGround) StartCoroutine(TrickRoutine(index));

    }

    IEnumerator TrickRoutine(int index)
    {
        inTrick = true;
        if (!QTECompleted) QTEPerform(index);

        SpriteRenderer.sprite = AnimationDict[$"{index}"];

        TempLikes += 5;

        yield return new WaitForSeconds(trickCooldown);

        if(!onGround) SpriteRenderer.sprite = AnimationDict["2"];
        inTrick = false;
    }

    public void SwitchOnGroundState(bool _onGround)
    {
        onGround = _onGround;

        if (onGround)
        {
            SpriteRenderer.sprite = AnimationDict["1"];

            GameManager.Instance.AddTempLikes(TempLikes);
            TempLikes = 0;
            QTECompleted = true;
            QTEText.text = QTEGetString();

            jumpRestoreTimer.Reset();
            jumpRestoreTimer.Start();
        }
        else
        {
            SpriteRenderer.sprite = AnimationDict["2"];
            QTEStart();
        }
    }

    public void StandUp()
    {
        if (!fell) StartCoroutine(FallRecovery());
        fell = true;
        GameManager.Instance.AntiHype();
        SoundManager.Instance.PlaySound("Fell");
    }

    IEnumerator FallRecovery()
    {
        SpriteRenderer.sprite = AnimationDict["bam"];
        TempLikes = 0;
        QTECompleted = true;
        QTEText.text = QTEGetString();

        yield return new WaitForSeconds(1);

        transform.rotation = Quaternion.identity;
        RB.velocity = Vector2.zero;
        SpriteRenderer.sprite = AnimationDict["1"];
        fell = false;
    }
}

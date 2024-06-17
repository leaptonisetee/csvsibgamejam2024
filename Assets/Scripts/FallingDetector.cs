using UnityEngine;

public class FallingDetector : MonoBehaviour
{
    public PlayerController Player;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Player.StandUp();
        }
    }
}

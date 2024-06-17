using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public PlayerController Player;

    private Collider2D Collider;

    private void Start()
    {
        Collider = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Player.SwitchOnGroundState(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && !CheckGround())
        {
            Player.SwitchOnGroundState(false);
        }

    }

    private bool CheckGround()
    {
        List<Collider2D> cols = new List<Collider2D>();
        Physics2D.OverlapCollider(Collider, new ContactFilter2D().NoFilter(), cols);

        foreach( Collider2D col in cols)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Ground")) return true;
        }

        return false;
    }
}

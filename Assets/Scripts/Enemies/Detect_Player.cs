using UnityEngine;

public class Detect_Player : MonoBehaviour
{
    [SerializeField]
    private Enemy_Patroling_FlyingFollowing_Shooter parent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        string otherTag = other.gameObject.tag;

        // Atack player if he is in attack area
        if (otherTag == "Player" && !parent.getAttack) {
            parent.StartAttack();
        }
    }
}

using UnityEngine;

public class DetectEnemy : DefaultClass
{
    protected Player player;
    protected Popup textPopup;
    protected ParticleSystem ps;

    protected virtual void Awake()
    {
        player = FindObjectOfType<Player>();
        textPopup = GetComponent<Popup>();
        ps = GetComponent<ParticleSystem>();
    }

    protected void OnParticleCollision(GameObject other)
    {
        // Detect an enemy
        if (other.gameObject.tag == "Enemy") {
            // disable collision after hitting an enemy
            ParticleSystem.CollisionModule coll = ps.collision;
            coll.enabled = false;

            // Deal damage to an enemy
            other.GetComponent<AEnemy>().SetHealth(-player.Damage);

            // Show damage popup
            textPopup.ShowPopup(player.Damage.ToString(), other.transform);
        }
    }
}

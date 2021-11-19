using UnityEngine;

public class Lamp : MonoBehaviour
{
    [SerializeField]
    private GameObject lightParticles;

    public void LightOn()
    {
        lightParticles.SetActive(true);
    }

    public void LightOff()
    {
        lightParticles.SetActive(false);
    }
}

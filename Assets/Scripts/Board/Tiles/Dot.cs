using UnityEngine;

public class Dot : MonoBehaviour
{
    [SerializeField] private ParticleSystem _explosion;
    [SerializeField] private float _destroyDelay;

    public void Remove()
    {
        _explosion.Play();
        Destroy(gameObject, _destroyDelay);
    }
}
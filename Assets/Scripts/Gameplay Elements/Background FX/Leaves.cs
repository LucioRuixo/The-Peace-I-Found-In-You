using UnityEngine;

public class Leaves : MonoBehaviour
{
    [SerializeField] ParticleSystem backgroundParticles = null;
    [SerializeField] ParticleSystem forefrontParticles = null;

    public bool IsPlaying { private set; get; } = false;

    public void Play()
    {
        backgroundParticles.gameObject.SetActive(true);
        backgroundParticles.Play();
        forefrontParticles.gameObject.SetActive(true);
        forefrontParticles.Play();

        IsPlaying = true;
    }

    public void Stop()
    {
        backgroundParticles.Stop();
        backgroundParticles.gameObject.SetActive(false);
        forefrontParticles.Stop();
        forefrontParticles.gameObject.SetActive(false);

        IsPlaying = false;
    }
}
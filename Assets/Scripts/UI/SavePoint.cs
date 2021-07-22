using System.Collections;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public int SavePointNumber;
    private Animation m_Animation;
    private ParticleSystem m_Particle;

    public bool IsCheck { get; set; } = false;

    private void Awake()
    {
        m_Animation = transform.GetComponentInChildren<Animation>();
        m_Particle = transform.GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsCheck) return;
        if (!collision.CompareTag("Player")) return;

        //IsCheck = true;

        m_Animation.Play();
        Debug.Log(m_Animation.clip.name);
        m_Particle.Play();
    }
}

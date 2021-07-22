using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PawnAnimation m_PawnAnimation;
    [SerializeField] private SpriteRenderer m_Renderer;
    [SerializeField] private Rigidbody2D m_Rigidbody;
    [SerializeField] private CapsuleCollider2D m_Collider;
    [SerializeField] private Collider2D m_GroundCollider;
    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private float m_JumpSpeed;
    [SerializeField] private UnityEngine.UI.Slider m_JumpSlider;
    private float m_JumpPower = 0;
    private float m_Bounciness = 0;

    public bool IsLeft { get; private set; } = false;
    public bool IsLockMove { get; private set; } = false;

    private void Awake()
    {
        m_Bounciness = m_Collider.sharedMaterial.bounciness;
    }

#if UNITY_EDITOR
    private void OnDestroy()
    {
        m_Collider.sharedMaterial.bounciness = m_Bounciness;
    }
#endif

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Z))
            m_PawnAnimation.Attack = true;

        if (!m_PawnAnimation.Attack && !m_PawnAnimation.Jump)
        {
            OnMove();
        }

        if (!m_GroundCollider.enabled)
        {
            if (m_PawnAnimation.Jump) m_GroundCollider.enabled = true;
            else if (m_Rigidbody.velocity.y < -0.1f)
            {
                m_PawnAnimation.Jump = true;
                m_GroundCollider.enabled = true;

                m_Collider.sharedMaterial.bounciness = m_Bounciness;
            }
        }
    }

    private void Update()
    {
        if (m_PawnAnimation.Jump) return;
        if (Input.GetKey(KeyCode.Space))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!m_JumpSlider.gameObject.activeSelf) m_JumpSlider.gameObject.SetActive(true);
            }
            m_JumpPower += Time.deltaTime;
            if (m_JumpPower >= 1f) m_JumpPower = 1f;

            m_JumpSlider.value = m_JumpPower;
            IsLockMove = true;

            m_PawnAnimation.Move = false;
            m_PawnAnimation.Attack = false;
        }
        else if (Input.GetKeyUp(KeyCode.Space) && m_JumpPower != 0f)
        {
            m_Rigidbody.velocity = Vector3.zero;
            bool isMove = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
            Vector3 axis = new Vector3(isMove ? transform.right.x * 0.5f : 0, 1, 0);

            if (m_JumpPower != 0f)
            {
                m_JumpPower += 1f;
                AddForceDirection(axis, m_JumpSpeed * m_JumpPower);
                m_PawnAnimation.Jump = true;
            }

            m_JumpPower = 0;
            IsLockMove = false;

            m_JumpSlider.gameObject.SetActive(false);
            m_Collider.sharedMaterial.bounciness = m_Bounciness;
        }
    }

    private void OnMove()
    {
        if (IsLockMove) return;
        if (Input.GetKey(KeyCode.A))
        {
            if (!IsLeft) transform.Rotate(Vector3.up * 180f);

            IsLeft = true;

            Vector3 speed = Vector3.right * m_MoveSpeed * Time.deltaTime;
            transform.Translate(speed);

            m_PawnAnimation.Move = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (IsLeft) transform.Rotate(Vector3.up * 180f);

            IsLeft = false;

            Vector3 speed = Vector3.right * m_MoveSpeed * Time.deltaTime;
            transform.Translate(speed);

            m_PawnAnimation.Move = true;
        }
        else
        {
            m_PawnAnimation.Move = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Grounds")) return;

        if (m_PawnAnimation.Attack) m_PawnAnimation.Attack = false;
        else
        {
            if (m_Rigidbody.velocity.y < 0f)
            {
                m_PawnAnimation.Jump = false;
                m_Rigidbody.velocity = Vector3.zero;
                m_GroundCollider.enabled = false;
                m_Collider.sharedMaterial.bounciness = 0;

                transform.Translate(0, -0.01f, 0);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Untagged")) return;
        if (collision.CompareTag("Grounds"))
        {
            m_PawnAnimation.Jump = false;
            m_Rigidbody.velocity = Vector3.zero;
            m_GroundCollider.enabled = false;
            m_Collider.sharedMaterial.bounciness = 0;
        }
    }

    public void AddForceDirection(Vector3 axis, float force)
    {
        m_Rigidbody.AddForce(axis * force, ForceMode2D.Impulse);
    }

}

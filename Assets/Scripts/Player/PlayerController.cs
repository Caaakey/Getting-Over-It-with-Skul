﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PawnAnimation m_PawnAnimation;
    [SerializeField] private SpriteRenderer m_Renderer;
    [SerializeField] private Rigidbody2D m_Rigidbody;
    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private float m_JumpSpeed;

    public bool IsLeft { get; private set; } = false;

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Z))
            m_PawnAnimation.Attack = true;

        if (!m_PawnAnimation.Attack && !m_PawnAnimation.Jump)
        {
            OnMove();
        }
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        if (!m_PawnAnimation.Jump)
        {
            m_Rigidbody.velocity = Vector3.zero;
            Vector3 axis = new Vector3(m_PawnAnimation.Move ? transform.right.x * 0.5f : 0, 1, 0);

            m_Rigidbody.AddForce(axis * m_JumpSpeed, ForceMode2D.Impulse);

            m_PawnAnimation.Jump = true;
        }
    }

    private void OnMove()
    {
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
        if (collision.CompareTag("Grounds"))
        {
            if (m_PawnAnimation.Attack) m_PawnAnimation.Attack = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Grounds"))
        {
            m_PawnAnimation.Jump = false;
        }
        else 
            m_PawnAnimation.Jump = true;
    }

}
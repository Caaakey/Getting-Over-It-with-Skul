using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public struct CameraRect
    {
        public float Left;
        public float Top;
        public float Right;
        public float Bottom;
    }

    public Camera MainCamera;
    [SerializeField] private Transform m_PlayerTransform;

    private float m_Height = 0;
    private float m_Width = 0;
    private CameraRect m_CameraRect;

    private Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }

    private void Awake()
    {
        m_Height = MainCamera.orthographicSize * 2f;
        m_Width = m_Height * MainCamera.aspect;

        float halfWidth = m_Width / 2f;
        float halfHeight = m_Height / 2f;

        m_CameraRect = new CameraRect()
        {
            Left    = -halfWidth,
            Right   = +halfWidth,
            Top     = +halfHeight,
            Bottom  = -halfHeight
        };
    }

    private void LateUpdate()
    {
        var playerPosition = m_PlayerTransform.position;

        if (playerPosition.x > m_CameraRect.Right)
        {
            Position = new Vector3(Position.x + m_Width, Position.y, 0);
            m_CameraRect.Left += m_Width;
            m_CameraRect.Right += m_Width;
        }
        else if (playerPosition.x < m_CameraRect.Left)
        {
            Position = new Vector3(Position.x - m_Width, Position.y, 0);
            m_CameraRect.Left -= m_Width;
            m_CameraRect.Right -= m_Width;
        }

        if (playerPosition.y > m_CameraRect.Top)
        {
            Position = new Vector3(Position.x, Position.y + m_Height, 0);
            m_CameraRect.Top += m_Height;
            m_CameraRect.Bottom += m_Height;
        }
        else if (playerPosition.y < m_CameraRect.Bottom)
        {
            Position = new Vector3(Position.x, Position.y - m_Height, 0);
            m_CameraRect.Top -= m_Height;
            m_CameraRect.Bottom -= m_Height;
        }
    }
}

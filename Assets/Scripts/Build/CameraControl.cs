using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CameraControl : MonoBehaviour
{
    public bool isRotate;
    public float speedMove = 5;
    public float ratio = 800;//放大缩小速率
    public float min_distance = 5; //相机距物体最小距离
    public float max_distance = 80;//相机距物体最大距离
    private float view_distance;
    //滑动结束时的瞬时速度
    public Vector3 Speed = Vector3.zero;

    //每帧偏差
    private float decelerationRate = 0.2f;//速率衰减值
    private Vector3 offSet = Vector3.zero;
    public Vector3 Rotion_Transform;

    private float direction;
    private float medianX;
    private float medianY;

    private float mouse_Wheel;
    private float mouse_x;
    private float mouse_y;

    public float valuemaxX;
    public float valueminX;
    public float valuemaxZ;
    public float valueminZ;

    private Camera m_Camera;

    private Touch oldTouch1;  //上次触摸点1(手指1)
    private Touch oldTouch2;  //上次触摸点2(手指2)
    private void Awake()
    {
        m_Camera = transform.Find("Camera").GetComponent<Camera>();
        medianY = Screen.height * 0.5f;
        medianX = Screen.width * 0.5f;
    }
    public void UpdataData()
    {
        if (Input.GetMouseButton(0))
        {
            if (Input.touchCount == 1)
            {
                mouse_x = Input.GetTouch(0).deltaPosition.x * 0.1f;
                mouse_y = Input.GetTouch(0).deltaPosition.y * 0.1f;
                offSet.x = mouse_x;
                offSet.y = mouse_y;
                //瞬时速度
                Speed = offSet / (Time.deltaTime * 10);
            }
            else if (Input.touchCount < 1)
            {
                mouse_x = Input.GetAxis("Mouse X");
                mouse_y = Input.GetAxis("Mouse Y");
                offSet.x = mouse_x;
                offSet.y = mouse_y;
                //瞬时速度
                Speed = offSet / (Time.deltaTime * 10);
            }
        }
        else
        {
            Speed *= Mathf.Pow(decelerationRate, Time.deltaTime);
            if (Mathf.Abs(Vector3.Magnitude(Speed)) < 1)
            {
                Speed = Vector3.zero;
            }
        }
        if (isRotate)
        {
            MoveRotate(Speed);
        }
        else
        {
            Came_Ctrl_Move(Speed);
        }
    }
    //镜头的移动
    void Came_Ctrl_Move(Vector3 diff)
    {
        if (Vector3.Magnitude(diff) == 0)
        {
            return;
        }
        if (transform.localPosition.z >= valueminZ && transform.localPosition.z <= valuemaxZ)
        {
            transform.Translate(-Vector3.forward * diff.y * Time.deltaTime * speedMove, Space.Self);
        }
        if (transform.localPosition.x >= valueminX && transform.localPosition.x <= valuemaxX)
        {
            transform.Translate(Vector3.left * diff.x * Time.deltaTime * speedMove, Space.Self);
        }
        Vector3 forward = transform.localPosition;
        forward.z = Mathf.Clamp(forward.z, valueminZ, valuemaxZ);
        forward.x = Mathf.Clamp(forward.x, valueminX, valuemaxX);
        transform.localPosition = forward;
    }
    //镜头的旋转
    void MoveRotate(Vector3 speed)
    {
        if (Vector3.Magnitude(speed) == 0)
        {
            return;
        }
        if (speed.y != 0)
        {
            if (Input.mousePosition.x > medianX)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }
            Cam_Ctrl_Rotation(direction * speed.y);
        }
        if (speed.x != 0)
        {
            if (Input.mousePosition.y < medianY)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }
            Cam_Ctrl_Rotation(direction * speed.x);
        }
        UIBase.Instance.UpdataPoint();
    }
    void Cam_Ctrl_Rotation(float m_x)
    {
        transform.RotateAround(Rotion_Transform, Vector3.up, m_x * Time.deltaTime);
    }
    //镜头缩放  //多点触摸, 放大缩小
    public void ScrollMouse()
    {
        if (Input.touchCount > 1)
        {
            Touch newTouch1 = Input.GetTouch(0);
            Touch newTouch2 = Input.GetTouch(1);
            if (newTouch2.phase == TouchPhase.Began)
            {
                oldTouch2 = newTouch2;
                oldTouch1 = newTouch1;
                return;
            }
            if (newTouch2.phase == TouchPhase.Moved || newTouch1.phase == TouchPhase.Moved)
            {
                float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
                float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);
                mouse_Wheel = newDistance - oldDistance;
                if (mouse_Wheel != 0)
                {
                    ratio = 1;
                    if (mouse_Wheel > 0)
                    {
                        ratio = -1;
                    }
                    view_distance = m_Camera.fieldOfView;
                    view_distance += ratio;
                    view_distance = Mathf.Clamp(view_distance, min_distance, max_distance);
                    m_Camera.fieldOfView = view_distance;
                }
            }
            //记住最新的触摸点，下次使用
            oldTouch1 = newTouch1;
            oldTouch2 = newTouch2;
        }
        else
        {
            mouse_Wheel = Input.GetAxis("Mouse ScrollWheel");
            if (mouse_Wheel != 0)
            {
                int index = 1;
                if (mouse_Wheel <= 0)
                {
                    index = -1;
                }
                view_distance = m_Camera.fieldOfView;
                view_distance += index * ratio;
                view_distance = Mathf.Clamp(view_distance, min_distance, max_distance);
                m_Camera.fieldOfView = view_distance;
            }
        }
    }
    //设置中心点
    public void SetCenter(Vector3 point)
    {
        isRotate = !isRotate;
        if(isRotate)
        {
            Rotion_Transform = point;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_Object : MonoBehaviour
{

    public Rigidbody Rb;
    public Gravity Gravity; // 当前重力对象
    RaycastHit HitInfo;
    bool isHit = false;

    private void Awake()
    {
        Rb = gameObject.GetComponent<Rigidbody>();
    }
    private void Update()
    {
        RayCast();
        SetGravity();
    }

    private void RayCast()
    {
        isHit = Physics.Raycast(transform.position, Vector3.down, out HitInfo, 3f, LayerMask.GetMask("Platform"));
        Vector3 forward = transform.forward;
        if (HitInfo.collider.gameObject.GetComponent<Gravity>())
        {
            Gravity = HitInfo.collider.gameObject.GetComponent<Gravity>();
        }

        Vector3 newUp;
        if (isHit)
        {
            newUp = HitInfo.normal;
        }
        else
        {
            newUp = Vector3.up;
        }

        Vector3 left = Vector3.Cross(forward, newUp);//note: unity use left-hand system, and Vector3.Cross obey left-hand rule.
        Vector3 newForward = Vector3.Cross(newUp, left);
        Quaternion oldRotation = transform.rotation;
        Quaternion newRotation = Quaternion.LookRotation(newForward, newUp);

        float kSoftness = 0.1f;//if do not want softness, change the value to 1.0f
        Rb.MoveRotation(Quaternion.Lerp(oldRotation, newRotation, kSoftness));
    }

    private void SetGravity()
    {
        Vector3 axis_old = transform.up;
        Vector3 axis_new = axis_old;
        if (Gravity)
        {
            switch (Gravity.thisType)
            {
                case GravityType.Sphere_away:
                    axis_new = transform.position - Gravity.gameObject.transform.position;
                    break;
                case GravityType.Sphere_into:
                    axis_new = Gravity.gameObject.transform.position - transform.position;
                    break;
                case GravityType.Static_away:
                    axis_new = Gravity.gameObject.transform.up;
                    break;
                case GravityType.Static_into:
                    axis_new = -Gravity.gameObject.transform.up;
                    break;
                case GravityType.Unshape_away:
                    axis_new = HitInfo.normal;
                    break;
                case GravityType.UnShape_into:
                    axis_new = -HitInfo.normal;
                    break;
            }
        }
        Physics.gravity = axis_new.normalized * Gravity.GravityAccelerate;
    }

    private bool IsInto(GravityType type) // 似乎没什么用的样子
    {
        if (type == GravityType.Static_into || type == GravityType.Sphere_into || type == GravityType.UnShape_into)
        {
            return true;
        }
        else return false;
    }
}

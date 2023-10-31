using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;

    public int sensitivity = 120;
    [HideInInspector] public float incspeed = 6;//6が標準
    //public float maxincspeed;

    float auto_control_power = 0;
    float auto_control_power_t = 0;
    float auto_control_power_per_f = 0.01f;
    [HideInInspector] [SerializeField] GameObject auto_control_prefab = null;
    GameObject auto_control_object = null;

    float RingPower;
    float rotpower = 0f;

    float rotpower_roll = 0f;
    float rotpower_pitch = 0f;

    Vector3 PlayerForce;

    [SerializeField] GameObject canvas = null;

    [HideInInspector] [SerializeField] GameObject pause_prefab = null;
    GameObject pause;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        transform.Rotate(Vector3.right, incspeed * Input.GetAxis("Mouse Y"));
        transform.Rotate(-Vector3.forward, incspeed * Input.GetAxis("Mouse X"));


        float rotpower_per_f = 0.05f;

        //ここからマウス使えない時用
        {
            if (Input.GetKey(KeyCode.K) && (Input.GetKey(KeyCode.Equals) == false))
            {
                if (rotpower_roll < 0f) rotpower_roll += rotpower_per_f;
                if (rotpower_roll < 1f) rotpower_roll += rotpower_per_f;
                else rotpower_roll = 1f;
            }
            else if (Input.GetKey(KeyCode.Equals) && (Input.GetKey(KeyCode.K) == false))
            {
                if (rotpower_roll > 0f) rotpower_roll -= rotpower_per_f;
                if (rotpower_roll > -1f) rotpower_roll -= rotpower_per_f;
                else rotpower_roll = -1f;
            }
            else
            {
                if (rotpower_roll < 0f)
                {
                    if (-rotpower_roll < rotpower_per_f) rotpower_roll = 0f;
                    else rotpower_roll += rotpower_per_f;
                }
                else if (rotpower_roll > 0f)
                {
                    if (rotpower_roll < rotpower_per_f) rotpower_roll = 0f;
                    else rotpower_roll -= rotpower_per_f;
                }
            }
            float rotspeed = 0f;
            if (rotpower_roll >= 0f) rotspeed = 1.8f * Mathf.SmoothStep(0f, 1f, rotpower_roll);
            if (rotpower_roll < 0f) rotspeed = 1.8f * Mathf.SmoothStep(0f, -1f, -rotpower_roll);
            transform.Rotate(Vector3.forward, rotspeed);
        }
        {
            if (Input.GetKey(KeyCode.O) && (Input.GetKey(KeyCode.L) == false))
            {
                if (rotpower_pitch < 0f) rotpower_pitch += rotpower_per_f;
                if (rotpower_pitch < 1f) rotpower_pitch += rotpower_per_f;
                else rotpower_pitch = 1f;
            }
            else if (Input.GetKey(KeyCode.L) && (Input.GetKey(KeyCode.O) == false))
            {
                if (rotpower_pitch > 0f) rotpower_pitch -= rotpower_per_f;
                if (rotpower_pitch > -1f) rotpower_pitch -= rotpower_per_f;
                else rotpower_pitch = -1f;
            }
            else
            {
                if (rotpower_pitch < 0f)
                {
                    if (-rotpower_pitch < rotpower_per_f) rotpower_pitch = 0f;
                    else rotpower_pitch += rotpower_per_f;
                }
                else if (rotpower_pitch > 0f)
                {
                    if (rotpower_pitch < rotpower_per_f) rotpower_pitch = 0f;
                    else rotpower_pitch -= rotpower_per_f;
                }
            }
            float rotspeed = 0f;
            if (rotpower_pitch >= 0f) rotspeed = 1.8f * Mathf.SmoothStep(0f, 1f, rotpower_pitch);
            if (rotpower_pitch < 0f) rotspeed = 1.8f * Mathf.SmoothStep(0f, -1f, -rotpower_pitch);
            transform.Rotate(Vector3.right, rotspeed);
        }
        //ここまでマウス使えない時用


        {
            if (Input.GetKey(KeyCode.D) && (Input.GetKey(KeyCode.A) == false))
            {
                if (rotpower < 0f) rotpower += rotpower_per_f;
                if (rotpower < 1f) rotpower += rotpower_per_f;
                else rotpower = 1f;
            }
            else if (Input.GetKey(KeyCode.A) && (Input.GetKey(KeyCode.D) == false))
            {
                if (rotpower > 0f) rotpower -= rotpower_per_f;
                if (rotpower > -1f) rotpower -= rotpower_per_f;
                else rotpower = -1f;
            }
            else
            {
                if (rotpower < 0f)
                {
                    if (-rotpower < rotpower_per_f) rotpower = 0f;
                    else rotpower += rotpower_per_f;
                }
                else if (rotpower > 0f)
                {
                    if (rotpower < rotpower_per_f) rotpower = 0f;
                    else rotpower -= rotpower_per_f;
                }
            }
            float rotspeed = 0f;
            if (rotpower >= 0f) rotspeed = 1.8f * Mathf.SmoothStep(0f, 1f, rotpower);
            if (rotpower < 0f) rotspeed = 1.8f * Mathf.SmoothStep(0f, -1f, -rotpower);
            transform.Rotate(Vector3.up, rotspeed, Space.World);
        }

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            if (auto_control_object == null)
            {
                auto_control_object = Instantiate(auto_control_prefab, canvas.transform) as GameObject;
            }

            if (auto_control_power_t < 1)
            {
                auto_control_power_t += auto_control_power_per_f;
            }
            else
            {
                auto_control_power_t = 1f;
            }

            float angle = Vector3.Angle(transform.up, Vector3.up);
            if (angle >= 30)
            {
                auto_control_power = 3 * Mathf.SmoothStep(0f, 1f, auto_control_power_t);
            }
            else
            {
                auto_control_power = 3 * Mathf.SmoothStep(0f, 1f, auto_control_power_t) * angle / 30;
            }

            Vector3 forward2 = new Vector3(transform.forward.x, 0, transform.forward.z);

            Quaternion forward2_q = Quaternion.LookRotation(forward2);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, forward2_q, auto_control_power);
        }
        else
        {
            if (auto_control_object != null)
            {
                Destroy(auto_control_object);
            }

            auto_control_power_t = 0;
        }


        RingPower = 1f;
        if (Input.GetKey(KeyCode.W)) RingPower = 2f;
        if (Input.GetKey(KeyCode.S)) RingPower = 0.25f;

        PlayerForce = transform.up * (-Physics.gravity.y) * RingPower;

        float inclination_angle = Vector3.Angle(Vector3.up, transform.up);

        rb.AddForce(PlayerForce);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && pause == null)
        {
            pause = Instantiate(pause_prefab, canvas.transform) as GameObject;
            Time.timeScale = 0;
        }
    }

    public void DestroyPause()
    {
        Destroy(pause);
        Time.timeScale = 1;
    }
}
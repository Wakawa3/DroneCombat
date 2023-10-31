using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingMissile : MonoBehaviour
{
    [SerializeField] Transform[] Launcher = new Transform[2];
    int next_launcher = 0;

    [SerializeField] GameObject Missile = null;
    GameObject Missiles;
    [HideInInspector] public GameObject Target = null;
    [HideInInspector] public int targetID = -1;
    int targetID_former;
    [HideInInspector] public float lock_on_degree;
    public float max_lock_on_range;
    public float max_lock_on_screen_range;//0から0.5の間

    GameObject management;
    EnemyManagement em;

    EnergyIndicatorScript eis;
    public float required_energy = 45;

    void Start()
    {
        management = GameObject.FindWithTag("Management");
        em = management.GetComponent<EnemyManagement>();

        eis = GetComponent<EnergyIndicatorScript>();
    }

    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        float[] ScreenDistanceFromCenter = new float[em.EnemyObject.Length]; //毎フレーム配列を作ってしまっている
        float[] DistanceFromPlayer = new float[em.EnemyObject.Length];
        float[] value = new float[em.EnemyObject.Length];

        float[] SelectingValue = ValueOfSelectingTarget(ScreenDistanceFromCenter, DistanceFromPlayer, value);

        if (Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.P))
        {
            Target = ChangeTarget(SelectingValue);
        }

        LockOnDegreeCalculation();

        targetID_former = targetID;


        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.I))
        {
            Launch(Launcher[next_launcher]);
            next_launcher++;
            if(next_launcher >= Launcher.Length)
            {
                next_launcher = 0;
            }
        }
    }

    float[] ValueOfSelectingTarget(float[] ScreenDistanceFromCenter, float[] DistanceFromPlayer, float[] value)
    {

        for (int i = 0; i < em.EnemyObject.Length; i++)
        {
            if (em.EnemyObject[i] == null)
            {
                value[i] = -2; //オブジェクトがなければ-2
            }
            else if (em.enemy_is_rendered[i] == false)
            {
                value[i] = -1; //オブジェクトがカメラの外なら-1
            }
            else
            {
                ScreenDistanceFromCenter[i] = Vector2.Distance(new Vector2(Screen.width / 2, Screen.height / 2) / Screen.height
                                                              , em.enemy_screen_pos[i] / Screen.height);
                DistanceFromPlayer[i] = Vector3.Distance(transform.position, em.EnemyObject[i].transform.position);
                value[i] = ScreenDistanceFromCenter[i] * 1500 + DistanceFromPlayer[i]; //DistanceFromPlayerの値は要調整
                //valueが小さいものにロックオンする
            }
           
        }
        return value;
    }

    GameObject ChangeTarget(float[] Value)
    {
        float MinValue = 0;
        GameObject PrimaryTarget = null;
        //targetID = -1;

        for (int i = 0; i < em.EnemyObject.Length; i++)
        {
            if (Value[i] >= 0)
            {
                if (Value[i] < MinValue || MinValue == 0)
                {
                    MinValue = Value[i];
                    PrimaryTarget = em.EnemyObject[i];
                    targetID = i;
                }
            }
        }
        return PrimaryTarget;
    }

    void Launch(Transform launcher)
    {
        if (eis.missile_energy >= required_energy)
        {
            Missiles = Instantiate(Missile, launcher.position, transform.rotation) as GameObject;
            eis.missile_energy -= required_energy;
        }
    }
    
    void LockOnDegreeCalculation() //lock_on_degreeの操作
    {
        if (targetID != targetID_former) lock_on_degree = 0;

        if (Target != null && em.enemy_is_rendered[targetID])
        {
            float distance = Vector3.Distance(Target.transform.position, GameObjectManagement.player.transform.position);

            if (distance <= max_lock_on_range)
            {
                Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);
                Vector2 converted_center = center / Screen.height;
                Vector2 converted_pos = em.enemy_screen_pos[targetID] / Screen.height;

                float converted_screen_distance = Vector2.Distance(converted_center, converted_pos);

                if (converted_screen_distance <= max_lock_on_screen_range)
                {
                    lock_on_degree += (1 - converted_screen_distance / max_lock_on_screen_range) * 7 * Time.deltaTime * 60;
                }
                else if (converted_screen_distance <= max_lock_on_screen_range + 0.1f)
                {

                }
                else
                {
                    lock_on_degree -= (-1 + converted_screen_distance / (max_lock_on_screen_range + 0.1f)) * 7 * Time.deltaTime * 60;
                }

            }
            else
            {

                lock_on_degree -= 20 * Time.deltaTime * 60;

            }

        }
        else lock_on_degree = 0;

        if (lock_on_degree < 0) lock_on_degree = 0;
        else if (lock_on_degree > 100) lock_on_degree = 100;
    }
}
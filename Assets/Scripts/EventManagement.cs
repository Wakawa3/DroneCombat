using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//EnemyApperanceより後に処理
public class EventManagement : MonoBehaviour
{
    [SerializeField] GameObject canvas1 = null;

    [HideInInspector] [SerializeField] GameObject game_over_ui_prefab = null;
    [HideInInspector] [SerializeField] GameObject game_over_menu = null;
    [HideInInspector] [SerializeField] GameObject stage_clear_ui_prefab = null;
    [SerializeField] Image event_text_back = null;
    [SerializeField] Text event_text_speaker_object = null;
    [SerializeField] Text event_text_object = null;

    EnemyManagement em;
    TimeManagement tm;

    GameObject player;

    [SerializeField] EventClass[] event_class = new EventClass[0];

    bool shift_event_group = false;
    [HideInInspector] public bool[] event_processed;//external_triggerで使う
    int event_group_ongoing = 0;
    
    EventTextClass displayed_event_text = null;//表示されていなければ-1
    EventTextClass reserved_text = null;
    IEnumerator event_text_ie = null;
    
    public enum EventTriggerType
    {
        external_trigger, //event_groupは指定できない
        score_more,
        score_less,
        time_limit,
        time_from_event,
        number_of_enemies,
        number_of_targets,
        HP_of_target,
        player_pos,
        time_and_player_pos,
        specific_enemy_pos_out,
        touch_trigger_collider_player,
        touch_trigger_collider_specific_enemy
    }

    public enum EventMethodType
    {
        shift_next_event_group,
        enemy_set_active,
        destroy_enemy,
        destroy_all_enemies, //他のイベントと同時に起こりEnemyAppearnceが実行される前に処理されないようにする
        reset_time_limit,
        display_event_text,
        cancel_reserved_text,
        game_over,
        stage_clear
    }

    public enum EventTextSpeaker
    {
        クリア条件,
        指揮官
    }

    [System.Serializable]
    public class EventClass
    {
        public string event_name;//インスペクタ用のメモ

        public int[] event_group;

        public EventTriggerClass event_trigger;
        
        public EventMethodClass[] event_method;
    }

    [System.Serializable]
    public class Array2Dint
    {
        public int[] int_2D;
    }

    [System.Serializable]
    public class Array2Dfloat
    {
        public float[] float_2D;
    }

    [System.Serializable]
    public class Array2DGameObject
    {
        public GameObject[] gameObject_2D;
    }

    [System.Serializable]
    public class EventTriggerClass
    {
        public EventTriggerType event_trigger_type;
        public int trigger_int;
        public float trigger_float;
        public Vector3 trigger_vector3;
        public GameObject[] trigger_gameObjects;
    }

    [System.Serializable]
    public class EventMethodClass
    {
        public EventMethodType event_method_type;
        public float method_float;
        public GameObject[] method_gameObjects;
        public EventTextClass method_event_text;
    }

    [System.Serializable]
    public class EventTextClass
    {
        public float[] display_time = new float[0];
        public float[] interval_time = new float[0];
        public int priority_level = 0; //大きい数のほうが優先度高
        public bool reserve_text = false; //1つまでテキストを予約できる
        public bool random_text = false;
        public EventTextSpeaker[] event_text_speaker = new EventTextSpeaker[0];
        [Multiline] public string[] event_text = new string[0];
    }

    /*void Awake()
    {
        foreach (EventClass eventclass in event_class)
        {
            foreach (EventMethodClass eventmethod in eventclass.event_method)
            {
                foreach (GameObject gameobject in eventmethod.method_gameobject)
                {
                    if (gameobject.GetComponent<GameObjectGroupScript>() != null)
                    {
                        int default_length = eventmethod.method_gameobject.Length;

                        Transform[] children_transforms = gameobject.GetComponentsInChildren<Transform>();
                        GameObject[] children_gameobjects = new GameObject[children_transforms.Length];
                        for (int i = 0; i < children_transforms.Length; i++)
                        {
                            children_gameobjects[i] = children_transforms[i].gameObject; 
                        }

                        System.Array.Resize(ref eventmethod.method_gameobject, default_length + children_gameobjects.Length);

                        children_gameobjects.CopyTo(eventmethod.method_gameobject, default_length);
                    }
                }
            }
        }
    }*/

    void Start()
    {
        em = GetComponent<EnemyManagement>();
        tm = GetComponent<TimeManagement>();

        player = GameObjectManagement.player;

        event_processed = new bool[event_class.Length];
        for (int i = 0; i < event_processed.Length; i++)
        {
            event_processed[i] = false;
        }
    }

    void FixedUpdate()
    {
        //event_textの処理
        if (event_text_ie == null)
        {
            if (reserved_text != null)
            {
                event_text_ie = EventTextCoroutine(reserved_text);
                reserved_text = null;
                StartCoroutine(event_text_ie);
            }
        }

        if (shift_event_group)
        {
            event_group_ongoing++;
            shift_event_group = false;
        }

        for (int i = 0; i < event_class.Length; i++)
        {
            if (event_processed[i] == false)
            {
                for (int j = 0; j < event_class[i].event_group.Length; j++)
                {
                    if (event_class[i].event_group[j] == event_group_ongoing && JudgeEventTrigger(i))
                    {
                        RunEventMethod(i);
                        break;
                    }
                }
            }
        }
    }



    bool JudgeEventTrigger(int event_number)
    {
        EventTriggerType type = event_class[event_number].event_trigger.event_trigger_type;

        bool judgement = false;

        switch (type)
        {
            case EventTriggerType.score_more:
                if (ScoreScript.score >= event_class[event_number].event_trigger.trigger_int)
                {
                    judgement = true;
                }
                break;

            case EventTriggerType.score_less:
                if (ScoreScript.score < event_class[event_number].event_trigger.trigger_int)
                {
                    judgement = true;
                }
                break;

            case EventTriggerType.time_limit:
                if (tm.time_limit <= event_class[event_number].event_trigger.trigger_float)
                {
                    judgement = true;
                }
                break;

            case EventTriggerType.number_of_enemies:
                if (em.number_of_enemies <= event_class[event_number].event_trigger.trigger_int)
                {
                    judgement = true;
                }
                break;

            case EventTriggerType.number_of_targets:
                if (em.number_of_targets <= event_class[event_number].event_trigger.trigger_int)
                {
                    judgement = true;
                }
                break;

            case EventTriggerType.player_pos:
                {
                    Vector3 pos2 = player.transform.position;
                    pos2.y = 0;

                    Vector3 event_pos2 = event_class[event_number].event_trigger.trigger_vector3;
                    event_pos2.y = 0;

                    float diff2 = Vector3.Distance(pos2, event_pos2);
                    if (diff2 <= event_class[event_number].event_trigger.trigger_float)
                    {
                        judgement = true;
                    }
                    break;
                }

            case EventTriggerType.specific_enemy_pos_out:
                {
                    foreach (GameObject gameObject in event_class[event_number].event_trigger.trigger_gameObjects)
                    {
                        if (gameObject != null)
                        {
                            Vector3 pos2 = gameObject.transform.position;
                            pos2.y = 0;

                            Vector3 event_pos2 = event_class[event_number].event_trigger.trigger_vector3;
                            event_pos2.y = 0;

                            float diff2 = Vector3.Distance(pos2, event_pos2);
                            if (diff2 >= event_class[event_number].event_trigger.trigger_float)
                            {
                                judgement = true;
                                break;
                            }
                        }
                    }
                    break;
                }
        }
        
        return judgement;
    }

    public void RunEventMethod(int event_number)
    {
        event_processed[event_number] = true;

        for (int i = 0; i < event_class[event_number].event_method.Length; i++)
        {
            EventMethodType type = event_class[event_number].event_method[i].event_method_type;

            switch (type)
            {
                case EventMethodType.shift_next_event_group:
                    shift_event_group = true; //ここでevent_group_ongoing++してしまうと同じフレーム中に次のevent_groupの処理が実行されてしまう
                    break;

                case EventMethodType.enemy_set_active:
                    foreach (GameObject inactive_enemy in event_class[event_number].event_method[i].method_gameObjects)
                    {
                        inactive_enemy.SetActive(true);
                    }
                    break;

                case EventMethodType.destroy_enemy:
                    for (int j = 0; j < event_class[event_number].event_method[i].method_gameObjects.Length; j++)
                    {
                        //Debug.Log(method_destroy_enemy[event_number].game_object[j].GetComponent<EnemyAppearance>().EnemyObjectID);
                    }
                    break;

                case EventMethodType.destroy_all_enemies:
                    foreach (GameObject enemy in em.EnemyObject)
                    {
                        if (enemy != null)
                        {
                            enemy.GetComponent<EnemyHPScript>().DestroyEnemy();
                        }
                    }
                    break;

                case EventMethodType.reset_time_limit:
                    tm.ResetTimeLimit(event_class[event_number].event_method[i].method_float);
                    break;

                case EventMethodType.display_event_text:
                    if (event_text_ie == null)//この条件とdisplayed_event_text_number == -1は同値
                    {
                        event_text_ie = EventTextCoroutine(event_class[event_number].event_method[i].method_event_text);
                        StartCoroutine(event_text_ie);
                    }
                    else if (event_class[event_number].event_method[i].method_event_text.priority_level > displayed_event_text.priority_level)
                    {
                        StopCoroutine(event_text_ie);
                        event_text_ie = null;
                        event_text_ie = EventTextCoroutine(event_class[event_number].event_method[i].method_event_text);
                        StartCoroutine(event_text_ie);
                    }
                    else
                    {
                        if (event_class[event_number].event_method[i].method_event_text.reserve_text)
                        {
                            reserved_text = event_class[event_number].event_method[i].method_event_text;
                        }
                    }
                    break;

                case EventMethodType.cancel_reserved_text:
                    reserved_text = null;
                    break;

                case EventMethodType.game_over:
                    StartCoroutine(GameOverCoroutine(5));
                    break;

                case EventMethodType.stage_clear:
                    StartCoroutine(StageClearCoroutine(5));
                    break;
            }
        }
    }

    IEnumerator GameOverCoroutine(float wait_time)
    {
        tm.stop_time = true;

        Instantiate(game_over_ui_prefab, canvas1.transform);

        float t = 0;
        while (t < wait_time)
        {
            t += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        Time.timeScale = 0;
        Instantiate(game_over_menu, canvas1.transform);
    }

    IEnumerator StageClearCoroutine(float wait_time)
    {
        tm.stop_time = true;

        Instantiate(stage_clear_ui_prefab, canvas1.transform);

        float t = 0;
        while (t < wait_time)
        {
            t += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        SceneManager.LoadScene("ClearScene");
    }

    IEnumerator EventTextCoroutine(EventTextClass event_text_class)
    {

        displayed_event_text = event_text_class;

        if (event_text_class.random_text == false)
        {
            for (int i = 0; i < event_text_class.event_text.Length; i++)
            {
                event_text_back.color = new Color(0, 0, 0, 55f / 255f);

                event_text_speaker_object.text = event_text_class.event_text_speaker[i].ToString();
                event_text_speaker_object.color = new Color(1, 1, 1, 1);

                event_text_object.text = event_text_class.event_text[i];
                event_text_object.color = new Color(1, 1, 1, 1);


                float t = 0;
                while (t < event_text_class.display_time[i])
                {
                    t += Time.deltaTime;
                    yield return new WaitForFixedUpdate();
                }

                event_text_speaker_object.color = new Color(1, 1, 1, 0);
                event_text_object.color = new Color(1, 1, 1, 0);

                t = 0;
                while (t < event_text_class.interval_time[i])
                {
                    t += Time.deltaTime;
                    yield return new WaitForFixedUpdate();
                }
            }
        }
        else
        {
            int text_number = Random.Range(0, event_text_class.event_text.Length); //minの値は含みmaxの値は含まないことに注意


            event_text_back.color = new Color(0, 0, 0, 55f / 255f);

            event_text_speaker_object.text = event_text_class.event_text_speaker[text_number].ToString();
            event_text_speaker_object.color = new Color(1, 1, 1, 1);

            event_text_object.text = event_text_class.event_text[text_number];
            event_text_object.color = new Color(1, 1, 1, 1);


            float t = 0;
            while (t < event_text_class.display_time[text_number])
            {
                t += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }

            event_text_speaker_object.color = new Color(1, 1, 1, 0);
            event_text_object.color = new Color(1, 1, 1, 0);
        }

        event_text_back.color = new Color(0, 0, 0, 0);

        displayed_event_text = null;
        event_text_ie = null;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//OptionScriptより後に処理
public class AdjustSensitivityScript : MonoBehaviour
{
    GameObject player;
    PlayerController pc;

    [HideInInspector] public bool activated = false;

    [SerializeField] RectTransform gray_back = null;
    [SerializeField] RectTransform blue_back = null;
    [SerializeField] RectTransform slider = null;

    int max_sensitivity = 150;
    int min_sensitivity = 50;

    [SerializeField] Text sensitivity_text = null;

    OptionScript op;

    IEnumerator adjust_sensitivity = null;

    bool is_activated_frame = true;

    void Start()
    {
        player = GameObjectManagement.player;
        pc = player.GetComponent<PlayerController>();

        op = transform.parent.gameObject.GetComponent<OptionScript>();

        RenderSlider();
    }

    void Update()
    {
        if (activated == false)
        {
            return;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (pc.sensitivity < max_sensitivity)
            {
                if (adjust_sensitivity == null)
                {
                    adjust_sensitivity = AdjustSensitivity(true);
                    StartCoroutine(adjust_sensitivity);
                }
            }

            RenderSlider();
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (pc.sensitivity > min_sensitivity)
            {
                if (adjust_sensitivity == null)
                {
                    adjust_sensitivity = AdjustSensitivity(false);
                    StartCoroutine(adjust_sensitivity);
                }
            }

            RenderSlider();
        }
        else
        {
            if (adjust_sensitivity != null)
            {
                StopCoroutine(adjust_sensitivity);
                adjust_sensitivity = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Backspace))
        {
            BackToOption();
            return;
        }
        //開いた瞬間前の画面に戻るのをis_activated_frameで防ぐ
        if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return)) && is_activated_frame == false)
        {
            BackToOption();
            Debug.Log("a");
            return;
        }

        is_activated_frame = false;
    }

    IEnumerator AdjustSensitivity(bool positive_adjustment)
    {
        Adjust(positive_adjustment);
        yield return null;

        for (int i = 0; i < 30; i++)
        {
            yield return null;
        }

        for (; ; )
        {
            Adjust(positive_adjustment);
            yield return null;
            yield return null;
        }
    }

    void Adjust(bool positive_adjustment)
    {
        if (positive_adjustment)
        {
            pc.sensitivity++;
            pc.incspeed = pc.sensitivity / 20f;
        }
        else
        {
            pc.sensitivity--;
            pc.incspeed = pc.sensitivity / 20f;
        }
    }

    void RenderSlider()
    {
        blue_back.sizeDelta = new Vector2(gray_back.sizeDelta.x * (pc.sensitivity - min_sensitivity) / (max_sensitivity - min_sensitivity)
                                        , gray_back.sizeDelta.y);
        blue_back.anchoredPosition = new Vector2(gray_back.anchoredPosition.x - gray_back.sizeDelta.x / 2 + blue_back.sizeDelta.x / 2, gray_back.anchoredPosition.y);

        slider.anchoredPosition = new Vector2(gray_back.anchoredPosition.x - gray_back.sizeDelta.x / 2 + blue_back.sizeDelta.x, blue_back.anchoredPosition.y);
        sensitivity_text.text = "" + pc.sensitivity;
    }

    void BackToOption()
    {
        op.activated = true;
        is_activated_frame = true;
        activated = false;
    }
}

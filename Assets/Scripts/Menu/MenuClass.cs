using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuClass : MonoBehaviour
{
    protected int choice_number = 0;
    
    [SerializeField] RectTransform cursor = null;
    Image cursor_image;
    [SerializeField] Color cursor_color = Color.white;//デフォルトの色

    [SerializeField] Text explanation_text = null; 

    [SerializeField] protected GameObject[] menu_choice = new GameObject[0];
    RectTransform[] menu_choice_rt = new RectTransform[0];
    Text[] menu_choice_text = new Text[0];

    [SerializeField] [Multiline] String[] explanation_text_string = new string[0];//基本Inspector上で指定

    [HideInInspector] public bool activated = true;

    [SerializeField] bool when_inactivated_hide_menu = true;
    [SerializeField] Image[] extra_hide_image = new Image[0];
    [SerializeField] Text[] extra_hide_text = new Text[0];

    MenuClass base_mc;

    protected void Start()
    {
        Array.Resize(ref menu_choice_rt, menu_choice.Length);
        Array.Resize(ref menu_choice_text, menu_choice.Length);
        for (int i = 0; i < menu_choice.Length; i++)
        {
            menu_choice_rt[i] = menu_choice[i].GetComponent<RectTransform>();
            menu_choice_text[i] = menu_choice[i].GetComponent<Text>();
        }

        Array.Resize(ref explanation_text_string, menu_choice.Length);//念のためエラー防止

        cursor_image = cursor.GetComponent<Image>();

        activated = true;

        choice_number = 0;
        CursorShift();

        base_mc = transform.parent.gameObject.GetComponent<MenuClass>();
    }

    protected void Update()
    {
        if (activated == false)
        {
            if (when_inactivated_hide_menu)
            {
                cursor_image.color = new Color(0, 0, 0, 0);

                for (int i = 0; i < menu_choice_text.Length; i++)
                {
                    Color color = menu_choice_text[i].color;
                    color.a = 0;
                    menu_choice_text[i].color = color;
                }

                for (int i = 0; i < extra_hide_image.Length; i++)
                {
                    Color color = extra_hide_image[i].color;
                    color.a = 0;
                    extra_hide_image[i].color = color;
                }

                for (int i = 0; i < extra_hide_text.Length; i++)
                {
                    Color color = extra_hide_text[i].color;
                    color.a = 0;
                    extra_hide_text[i].color = color;
                }
            }
            else
            {
                cursor_image.color = new Color(232f / 255f, 238f / 255f, 44f / 255f);
            }
            
            return;
        }
        else
        {
            cursor_image.color = cursor_color;

            if (when_inactivated_hide_menu)
            {
                for (int i = 0; i < menu_choice_text.Length; i++)
                {
                    Color color = menu_choice_text[i].color;
                    color.a = 1;
                    menu_choice_text[i].color = color;
                }

                for (int i = 0; i < extra_hide_image.Length; i++)
                {
                    Color color = extra_hide_image[i].color;
                    color.a = 1;
                    extra_hide_image[i].color = color;
                }

                for (int i = 0; i < extra_hide_text.Length; i++)
                {
                    Color color = extra_hide_text[i].color;
                    color.a = 1;
                    extra_hide_text[i].color = color;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            choice_number--;
            if (choice_number < 0)
            {
                choice_number = 0;
            }

            CursorShift();
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            choice_number++;
            if (choice_number > menu_choice.Length - 1)
            {
                choice_number = menu_choice.Length - 1;
            }

            CursorShift();
        }
       
    }

    void CursorShift()
    {
        cursor.anchoredPosition = new Vector2(menu_choice_rt[choice_number].anchoredPosition.x - menu_choice_rt[choice_number].sizeDelta.x * menu_choice_rt[choice_number].localScale.x / 2 - 10, menu_choice_rt[choice_number].anchoredPosition.y);
        if (explanation_text != null)
        {
            explanation_text.text = explanation_text_string[choice_number];
        }
    }

    protected void BackToBaseMenu()
    {
        base_mc.activated = true;
        Destroy(this.gameObject);
    }
}

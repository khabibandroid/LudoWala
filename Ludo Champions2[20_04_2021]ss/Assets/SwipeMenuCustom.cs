using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeMenuCustom : MonoBehaviour
{
    public Scrollbar scrollBar;
    bool trans1 = false, trans2 = false, transLeft = false ;
    string[] title, desc;
    public GameObject la, ra;
    float scrollPos = 0.4f;
    int current = 1;
    float[] pos;
    public Text Title, Description;
    public string Title1, Title2, Title3, Title4, desc1, desc2, desc3, desc4;
    public MenuButtonAlphaManagerCustom[] optionBtns;
    // Start is called before the first frame update
    void Start()
    {
        scrollBar.value = 0.4f;
        EventCounter.ResetALLData();
        title = new string[4];
        title[0] = Title1;
        title[1] = Title2;
        title[2] = Title3;
        title[3] = Title4;

        current = 1;
        trans1 = true;
        trans2 = false;

        desc = new string[4];
        desc[0] = desc1;
        desc[1] = desc2;
        desc[2] = desc3;
        desc[3] = desc4;
    }

    // Update is called once per frame
    void Update()
    {

        if (trans1)
        {
            /*//fadeOut, next title
            if(Title.color.a > 0.02f)
            {
                Title.color = new Color(Title.color.r, Title.color.g, Title.color.b, Mathf.Lerp(Title.color.a, 0, 0.1f));
                Description.color = new Color(Description.color.r, Description.color.g, Description.color.b, Title.color.a);
            }
            else*/
            {
                Title.color = new Color(Title.color.r, Title.color.g, Title.color.b, 0);
                Description.color = new Color(Description.color.r, Description.color.g, Description.color.b, 0);
                Title.text = title[current];
                Description.text = desc[current];
                trans1 = false;
                trans2 = true;
            }
        }else if (trans2)
        {
            //fadeIn
            if (Title.color.a < 255)
            {
                Title.color = new Color(Title.color.r, Title.color.g, Title.color.b, Mathf.Lerp(Title.color.a, 1, 0.1f));
                Description.color = new Color(Description.color.r, Description.color.g, Description.color.b, Title.color.a);
            }
            else
            {
                trans2 = false;
            }
        }

        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length-1f);

        for (int i = 0; i < pos.Length; i++) {
            pos[i] = distance * i;
        }

        for (int i = 0; i < pos.Length; i++)
        {
            optionBtns[i].alpha = 1 - (Mathf.Abs(scrollBar.value - pos[i]));
        }

            if (Input.GetMouseButton(0))
        {
            scrollPos = scrollBar.value;
        }
        else
        {
            for(int i = 0; i < pos.Length; i++)
            {
                if(scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
                {
                    if(current != i)
                    {
                        la.SetActive(current > 0);
                        ra.SetActive(current < pos.Length);
                        current = i;
                        trans2 = false;
                        trans1 = true;
                    }
                    scrollBar.value = Mathf.Lerp(scrollBar.value, pos[i], 0.1f);
                }
            }
        }

        for (int i = 0; i < pos.Length; i++)
        {
            if (scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
            {
                optionBtns[i].gameObject.transform.localScale = Vector2.Lerp(optionBtns[i].gameObject.transform.localScale, new Vector2(3f, 3f), 0.1f);
                for (int a = 0; a < pos.Length; a++)
                {
                    optionBtns[a].gameObject.transform.localScale = Vector2.Lerp(optionBtns[a].gameObject.transform.localScale, new Vector2(0.8f, 0.8f), 0.1f);
                }
            }
        }
    }

    public void MoveRight()
    {
        trans1 = true;
        trans2 = false;
        transLeft = false;
        current+= current < pos.Length - 1 ? 1 : 0;
        scrollPos = pos[current];
        if(current >= pos.Length - 1)
        {
            current = pos.Length - 1;
            scrollPos = pos[current];
            ra.SetActive(false);
        }
        la.SetActive(true);
    }

    public void MoveLeft()
    {
        trans1 = true;
        trans2 = false;
        transLeft = true;
        current -= current > 0 ? 1 : 0;
        scrollPos = pos[current];
        if (current <= 0)
        {
            current = 0;
            scrollPos = pos[current];
            la.SetActive(false);
        }
        ra.SetActive(true);
    }
}

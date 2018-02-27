using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;
using UnityEngine.UI;

namespace DreamKeeper
{
    public class UIFadeIn:ViewBase
    {
        private float fadeSpeed = 0.5f;
        private Image img;
        private Color color;
        private bool isFading = true;

        private void Awake()
        {
            base.UIForm_Type = UIFormType.PopUp;
            base.UIForm_ShowMode = UIFormShowMode.ReverseChange;
            base.UIForm_LucencyType = UIFormLucenyType.Lucency;
            img = GetComponent<Image>();
            color=img.color;
        }

        private void Update()
        {
            if (isFading)
            {
                if (color.a < 1)
                {
                    color.a += Time.deltaTime * fadeSpeed;
                    img.color = color;
                }
                else
                {
                    isFading = false;
                }
            }
        }
    }
}
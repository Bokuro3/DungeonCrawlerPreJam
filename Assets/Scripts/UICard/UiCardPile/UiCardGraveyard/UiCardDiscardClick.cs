﻿using UnityEngine;
using UnityEngine.EventSystems;

namespace Tools.UI.Card
{
    /// <summary>
    ///     Dicard/Play cards when the object is clicked.
    /// </summary>
    [RequireComponent(typeof(IMouseInput))]
    public class UiCardDiscardClick : MonoBehaviour
    {
        UiPlayerHandUtils Utils { get; set; }
        IMouseInput Input { get; set; }

        void OnEnable()
        {
            Utils = transform.parent.GetComponentInChildren<UiPlayerHandUtils>();
            Input = GetComponent<IMouseInput>();
            Input.OnPointerClick += PlayRandom;
        }

        bool isPlayer;
        void PlayRandom(PointerEventData obj) 
        {
            if (isPlayer)
            {
                Utils.PlayCard();
            }
        }
    }
}
﻿using UnityEngine.EventSystems;

namespace Tools.UI.Card
{
    /// <summary>
    ///     Battlefield Zone.
    /// </summary>
    public class UiZoneBattleField : UiBaseDropZone
    {
        protected override void OnPointerUp(PointerEventData eventData)
        {
            //AudioManager.Instance.Play(SistemaDeTurnos.Instance.au_JugarCarta);
            CardHand?.PlaySelected();
        } 
    }
}
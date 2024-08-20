using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static event EventHandler<ButtonInteractEventArgs> OnButtonInteractEventHandler;

    public class ButtonInteractEventArgs : EventArgs
    {
        public enum InteractionType { MouseDown, MouseUp };

        public readonly InteractionType interaction;

        public ButtonInteractEventArgs(InteractionType _interaction)
            => interaction = _interaction;
    }

    public void OnPointerDown(PointerEventData eventData)
        => OnButtonInteract(new(ButtonInteractEventArgs.InteractionType.MouseDown));

    public void OnPointerUp(PointerEventData eventData)
        => OnButtonInteract(new(ButtonInteractEventArgs.InteractionType.MouseUp));

    protected virtual void OnButtonInteract(ButtonInteractEventArgs e) 
        => OnButtonInteractEventHandler?.Invoke(this, e);

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Input;
using HoloToolkit.Unity.InputModule;
using UnityEngine.UI;
using HoloToolkit.Unity;

public class HandInfo : Singleton<HandInfo> {
    public Image hand;

    public static bool handPressed = false;
    public static Vector3 handPosition_Screen;
    public static Vector3 handPosition_World;

    protected override void Awake()
    {
        InteractionManager.SourceUpdated += InteractionManager_SourceUpdated;
    }

    protected override void OnDestroy()
    {
        InteractionManager.SourceUpdated -= InteractionManager_SourceUpdated;
    }

    private void InteractionManager_SourceUpdated(InteractionSourceState hand)
    {
        //get hand info
        handPressed = hand.pressed;
        hand.properties.location.TryGetPosition(out handPosition_World);
        handPosition_Screen = Camera.main.WorldToScreenPoint(handPosition_World);
        handPosition_Screen -= new Vector3(Screen.width / 2, Screen.height / 2, 0);

        //show hand
        this.hand.enabled = handPressed;
        this.hand.rectTransform.anchoredPosition = handPosition_Screen;
        handPosition_World = this.hand.rectTransform.position;
    }
}
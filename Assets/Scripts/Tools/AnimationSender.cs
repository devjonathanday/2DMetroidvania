using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSender : MonoBehaviour
{
    [SerializeField] AnimationReceiver receiver = null;

    public void SendToReceiver(float value)
    {
        receiver.UpdateValue(value);
    }
}
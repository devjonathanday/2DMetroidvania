using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationReceiver : MonoBehaviour
{
    [SerializeField] string valueName;
    [ReadOnlyField] public float value = 0;

    public void UpdateValue(float _value)
    {
        value = _value;
        OnValueChanged.Invoke();
    }

    public delegate void AnimationReceiverEvent();

    public AnimationReceiverEvent OnValueChanged;
    public AnimationReceiverEvent OnTrigger;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Effect {
    [SerializeField] float baseValue = 0.0f;
    List<float> valueChanges = new List<float>();

    public float GetValue() {
        float totalEffectValue = baseValue;
        foreach (float effectValue in valueChanges)
        {
            totalEffectValue = totalEffectValue + effectValue;
        }
        return totalEffectValue;
    }

    public void SetValue(float value) {
        baseValue = value;
    }

    public void AddValueChanges(float changes) {
        valueChanges.Add(changes);
    }

    public void RemoveValueChanges(float changes) {
        valueChanges.Remove(changes);
    }
}
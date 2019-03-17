using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VectorValue : ScriptableObject, ISerializationCallbackReceiver {

    [Header("In Game Value")]
    public Vector2 initialValue;

    [Header("Starting Value")]
    public Vector2 defaultValue;

    public void OnAfterDeserialize() {
        initialValue = defaultValue;
    }

    public void OnBeforeSerialize() {
       
    }
}

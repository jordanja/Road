using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelper {
    
    public static float Sigmoid(float value) {
        float k = Mathf.Exp(value);
        return k / (1.0f + k);
    }
}

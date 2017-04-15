using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2Input {
    public string type;
    public int IntParam1;

    public float FloatParam1;
    public float FloatParam2;
    public float FloatParam3;

    public float rotation_speed;
    public int rotate;

    public S2Input() {
        //empty constructor
    }

    public S2Input(string type, float phi, float width, float scale) {
        //circle non vertical path.
        this.type = type;
        FloatParam1 = phi;
        if (scale < 0.1f || scale > Mathf.PI)
            FloatParam2 = 0.1f;
        else
            FloatParam2 = scale;
        FloatParam3 = width;
    }

    public S2Input(string type, int k)
    {
        this.type = type;
        IntParam1 = k;
    }

    public S2Input(string type, int IntParam1, float FloatParam1, float FloatParam2, float FloatParam3,
        float rotation_speed, int rotate) {

        this.type = type;
        this.IntParam1 = IntParam1;
        this.FloatParam1 = FloatParam1;
        this.FloatParam2 = FloatParam2;
        this.FloatParam3 = FloatParam3;
        this.rotation_speed = rotation_speed;
        this.rotate = rotate;
    }

    public void setRotationInfo(int rotate, float speed) {
        this.rotate = rotate;
        rotation_speed = speed;
    }
}

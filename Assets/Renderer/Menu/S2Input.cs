using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2Input {
    public string type;
    public int IntParam1;
    public int IntParam2;
    public int IntParam3;

    public float FloatParam1;
    public float FloatParam2;
    public float FloatParam3;

    public float rotation_speed;
    public int rotate;

    public S2Input(string type, int IntParam1, int IntParam2, int IntParam3, float FloatParam1, float FloatParam2, float FloatParam3,
        float rotation_speed, int rotate) {

        this.type = type;
        this.IntParam1 = IntParam1;
        this.IntParam2 = IntParam2;
        this.IntParam3 = IntParam3;
        this.FloatParam1 = FloatParam1;
        this.FloatParam2 = FloatParam2;
        this.FloatParam3 = FloatParam3;
        this.rotation_speed = rotation_speed;
        this.rotate = rotate;
    }
}

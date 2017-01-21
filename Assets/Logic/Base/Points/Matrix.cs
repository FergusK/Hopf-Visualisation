using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix {
    public float[,] m;             // Should be 4x4

    public static void main()
    {

    }

    public Matrix()
    {
        m = new float[4,4];
        setIdentity();
    }

    public void setIdentity()
    {   /* Resets the matrix to the Identity matrix */
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
                if (i == j)
                    m[i,i] = 1.0f;

                else
                    m[i,j] = m[j,i] = 0.0f;
    }

    public Matrix multiply(Matrix a)
    {
        Matrix result = new Matrix();

        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
            {
                result.m[i,j] = 0.0f;
                for (int k = 0; k < 4; k++)
                    result.m[i,j] += (m[i,k] * a.m[k,j]);
            }

        return result;
    }

    public void setTranslation(float tx, float ty, float tz)
    {
        this.setIdentity();
        m[0,3] = tx;
        m[1,3] = ty;
        m[2,3] = tz;
    }

    public void setRotationX(float a)
    {
        float cs, sn;

        this.setIdentity();
        cs = Mathf.Cos(a);
        sn = Mathf.Sin(a);
        m[1,1] = cs;
        m[2,1] = sn;
        m[1,2] = -sn;
        m[2,2] = cs;
    }

    public void setRotationY(float a)
    {
        float cs, sn;

        this.setIdentity();
        cs = Mathf.Cos(a);
        sn = Mathf.Sin(a);
        m[0,0] = cs;
        m[2,0] = -sn;
        m[0,2] = sn;
        m[2,2] = cs;
    }

    public void setRotationZ(float a)
    {
        float cs, sn;

        this.setIdentity();
        cs = Mathf.Cos(a);
        sn = Mathf.Sin(a);
        m[0,0] = cs;
        m[0,1] = -sn;
        m[1,0] = sn;
        m[1,1] = cs;
    }

    public Matrix inverse()
    {
        float det;
        int i, j;
        Matrix invM = new Matrix();

        det = m[0,0] * (m[1,1] * m[2,2] - m[2,1] * m[1,2]) +
        m[0,1] * (m[1,2] * m[2,0] - m[2,2] * m[1,0]) +
        m[0,2] * (m[1,0] * m[2,1] - m[2,0] * m[1,1]);

        if (det == 0.0)
        {
            Debug.Log("singular transformation matrix\n");
            return null;
        }

        invM.m[0,0] = m[1,1] * m[2,2] - m[1,2] * m[2,1];
        invM.m[1,0] = m[1,2] * m[2,0] - m[1,0] * m[2,2];
        invM.m[2,0] = m[1,0] * m[2,1] - m[1,1] * m[2,0];

        invM.m[0,1] = m[0,2] * m[2,1] - m[0,1] * m[2,2];
        invM.m[1,1] = m[0,0] * m[2,2] - m[0,2] * m[2,0];
        invM.m[2,1] = m[0,1] * m[2,0] - m[0,0] * m[2,1];

        invM.m[0,2] = m[0,1] * m[1,2] - m[0,2] * m[1,1];
        invM.m[1,2] = m[0,2] * m[1,0] - m[0,0] * m[1,2];
        invM.m[2,2] = m[0,0] * m[1,1] - m[0,1] * m[1,0];

        for (i = 0; i < 3; i++)
            for (j = 0; j < 3; j++)
                invM.m[i,j] /= det;

        for (j = 0; j < 3; j++)
        {
            invM.m[j,3] = 0.0f;
            for (i = 0; i < 3; i++) invM.m[j,3] -= (m[i,3] * invM.m[j,i]);
        }
        invM.m[3,3] = 1.0f;
        return invM;
    }
}

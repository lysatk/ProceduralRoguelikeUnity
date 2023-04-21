using System;
using UnityEngine;

public class IKController : MonoBehaviour
{
    float legspeed = 10f;
    float gait = 0f;
    Vector2 movingDirection = new Vector2(0, 0);
    Vector2 facingDirection = new Vector2(0, 0);
    float mo_co = 1f;
    float x = 0;
    float y = 0;


    float pointDirection(float x1, float x2, float y1, float y2)
    {
        Vector2 vectorTemp1 = new Vector2(x1, y1);
        Vector2 vectorTemp2 = new Vector2(x2, y2);

        //calc Direction here

        return 0;
    }
    float pointDistance(float x1, float x2, float y1, float y2)
    {
        Vector2 vectorTemp1 = new Vector2(x1, y1);
        Vector2 vectorTemp2 = new Vector2(x2, y2);

        Vector2.Distance(vectorTemp1, vectorTemp2);
        return Vector2.Distance(vectorTemp1, vectorTemp2); 
    }


    float lengthDirX(float len, Vector2 dir)// lenght, direction
    {
        dir = dir.normalized * (float)len;
        return dir.x;
    }

    float lengthDirY(float len, Vector2 dir)// lenght, direction
    {
        dir = dir.normalized * (float)len;
        return dir.y;
    }

    void createIK(float lengthCalf, float lengthThigh)
    {
        //

    }

    void drawLeg(float lengthCalf, float lengthThigh, float hipX, float hipY)
    {
        //**********A*********
        //*********/|*********
        //*******b/ |*********
        //*******/  |*********
        //******/   |*********
        //*****C\   |c********
        //*******\  |*********
        //*******a\ |*********
        //*********\|*********
        //**********B*********
        //********************


        //Rule of Cosines will be here soon 
        float cLength, bLength, aLength, alpha, beta, ax, ay, ix, iy, knee_mod, Ax, Ay, Bx, By, Cx, Cy, C2x, C2y;
        //cLenght is the distance from hip to foot
        Ax = hipX;
        Ay = hipY;
        bLength = lengthThigh;
        aLength = lengthCalf;

        //mo_co -> motion counter used for the oscillating animation of foot // move somewhere else

        knee_mod = Ax - (Ax + lengthDirX(1, facingDirection)); //Direction the knee will bend for the "3D" knee

        if (legspeed > 0)
            gait = (float)Math.Pow(legspeed * 2, 0.4); //how big the step is (you may want to tweak this!)
                                                       //Note that the stride is not related to movement speed linearly, but instead uses a exponent of 0.4.

        ///////////////////////////////////////////////////////////////////
        //
        //Sin-> Horizontal movement, Cos-> Veritcal movement of the "foot"
        //
        //////////////////////////////////////////////////////////////////

        ax = x + lengthDirX(Convert.ToSingle((gait) * ((aLength + bLength) / 4) * (Math.Sin(Mathf.Deg2Rad * mo_co)) - ((legspeed * 1.25)) * 2), movingDirection);
        //x=location x

        ay = (float)(y + ((gait) * ((aLength + bLength) / 6) * (-Math.Cos(Mathf.Deg2Rad * (mo_co)) - 1)));
        //y=location y

        ///IK CALCULATION///
        alpha = pointDirection(Ax, Ay, ax, ay); //angle between hip and foot

        cLength = Math.Min(pointDistance(Ax, Ay, ax, ay), (aLength + bLength)); //distance between hip and foot, restricted to total limb length





    }


    void Start()
    {
        createIK(10, 8);//Crete the IK on Start
    }

}

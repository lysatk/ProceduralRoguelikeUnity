using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKController : MonoBehaviour
{

    void createIK(float lengthCalf, float lengthThigh)
    {
        //

    }

    void drawLeg(float lengthCalf, float lengthThigh, float hipX, float hipY)
    {
        //Rule of Cosines will be here soon 
        float _c, _b, _a, alpha, beta, ax, ay, ix, iy, knee_mod, Ax, Ay, Bx, By, Cx, Cy, C2x, C2y, mo_co;



    }


    void Start()
    {
        createIK(10, 8);//Crete the IK on begin Play
    }

}

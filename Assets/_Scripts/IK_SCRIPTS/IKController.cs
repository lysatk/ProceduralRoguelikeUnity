using System;
using UnityEditor;
using UnityEngine;

public class IKController : MonoBehaviour
{
    public float legspeed = 0f;
    float gait = 0f;
    public float movingDirection = 0;
    public float facingDirection = 180;
    float x = 0;
    float y = 0;
    float mo_co = 1f;// motion counter used for the oscillating animation of foot // move somewhere else
    float hipx, hipy, motion_counter, thigh, calf, facingdirection, movingdirection;

    Rigidbody2D rb;
    public Texture2D tex;
    private Sprite mySprite;
    private SpriteRenderer sr;
    GameObject player;

    //////////////////
    ///PRAWDOPODOBNIE PRZYCZYN¥ JEST U¯YWANIE GLOBALNEGO TRANSFORM SPRÓBUJ PIEWR JAKOŒ LOKAL TO ZROBIÆ EJ
    ///
    ///
    ///////////////
    float pointDirection(float x1, float y1, float x2, float y2)
    {
        float xDiff = x2 - x1;
        float yDiff = y2 - y1;
        return (float)((float)Mathf.Rad2Deg * Math.Atan2(yDiff, xDiff));
    }

    float pointDistance(float x1, float y1, float x2, float y2)
    {
        return (float)Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));// Euclidean distance formula 
    }


    float lengthDirX(float len, float dir)// lenght, direction
    {
        return (float)(len * Math.Cos(dir));
    }

    float lengthDirY(float len, float dir)// lenght, direction
    {
        return (float)(len * Math.Sin(dir));
    }

    void createIK(float lengthCalf, float lengthThigh)
    {


        motion_counter = 0; //counter for animating the leg movement

        thigh = lengthCalf; //thigh length

        calf = lengthThigh; //calf length

        facingdirection = 1.3f; //direction for knee (but also character in general)

        legspeed = 1; //speed of movement

        movingdirection = 1.3f; //direction of movement

        gait = 0.1f; //size of step

    }

    void drawLeg(float lengthCalf, float lengthThigh, float hipX, float hipY, float off)
    {
        //**********A*********//
        //*********/|*********//
        //*******b/ |*********//
        //*******/  |*********//
        //******/   |*********//
        //*****C\   |c********//
        //*******\  |*********//
        //*******a\ |*********//
        //*********\|*********//
        //**********B*********//
        //********************//


        //Rule of Cosines will be here soon 
        float cLength, bLength, aLength, ax, ay, ix, iy, kneeMod, Ax, Ay, Bx, By, Cx, Cy, C2x, C2y;
        //cLenght is the distance from hip to foot

        Ax = hipX;
        Ay = hipY;
        bLength = lengthThigh;
        aLength = lengthCalf;


        ix = 0;//temporary 
        iy = 0;//temporary
        float alpha, beta;


        kneeMod = Ax - (Ax + lengthDirX(0.01f, facingDirection)); //Direction the knee will bend for the "3D" knee

        if (legspeed > 0)
            gait = (float)Math.Pow(legspeed / 2, 0.4); //how big the step is (may need tweaking)
                                                       //Stride is not related to movement speed linearly, it uses a exponent of 0.4.


        //Sin-> Horizontal movement, Cos-> Veritcal movement of the "foot"


        ax = x + lengthDirX(Convert.ToSingle((gait) * ((aLength + bLength) / 4) * (Mathf.Rad2Deg * Math.Sin(mo_co)) - ((legspeed * 1.25)) * 2), movingDirection);
        //x=location x

        ay = (float)(y + ((gait) * ((aLength + bLength) / 6) * (-Math.Cos((mo_co)) - 1f)));
        //y=location y

        ///IK CALCULATION///
        alpha = pointDirection(Ax, Ay, ax, ay); //angle between hip and foot

        cLength = Math.Min(pointDistance(Ax, Ay, ax, ay), (aLength + bLength)); //distance between hip and foot, restricted to total limb length

        Bx = Ax + lengthDirX(cLength, alpha); //foot x position

        By = Ay + lengthDirY(cLength, alpha); //foot y position
        beta = (float)(Mathf.Rad2Deg * (Math.Acos(Math.Min(1, Math.Max(-1, (Math.Pow(bLength, 2) + Math.Pow(cLength, 2) - Math.Pow(aLength, 2)) / (2 * (bLength) * cLength))))));

        // beta = pointDirection(betaDeg); //"Law of Cosines" to get angle of thigh, _c
        Cx = Ax + lengthDirX(bLength, alpha - beta);//knee x position
        Cy = Ay + lengthDirY(bLength, alpha - beta);//knee y position

        ix = Ax + lengthDirX((float)(bLength * Math.Cos(beta)), pointDirection(Ax, Ay, Bx, By)); //find the intersect point on the hip->foot line which divides the triangle into 2 right-angle triangles

        iy = Ay + lengthDirY((float)(bLength * Math.Cos(beta)), pointDirection(Ax, Ay, Bx, By)); //this is to foreshorten the knee when facing toward the camera.


        C2x = ix + lengthDirX(pointDistance(ix, iy, Cx, Cy) * kneeMod, pointDirection(ix, iy, Cx, Cy));//"3D" knee x position
        C2y = iy + lengthDirY(pointDistance(ix, iy, Cx, Cy) * kneeMod, pointDirection(ix, iy, Cx, Cy));//"3D" knee y position


        ///////////////////////////////////////////////////////////////////////
        //////////////////DRAWING THE SPRITES WILL GO HERE/////////////////////
        ///////////////////////////////////////////////////////////////////////

        ///draw_line_width(Ax+lengthdir_x(argument5,facingdirection+90),Ay,C2x+lengthdir_x(argument5,facingdirection+90),C2y,3)
        ///draw_line_width(C2x+lengthdir_x(argument5,facingdirection+90),C2y,Bx+lengthdir_x(argument5,facingdirection+90),By,
        Debug.DrawLine(new Vector2(Ax + lengthDirX(off, facingDirection + 90), Ay), new Vector2(C2x + lengthDirX(off, facingDirection + 90), C2y), Color.magenta);
        Debug.DrawLine(new Vector2(C2x + lengthDirX(off, facingDirection + 90), C2y), new Vector2(Bx + lengthDirX(off, facingDirection + 90), By), Color.blue);



        //////////////move to another funtion perhaps???///////////////////////
        ///
        mo_co += (float)(Math.Pow(legspeed, 0.4));//this is the counting variable for the animation
        mo_co = mo_co % 360;  //limit the variable between 0 and 360

        Debug.Log("Ax:" + Ax + " Ay:" + Ay + " ix" + ix + " iy:" + iy + " Bx" + Bx + " By" + By);
        sr.sprite = mySprite;
    }


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // createIK(0.10f, 0.8f);//Crete the IK on Start

        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
        rb = player.GetComponent<Rigidbody2D>();

    }

    private void FixedUpdate()
    {
        facingDirection = pointDirection(x, Input.mousePosition.x, Input.mousePosition.y, y);
        movingDirection = pointDirection(x, Input.mousePosition.x, Input.mousePosition.y, y);
        x = player.transform.localPosition.x;////NIE WIEM CZY TO MA SENS
        y = player.transform.localPosition.y;
        y -= .08f;
        Debug.Log("x: " + x + "/ y: " + y);



        drawLeg(0.1f, 0.12f, x, y, 0.05f); drawLeg(0.1f, 0.12f, x, y, -0.05f);


        //  transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

}

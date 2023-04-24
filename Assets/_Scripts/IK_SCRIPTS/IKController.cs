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
    float motionCounter = 1f;// motion counter used for the oscillating animation of foot // move somewhere else
    float hipx, hipy, motion_counter, thigh, calf, facingdirection, movingdirection;


    Camera mainCam;


    Rigidbody2D rb;
    public Texture2D tex;
    private Sprite mySprite;
    private SpriteRenderer sr;
    GameObject player;

    Vector2 mousePos;

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

    //void createIK(float lengthCalf, float lengthThigh) //will be used later for proper initialization of variables needed for IK calculations
    //{


    //    motion_counter = 0; //counter for animating the leg movement

    //    thigh = lengthCalf; //thigh length

    //    calf = lengthThigh; //calf length

    //    facingdirection = 1.3f; //direction for knee (but also character in general)

    //    legspeed = 1; //speed of movement

    //    movingdirection = 1.3f; //direction of movement

    //    gait = 0.1f; //size of step

    //}

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
        float alpha, beta;


        kneeMod = Ax - (Ax + lengthDirX(0.01f, facingDirection)); //Direction the knee will bend for the "3D" knee

        if (legspeed > 0)
            gait = (float)Math.Pow(legspeed * 0.25f, 0.4f); //how big the step is (may need tweaking)
                                                            //Stride is not related to movement speed linearly, it uses a exponent of 0.4.


        //Sin-> Horizontal movement, Cos-> Veritcal movement of the "foot"
        ax = x + lengthDirX(Convert.ToSingle((gait) * ((aLength + bLength) / 4) * (Mathf.Rad2Deg * -Math.Sin(motionCounter)) - ((legspeed * 1.25)) * 2), movingDirection);
        //x=location x

        ay = (float)(y + ((gait) * ((aLength + bLength) / 6) * (Mathf.Rad2Deg * Math.Cos((motionCounter)) - 1f)));
        //y=location y

        ///IK CALCULATION///
        alpha = pointDirection(Ax, Ay, ax, ay); //angle between hip and foo
        cLength = Math.Min(pointDistance(Ax, Ay, ax, ay), (aLength + bLength)); //distance between hip and foot, restricted to total limb length

        Bx = Ax + lengthDirX(cLength, alpha); //foot x position
        By = Ay + lengthDirY(cLength, alpha); //foot y position

        beta = (float)(Mathf.Rad2Deg * (Math.Acos(Math.Min(1, Math.Max(-1, (Math.Pow(bLength, 2) + Math.Pow(cLength, 2) - Math.Pow(aLength, 2)) / (2 * (bLength) * cLength))))));

        Cx = Ax + lengthDirX(bLength, alpha - beta);//knee x position
        Cy = Ay + lengthDirY(bLength, alpha - beta);//knee y position

        ix = Ax + lengthDirX((float)(bLength * Math.Cos(beta)), pointDirection(Ax, Ay, Bx, By)); //find the intersect point on the hip->foot line which divides the triangle into 2 right-angle triangles

        iy = Ay + lengthDirY((float)(bLength * Math.Cos(beta)), pointDirection(Ax, Ay, Bx, By)); //this is to foreshorten the knee when facing toward the camera.


        C2x = ix + lengthDirX(pointDistance(ix, iy, Cx, Cy) * kneeMod, pointDirection(ix, iy, Cx, Cy));//"3D" knee x position
        C2y = iy + lengthDirY(pointDistance(ix, iy, Cx, Cy) * kneeMod, pointDirection(ix, iy, Cx, Cy));//"3D" knee y position


        /////////////////////////////////////////////////////////////////////
        //////////////////DRAWING THE SPRITES (might move to ext)////////////
        /////////////////////////////////////////////////////////////////////

        ///draw_line_width(Ax+lengthdir_x(argument5,facingdirection+90),Ay,C2x+lengthdir_x(argument5,facingdirection+90),C2y,3)
        ///draw_line_width(C2x+lengthdir_x(argument5,facingdirection+90),C2y,Bx+lengthdir_x(argument5,facingdirection+90),By,
        Debug.DrawLine(new Vector2(Ax + lengthDirX(off, facingDirection ), Ay), new Vector2(C2x + lengthDirX(off, facingDirection ), C2y), Color.magenta);
        Debug.DrawLine(new Vector2(C2x + lengthDirX(off, facingDirection ), C2y), new Vector2(Bx + lengthDirX(off, facingDirection ), By), Color.blue);





        Debug.Log("Ax:" + Ax + " Ay:" + Ay + " ix" + ix + " iy:" + iy + " Bx" + Bx + " By" + By + "Cx:" + Cx + " Cy:" + Cy + " C2x" + C2x + " C2y:" + C2y);
        //    sr.sprite = mySprite;//drawing sprite not used now
    }

    void motionCounterStep()
    {
        motionCounter += (float)(Math.Pow(legspeed, 0.4));//this is the counting variable for the animation
        motionCounter = motionCounter % 180;  //limit the variable between 0 and 360
    }
    void Start()
    {
        mainCam = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");
        // createIK(0.10f, 0.8f);//Crete the IK on Start

        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.color = new Color(0.1f, 0.9f, 0.9f, 1.0f);
        rb = player.GetComponent<Rigidbody2D>();


    }

    private void FixedUpdate()
    {
        x = player.transform.localPosition.x;
        y = player.transform.localPosition.y;
        Debug.Log("x: " + x + "/ y: " + y);


        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        facingDirection = pointDirection(x, y, mousePos.x, mousePos.y);
        movingDirection = pointDirection(x, y, mousePos.x, mousePos.y);
        y -= .08f;




        drawLeg(0.06f, 0.08f, x, y, 0.05f); drawLeg(0.06f, 0.08f, x, y, -0.05f);

        if (legspeed > 0.001f)
        {
            motionCounterStep();
        }

        //  transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }



}

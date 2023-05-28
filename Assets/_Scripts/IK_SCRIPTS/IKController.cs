using System;
using UnityEditor;
using UnityEngine;

public class IKController : MonoBehaviour
{
    public float legspeed = 0f;
    float gait = 0f;
    public float movingDirection = 0;
    public float facingDirection = 0;
    float x = 0;
    float y = 0;
    public float motionCounter = 0f;// motion counter used for the oscillating animation of foot // move somewhere else
    float hipx, hipy, motion_counter, thigh, calf;

    Camera mainCam;


    Rigidbody2D rb;
    public Texture2D tex;
    private Sprite mySprite;
    private SpriteRenderer sr;
    GameObject player;

    Vector2 mousePos;



    public static float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
    float pointDirection(float x1, float y1, float x2, float y2)

    {
        float xDiff = x2 - x1;
        float yDiff = y2 - y1;
        return (float)Math.Atan2(yDiff, xDiff);

        // return Remap((float)((float)Mathf.Rad2Deg*Math.Atan2(yDiff, xDiff)), -180f, 0f, 180f, 360f);

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

    void drawLeg(float lengthCalf, float lengthThigh, float hipX, float hipY, float off, float mc)
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
        float motionCount = Mathf.Rad2Deg * mc;
        Ax = hipX;
        Ay = hipY;
        aLength = lengthThigh;
        bLength = lengthCalf;
        float alpha, beta;


        kneeMod = Ax - (Ax - lengthDirX(1f, facingDirection)); //Direction the knee will bend for the "3D" knee

        if (legspeed > 0)
            gait = 2 * (float)Math.Pow(legspeed * 3, 4f); //how big the step is (may need tweaking)
                                                            //Stride is not related to movement speed linearly, it uses a exponent of 0.4.


        //Sin-> Horizontal movement, Cos-> Veritcal movement of the "foot"
        ax = x + lengthDirX(Convert.ToSingle((gait) * ((aLength + bLength) / 4) * (-Math.Sin(motionCount)) - ((legspeed * 0.25f))), movingDirection);
        //x=location x

        ay = (float)(y + ((gait) * ((aLength + bLength) / 6) * (Math.Cos(motionCount) - 1f)));
        //y=location y

        ///IK CALCULATION///
        alpha = pointDirection(Ax, Ay, ax, ay); //angle between hip and foot
        cLength = Math.Min(pointDistance(Ax, Ay, ax, ay), (aLength + bLength)); //distance between hip and foot, restricted to total limb length

        Bx = Ax + lengthDirX(cLength, alpha); //foot x position
        By = Ay + lengthDirY(cLength, alpha); //foot y position

        beta = (float)((Math.Acos(Math.Min(1, Math.Max(-1, ((bLength * bLength) + (cLength * cLength) - aLength * aLength)) / (2 * (bLength) * cLength)))));

        Cx = Ax + lengthDirX(bLength, alpha - beta);//knee x position
        Cy = Ay + lengthDirY(bLength, alpha - beta);//knee y position

        ix = Ax + lengthDirX((float)(bLength * Math.Cos(beta)), pointDirection(Ax, Ay, Bx, By)); //find the intersect point on the hip->foot line which divides the triangle into 2 right-angle triangles
        iy = Ay + lengthDirY((float)(bLength * Math.Cos(beta)), pointDirection(Ax, Ay, Bx, By)); //this is to foreshorten the knee when facing toward the camera.

        C2x = ix + lengthDirX(pointDistance(ix, iy, Cx, Cy) * kneeMod, pointDirection(ix, iy, Cx, Cy));//"3D" knee x position
        C2y = iy + lengthDirY(pointDistance(ix, iy, Cx, Cy) * kneeMod, pointDirection(ix, iy, Cx, Cy));//"3D" knee y position

        /////////////////////////////////////////////////////////////////////
        //////////////////DRAWING THE SPRITES (might move to ext)////////////
        /////////////////////////////////////////////////////////////////////

        float temp = facingDirection + Mathf.PI * 2;
        Debug.DrawLine(new Vector3(Ax + 0.3f, Ay, 0), new Vector3(Ax - 0.3f, Ay, 0));


        Debug.DrawLine(new Vector2(Ax + lengthDirX(off, temp), Ay), new Vector2(C2x + lengthDirX(off, temp), C2y), Color.magenta);
        Debug.DrawLine(new Vector2(C2x + lengthDirX(off, temp), C2y), new Vector2(Bx + lengthDirX(off, temp), By), Color.blue);

        Debug.Log(facingDirection);
        Debug.Log("Ax:" + Ax + " Ay:" + Ay + " ix" + ix + " iy:" + iy + " Bx" + Bx + " By" + By + "Cx:" + Cx + " Cy:" + Cy + " C2x" + C2x + " C2y:" + C2y);
        //sr.sprite = mySprite;//drawing sprite not used now
    }

    void motionCounterStep()
    {
        motionCounter += (float)(5.2f*(Math.Pow(legspeed, 4f)));//this is the counting variable for the animation
        motionCounter = motionCounter % Mathf.PI;  //limit the variable between 0 and 180
    }
    void Start()
    {
        mainCam = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");
        // createIK(0.10f, 0.8f);//Crete the IK on Start
        sr = gameObject.GetComponent<SpriteRenderer>();
        //  sr.color = new Color(0.1f, 0.9f, 0.9f, 1.0f);
        rb = player.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        x = player.transform.localPosition.x;
        y = player.transform.localPosition.y;

        Debug.Log("x: " + x + "/ y: " + y);
        y -= .08f;

        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        facingDirection = pointDirection(mousePos.x, mousePos.y, x, y);

        if (rb.velocity.magnitude > 0.01f)
        {
            legspeed = rb.velocity.magnitude/2;
            motionCounterStep();
            movingDirection = (float)Math.Atan2(rb.velocity.y, rb.velocity.x);

        }
        else
        {
            legspeed = (float)Math.Min(rb.velocity.magnitude, 2);
            movingDirection = facingDirection;
        }
        drawLeg(0.8f, 0.8f, x, y, 0.3f, motionCounter); drawLeg(0.8f, 0.8f, x, y, -0.3f, (float)(motionCounter + Mathf.PI / 2));


    }



}

using System;

[Serializable]
public enum ExampleEnemyType
{
    //Normal Enemies//Grunts//Goons
    //1-1->1-3
    Melee = 0,
    Ranged = 1,
    Boss = 2,
    //2-1->2-3
    SpiderSmall = 3,
    SpiderBig = 4,
    WebTurret = 5,
    //3-1->3-3
    Cultist = 6,
    Cultclop = 7,
    Kamikaze = 8,

    //Bosses
    OgreKing =30,//1-3
    SpiderWitch=31,//2-3
    CrystalDeamon=32
}
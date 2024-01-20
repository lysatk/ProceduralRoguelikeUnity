using System;

[Serializable]
public enum GameState
{
    Hub = 0,
    ChangeLevel = 1,
    Starting = 2,
    Lose = 3,
    PostLevel=4,
    BossReached=5,
    Restarting=6,
    Win=7,
    Null = 99
}
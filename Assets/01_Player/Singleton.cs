using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct UserData
{
    public string UserID;
    public string UserPassword;
    public int UserLevel;
    //public int KillCount;
}

public class Singleton
{
    private static Singleton GameSingletone = null;

    UserData User;
    public UserData GetUserData() { return User; }
    public int KillCount;
    public int NowLevel;
    public static Singleton Instance
    {
        get
        {
            if(GameSingletone == null)
            {
                GameSingletone = new Singleton();

                if(GameSingletone == null)
                {
                    Debug.Log("No Singletone Obj");
                }
            }
            return GameSingletone;
        }
    }

    public UserData SetUserData(string id, string password, int level)
    {
        UserData TempUser;
        TempUser.UserID = id;
        TempUser.UserPassword = password;
        TempUser.UserLevel = level;
        //TempUser.KillCount = 0;
        return User = TempUser;
    }

    public int SetKillCount() { return KillCount++; }
    public int SetNowLevel(int value) { return NowLevel = value; }
}

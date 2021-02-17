using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;


//rankpath = "Assets/resources/"
public class RankDB {
    public static void RankWriter(string rankpath,int[] rankArray)
    {
        StreamWriter sw = new StreamWriter(rankpath);
        for (int i = 0; i < 5; i++) {
            sw.WriteLine(rankArray[i]);
        }
        sw.Flush();
        sw.Close();
    }
    public static int[] RankReader(string rankpath)
    {
        int[] rankArray = new int[5];
        string s;
        StreamReader sr = new StreamReader(rankpath);
        
        for(int i= 0; i< 5; i++)
        {
            s = sr.ReadLine();
            rankArray[i] = int.Parse(s);
        }
        sr.Close();
        return rankArray;
        
    }
}

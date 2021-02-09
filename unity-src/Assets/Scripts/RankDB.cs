using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

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
        StreamReader sr = new StreamReader(rankpath);

        for(int i = 0; i< 5; i++)
        {
            rankArray[i] = sr.ReadLine();
        }
        sr.Close();
        return rankArray;
    }
}

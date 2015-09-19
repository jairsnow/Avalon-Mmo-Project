using System;
using System.Collections.Generic;
using UnityEngine;

static class globalqueque {

    public static List<singleQueque> queque;

    static globalqueque()
    {
        queque = new List<singleQueque>();
    }

    public static void resolve()
    {
        foreach (singleQueque workingQueque in queque)
        {
            if (workingQueque.status == true)
            {
                workingQueque.feedback();
            }
        }
    }
    
}

public class singleQueque
{
    public int startMessageTimestamp;
    public string action;
    public bool status;
    public Action feedback;
}

public class genericRequest {
    public string action;
}
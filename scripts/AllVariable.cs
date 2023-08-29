using System;
using Godot;
public partial class AllVariable
{
    private static int nitrovalue;
    private static int speedvalue = 400;
    private static int timevalue;
    private static int maplengthvalue;
    private static int hpvalue = 100;
    private static bool pauseonvalue = false;

    public int nitrous 
    {
        get { return nitrovalue; }
        set { nitrovalue = value; }
    }
    public int speed 
    {
        get { return speedvalue; }
        set { 
            speedvalue = value; 
            GD.Print("CIGO ALLITVA");    
        }
    }
    public int hp
    {
        get { return hpvalue; }
        set { hpvalue = value; }
    }
    public int time
    {
        get { return timevalue; }
        set { timevalue = value; }
    }
    public int maplength
    {
        get { return maplengthvalue; }
        set { maplengthvalue = value; }
    }
    public bool pauseon
    {
        get { return pauseonvalue; }
        set { pauseonvalue = value; }
    }
}
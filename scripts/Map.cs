using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Map : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export] private int mapLength;
    [Export] public int mapHeight;
    public TileMap tileMap;
    public TileSet tileSet;
    public Node2D bus_StopN;
    public Sprite red_circle;
    public Area2D bus_stop;
    public Node2D car_node;
    public KinematicBody2D car;
    public int tile_width = 64;
    public int tile_height = 64;
    public float multiplier = 8.28125f;
    [Export]public PackedScene spikestrip;
    [Export]public PackedScene  nitrous;
    [Export]public PackedScene sensor;
    [Export]public int spike_chance = 5;
    [Export] public int nitrous_chance = 95;
    public Vector2 position;
	public Area2D finish;
	public Sprite circle;
    public AllVariable allVariable;
    public enum dirtForm
    {
        oneBlock,
        oneUp,
        oneDown,
    }
    public int lastDirtIndex;
    public dirtForm lastDirtForm;

    public int lastBlock;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

        if (mapHeight < 3)
        {
            mapHeight = 3;
        }

        if (mapLength < 5)
        {
            mapLength = 5;
        }
        allVariable = new AllVariable();        
        bus_StopN = GetNode("/root/Game/Bus_stop") as Node2D;
        bus_stop = bus_StopN.GetNode("Area2D") as Area2D;
        car_node = GetNode("/root/Game/Car") as Node2D;
        car = car_node.GetNode("KinematicBody2D") as KinematicBody2D;
        //GD.Print($"BUS_STOP1{bus_stop.Position.x}");
        //TODO Mi a gyászért ír totális faszságot a getpos egyik helyen 186 a másikon már 286 és nem lehet állítani a poziciojat
        //TODO Ahol a dirt-t rakjuk le ott egy 1/(előre beállított eséllyel) lerak egy tárgyat(nitró,coin), egybe nem lehet mindkettő
        red_circle = bus_StopN.GetNode("Sprite") as Sprite;
        
        tileMap = GetNode("TileMap") as TileMap;
        tileSet = tileMap.TileSet;
        int grass = (int)tileSet.GetTilesIds()[1];
        int dirt = (int)tileSet.GetTilesIds()[0];
        Random rnd = new Random();
        for (int i = 0; i < mapLength; i++)
        {
            if (i == 0){
                for (int a = 0; a < mapHeight; a++)
                {
                    if (a != 2){
                        tileMap.SetCell(i,a,grass);

                    }else{
                        tileMap.SetCell(i,a,dirt);
                        Vector2 pos = new Vector2((i*tile_width),(a*tile_height));
                        Node2D sensorChild = (Node2D)sensor.Instance();
                        sensorChild.Position = pos;
                        sensorChild.Set("type","dirt");
                        AddChild(sensorChild);
                    }
                }

                lastDirtForm = dirtForm.oneBlock;
            }else
            {
                int column = i - 1;
                lastDirtIndex = getTileBefore(column);
                /*
                for (int a = 0; a < 3; a++)
                {
                    if (a == lastDirtIndex){
                        tileMap.SetCell(i,a,dirt);    
                    }else{
                        tileMap.SetCell(i,a,grass);
                    }
                    
                }
                */
                drawDirt(i);
            }
        }
        allVariable.maplength = mapLength;
        //GD.Print(allVariable.maplength);
        //GD.Print(mapLength);
        position = new Vector2(-((allVariable.maplength * 8.28125f) *64), 0);
		finish = GetNode("/root/Game/Bus_stop/Area2D") as Area2D;
		circle = GetNode("/root/Game/Bus_stop/Sprite") as Sprite;
		finish.Position = position;
		circle.Position = position;
    }


    public int getTileBefore(int oszlop){
        int res = 0;
        switch (lastDirtForm)
        {
            default:
                break;
            case dirtForm.oneBlock:
                for (int i = 0; i < mapHeight; i++)
                {
                    if (tileMap.GetCell(oszlop, i) == 0)
                    {
                        res = i;
                        break;
                    }
                }
                break;
            case dirtForm.oneUp:
                for (int i = 0; i < mapHeight; i++)
                {
                    if (tileMap.GetCell(oszlop, i) == 0)
                    {
                        res = i;
                        break;
                    }
                }
                break;
            case dirtForm.oneDown:
                for (int i = 0; i < mapHeight; i++)
                {
                    if (tileMap.GetCell(oszlop, i) == 0)
                    {
                        res = i;
                    }
                }
                break;
        }
        //GD.Print($"{oszlop+1}. oszlopban a {res}. index a dirt.");
        return res;
    }

    public void drawDirt(int oszlop)
    {
        Random randomObject = new Random();
        dirtForm randomForm = dirtForm.oneBlock;
        List<Vector2> positions = new List<Vector2>();
        switch (lastDirtIndex)
        {
            default:
                switch (lastDirtForm)
                {
                    case dirtForm.oneBlock:
                        int randomNum99 = randomObject.Next(0, 3);
                        switch (randomNum99)
                        {
                            default:
                                break;
                            case 0:
                                //GD.Print("1block" + lastDirtIndex);
                                randomForm = dirtForm.oneBlock;
                                positions.Add(new Vector2(oszlop,lastDirtIndex));
                                break;
                            case 1:
                                randomForm = dirtForm.oneDown;
                                //positions[0] = new Vector2(-1,-1);
                                positions.Add(new Vector2(oszlop,lastDirtIndex));
                                positions.Add(new Vector2(oszlop,lastDirtIndex+1));
                                break;
                            case 2:
                                randomForm = dirtForm.oneUp;
                                positions.Add(new Vector2(oszlop,lastDirtIndex-1));
                                positions.Add(new Vector2(oszlop,lastDirtIndex));
                                //positions[2] = new Vector2(-1,-1);

                                break;
                        }
                        break;
                    case dirtForm.oneDown:
                        int randomNum98 = randomObject.Next(0, 2);
                        switch (randomNum98)
                        {
                            default:
                                break;
                            case 0:
                                //int("1block" + lastDirtIndex);
                                randomForm = dirtForm.oneBlock;
                                positions.Add(new Vector2(oszlop,lastDirtIndex));
                                break;
                            case 1:
                                randomForm = dirtForm.oneDown;
                                positions.Add(new Vector2(oszlop,lastDirtIndex));
                                positions.Add(new Vector2(oszlop,lastDirtIndex+1));
                                break;
                        }
                        break;
                    case dirtForm.oneUp:
                        int randomNum97 = randomObject.Next(0, 2);
                        switch (randomNum97)
                        {
                            default:
                                break;
                            case 0:
                                //GD.Print("1block" + lastDirtIndex);
                                randomForm = dirtForm.oneBlock;
                                positions.Add(new Vector2(oszlop,lastDirtIndex));
                                break;
                            case 1:
                                randomForm = dirtForm.oneUp;
                                positions.Add(new Vector2(oszlop,lastDirtIndex-1));
                                positions.Add(new Vector2(oszlop,lastDirtIndex));
                                break;
                        }
                        break;
                }
                break;
            case 0:
                int randomNum = randomObject.Next(0, 2);
                switch (randomNum)
                {
                    default:
                        break;
                    case 0:
                        randomForm = dirtForm.oneBlock;
                        positions.Add(new Vector2(oszlop,0));
                        break;
                    case 1:
                        randomForm = dirtForm.oneDown;
                        positions.Add(new Vector2(oszlop,0));
                        positions.Add(new Vector2(oszlop,1));
                        break;
                }
                break;
            case var value when value == mapHeight-1:
                int randomNum2 = randomObject.Next(0, 2);
                switch (randomNum2)
                {
                    default:
                        break;
                    case 0:
                        randomForm = dirtForm.oneBlock;
                        positions.Add(new Vector2(oszlop,mapHeight-1));
                        break;
                    case 1:
                        randomForm = dirtForm.oneUp;
                        positions.Add(new Vector2(oszlop,mapHeight-2));
                        positions.Add(new Vector2(oszlop,mapHeight-1));
                        break;
                }
                break;
        }
        for (int i = 0; i < mapHeight; i++)
        {
            Vector2 pos_to_place = new Vector2();
            string type = "grass";
            if (tileMap.GetCell(oszlop, i) == -1)
            {
                pos_to_place = new Vector2((oszlop*tile_width),(i*tile_height));
                type = "grass";
                tileMap.SetCell(oszlop,i,1);
            }
            if (positions.Count > i && positions[i] != null)
            {  
                pos_to_place = new Vector2((oszlop*tile_width),((int)positions[i].y*tile_height));
                type = "dirt";
               // GD.Print($"Objektum lerakva {positions[i].y}. helyre!");
                tileMap.SetCell(oszlop,(int)positions[i].y,0);
            }
            if (type == "dirt"){
                // Area2D sensor
                Node2D sensorChild1 = (Node2D)sensor.Instance();        
                sensorChild1.Position = pos_to_place;
                sensorChild1.Set("type","dirt");
                AddChild(sensorChild1);
                //

                //Szarok
                int randomNum = randomObject.Next(0,101);
                if (randomNum < spike_chance){
                    Node2D spikeChild = (Node2D)spikestrip.Instance();
                    spikeChild.Position = pos_to_place;
                    spikeChild.Scale = new Vector2(0.1f,0.1f);
                    AddChild(spikeChild);
                }else if (randomNum > nitrous_chance){
                    Node2D nitrousChild = (Node2D)nitrous.Instance();
                    nitrousChild.Position = pos_to_place;
                    nitrousChild.Scale = new Vector2(0.3f,0.3f);
                    AddChild(nitrousChild);
                }
                //
            }
           
        }

        lastDirtForm = randomForm;

    }

    public override void _Process(float delta){
        int curTile = getTileByPos(car);
        //GD.Print("TILE: " + getTileByPos(car));
    }

    public int getTileByPos(KinematicBody2D body){
        int tile_id;
        Vector2 car_pos = new Vector2(car.Position.x,car.Position.y);

       //Vector2 tile_pos = new Vector2((car_pos.x/(tile_width*multiplier)),car_pos.y/((tile_height+1)*multiplier));
        Vector2 tile_pos = tileMap.WorldToMap(car.Position/multiplier);
        //GD.Print(tile_pos);
        //GD.Print(tile_pos);
        int tile = tileMap.GetCell((int)tile_pos.x,(int)tile_pos.y);
        switch (tile){
            default:
                tile_id = -1;
                break;
            case 0:
                tile_id = 0;
                break;
            case 1:
                tile_id = 1;
                break; 
        }

        return tile_id;
    }


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

﻿using UnityEngine;
using System.Collections;

public class ItemEntity
{
    private string name;
    private int x, y, id;

    public ItemEntity(int id, string i, int x, int y)
    {
        this.id = id;
        this.name = i; //Skal flyttes til playerobject
        this.x = x;
        this.y = y;
    }

    public string getName()
    {
        return name;
    }

    public int getID()
    {
        return id;
    }

    public void setX(int x)
    {
        this.x = x;
    }

    public int getX()
    {
        return x;
    }

    public int getY()
    {
        return y;
    }

    public void setY(int y)
    {
        this.y = y;
    }

    public void moveX(int x)
    {
        this.x += x;
    }

    public void moveY(int y)
    {
        this.y += y;
    }
}


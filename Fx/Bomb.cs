using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enum bomb type
public enum BombType
{
    None,
    Column,
    Row,
    Adjacent
};

public class Bomb : Dot
{
    public BombType bombType;
}



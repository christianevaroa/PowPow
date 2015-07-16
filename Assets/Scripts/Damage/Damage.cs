using UnityEngine;
using System.Collections;

// TODO: Add more types as I add stuff
public enum DamageType { PHYSICAL };

// TODO: Decide what this needs to do, whether it should be a class or struct
public struct Damage {

    public int amount;
    public DamageType type;

    public Damage(int amt, DamageType tp)
    {
        amount = amt;
        type = tp;
    }

    public override string ToString()
    {
        return "DamageType: " + type + ", Amount: " + amount;
    }

}

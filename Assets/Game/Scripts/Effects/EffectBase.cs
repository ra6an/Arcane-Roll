using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectBase : IEffect
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Duration { get; set; }

    public EffectType Type { get; set; }

    public ModifyStat Modify {  get; set; }

    //public abstract int Apply(int _amount);
    //public abstract void ApplyStatus();

    public void Add(int _duration)
    {
        Duration += _duration;
    }

    public void Tick()
    {
        if(Duration > 0)
        {
            Debug.Log("Removamo jedan duration");
            Duration--;
            //Debug.Log(Duration);
        }
    }
}

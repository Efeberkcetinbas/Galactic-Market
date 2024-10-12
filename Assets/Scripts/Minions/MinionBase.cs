using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MinionBase : MonoBehaviour
{
    public List<Color> colors=new List<Color>();
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Animator animator;
    protected virtual void Start()
    {
        Initalize();
    }

    protected virtual void Initalize()
    {
        //
        RandomizeColor();
    }

    public abstract void PerformAction();
    public abstract void RandomizeColor();
}

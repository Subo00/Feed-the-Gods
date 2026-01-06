using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Tools/Actions/Plot")]
public class PlotAction : ToolAction
{
    public override void Execute(Player player)
    {
        Debug.Log("I'm doing the thing");
    }
}

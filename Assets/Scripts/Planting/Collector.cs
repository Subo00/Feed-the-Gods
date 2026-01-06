using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Collector
{
    public void ReportBool(bool value);
    public void SetPlantData(PlantData plant);
}
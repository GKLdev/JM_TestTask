using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITransformData
{
    public Vector3 P_Position { get; set; }
    public Quaternion P_Rotation { get; set; }
    public Vector3 P_Orientation { get; set; }
}

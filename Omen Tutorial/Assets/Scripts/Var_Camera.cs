using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variable/Camera")]
public class Var_Camera : ScriptableObject
{
    public Camera Value { get; set; }
}

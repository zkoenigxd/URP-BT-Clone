using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Faction", fileName = "new Faction")]
public class FactionSO : ScriptableObject
{
    public string factionName;
    public string species;
    public List<string> techSpecialties;
    public List<string> shipSpecialties;
    public List<string> GravityWellAttributes;
    public List<string> SectorAttributes;
    public Color mainColor;
    public Color minorColor;

    public string startLevelKey;
}

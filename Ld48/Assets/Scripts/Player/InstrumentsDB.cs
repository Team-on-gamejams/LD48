using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DungeonObject { None, Lopata, Lopata2 }

[System.Serializable]
public class Instruments
{
    public DungeonObject type;
    public Sprite sprite;
    public bool Stackable = false;
    enum InstrumentType { resource, instrument }

}
public class InstrumentsDB : MonoBehaviour
{
    //public Dictionary<DungeonObject, float> instrumentDB = new Dictionary<DungeonObject, float>();
    public List<Instruments> instruments = new List<Instruments>();
}

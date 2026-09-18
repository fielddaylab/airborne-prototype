public enum RoomType
{
    Kitchen,
    Dining,
    Basement,
    Bedroom,
    Living,
    None
}

public enum SlotType
{
    Symptom,
    Source,
    Dialogue,
    Sensor
}

public enum FeatureType
{
    None = 0,
    Stove = 1,
    Furnace = 2,
    Electricity = 3,
    Spraycan = 4,
    MoldPatch = 5,
    Cigarrete = 6,
    Fan = 7,
    Purifier = 8,
    HeatPump = 9
}

public enum FeatureEvent
{
    On,
    Off
}

public enum PollutantType
{
    None,
    FreshAir,
    CO,
    NOx,
    O3,
    VOC,
    SOx,
    Mold,
    Dust
}

public enum Symptom
{
    None,
    ShortBreath,
    LungIrritation,
    Headache,
    Dizziness,
    Confusion,
    EyeBurn,
    ChestPain,
    LossConsciousness,
    Nausea,
    Cough,
    Z
}

public enum CharacterType
{
    Roundy,
    Blockhead,
    Triangelo
}

public enum DialogueSenses
{
    None,
    MetallicOdor,
    Various
}

public enum ConnectionType {
    Door,
    Window,
    Vent,
}

public enum FlowChangeEventType {
    Add,
    Remove,
    Move,
    Swap
}

public enum EquipmentType
{
    None,
    Observe,
    Scan,
    Meter,
    Fan,
    Filter,
    Cleaner,
    ElectricStove,
    HeatPump
}
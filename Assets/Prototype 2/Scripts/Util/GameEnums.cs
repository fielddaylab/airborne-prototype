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
    Furnace,
    Stove,
    Electricity,
    Spraycan,
    Fan,
    None,
    MoldPatch
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
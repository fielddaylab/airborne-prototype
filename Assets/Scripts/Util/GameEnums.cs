public enum RoomType
{
    Kitchen,
    Dining,
    Basement,
    Bedroom
}

public enum SlotType
{
    Symptom,
    Source,
    Dialogue,
    Sensor
}

public enum PollutionSource
{
    Furnace,
    Stove,
    Electricity,
    Spraycan
}

public enum SourceAction
{
    On,
    Off
}

public enum Pollutant
{
    None,
    FreshAir,
    CO2,
    NO,
    O3,
    VOC
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
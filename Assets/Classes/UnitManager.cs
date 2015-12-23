
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public enum UnitPartType
{
    Training,
    Weapon
}

public class UnitPart
{
    public string ID { get; private set; }
    public string Name { get; private set; }
    public UnitPartType Type { get; private set; }
    public int Cost { get; private set; }

    public UnitPart(string id, string name, UnitPartType type, int cost)
    {
        ID = id;
        Name = name;
        Type = type;
        Cost = cost;
    }
}

public class UnitManager
{
    private Dictionary<string, UnitPart> parts_ = new Dictionary<string, UnitPart>();

    public UnitPart GetPart(string id)
    {
        return parts_[id];
    }

    public void LoadFromXml(string text)
    {
        StringReader stream = new StringReader(text);
        XmlReader reader = XmlReader.Create(stream);
        try
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "unit_parts")
                {
                    ReadXmlParts(reader);
                }
            }
        }
        finally
        {
            stream.Dispose();
        }
    }

    private void ReadXmlParts(XmlReader reader)
    {
        while(reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "part")
            {
                UnitPart part = ReadPart(reader);
                parts_.Add(part.ID, part);
            }
            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "unit_parts")
                break;
        }
    }

    private UnitPart ReadPart(XmlReader reader)
    {
        string id = string.Empty;
        string name = string.Empty;
        UnitPartType type = UnitPartType.Training;
        int cost = 1;
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "name")
            {
                name = reader.ReadElementContentAsString();
            }
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "id")
            {
                id = reader.ReadElementContentAsString();
            }
            else if (reader.NodeType == XmlNodeType.Element && reader.Name == "type")
            {
                string t = reader.ReadElementContentAsString();
                if (t == "TRAINING")
                    type = UnitPartType.Training;
                else if (t == "WEAPON")
                    type = UnitPartType.Weapon;
                else
                    throw new Exception("Unknown part type");
            }
            else if (reader.NodeType == XmlNodeType.Element && reader.Name == "cost")
            {
                cost = reader.ReadElementContentAsInt();
            }
            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "part")
                break;
        }
        return new UnitPart(id, name, type, cost);
    }
}

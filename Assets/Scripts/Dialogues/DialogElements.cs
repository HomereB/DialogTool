/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot(ElementName = "Dialog")]
public class Dialog
{
    [XmlElement(ElementName = "Node")]
    public List<Node> Node { get; set; }
}


[XmlRoot(ElementName = "Node")]
public class Node
{
    [XmlElement(ElementName = "NodeId")]
    public int NodeId { get; set; }
    [XmlElement(ElementName = "Reply")]
    public List<Reply> Reply { get; set; }
    [XmlElement(ElementName = "Answers")]
    public Answers Answers { get; set; }
}


[XmlRoot(ElementName = "Reply")]
public class Reply
{
    [XmlElement(ElementName = "CharacterID")]
    public int CharacterID { get; set; }
    [XmlElement(ElementName = "CharacterImageLeft")]
    public string CharacterImageLeft { get; set; }
    [XmlElement(ElementName = "CharacterImageRight")]
    public string CharacterImageRight { get; set; }
    [XmlElement(ElementName = "Text")]
    public string Text { get; set; }
    [XmlElement(ElementName = "DisplayTime")]
    public float DisplayTime { get; set; }
    [XmlElement(ElementName = "DisplayTime")]
    public float TimeToDisplay { get; set; }
}


[XmlRoot(ElementName = "Answers")]
public class Answers
{
    [XmlElement(ElementName = "Choice")]
    public List<Choice> Choices { get; set; }
    [XmlElement(ElementName = "AnswerTime")]
    public float AnswerTime { get; set; }
    [XmlElement(ElementName = "AnswerTime")]
    public int defaultChoice { get; set; }
}


[XmlRoot(ElementName = "Choice")]
public class Choice
{
    [XmlElement(ElementName = "ChoiceID")]
    public int ChoiceID { get; set; }
    [XmlElement(ElementName = "ChoiceText")]
    public string ChoiceText { get; set; }

    [XmlElement(ElementName = "TimeBeforeDisplay")]
    public float TimeBeforeDisplay { get; set; }
    [XmlElement(ElementName = "DisplayTime")]
    public float DisplayTime { get; set; }

    [XmlElement(ElementName = "CharactersRelationshipImpacts")]
    public CharactersRelationshipImpacts CharactersRelationshipImpacts { get; set; }
    [XmlElement(ElementName = "NextNodeID")]
    public int NextNodeID { get; set; }
}


[XmlRoot(ElementName = "CharactersRelationshipImpact")]
public class CharactersRelationshipImpacts
{
    [XmlElement(ElementName = "CharacterID")]
    public List<int> CharacterID { get; set; }
    [XmlElement(ElementName = "RelationshipImpact")]
    public List<int> RelationshipImpact { get; set; }
}
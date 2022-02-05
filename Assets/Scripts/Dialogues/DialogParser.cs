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

public class DialogParser : MonoBehaviour
{
    public TextAsset rawDialogFile;
    public Dialog dialog;
    DialogDisplay dialogDisplay;

    private void Start()
    {
        string rawDialog = rawDialogFile.text;
        ParseXMLFile(rawDialog);
        dialogDisplay = gameObject.GetComponent<DialogDisplay>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            dialogDisplay.SetCurrentDialog(dialog);
            dialogDisplay.StartDialog();
        }
    }

    private void ParseXMLFile(string xmlData)
    {
        dialog = new Dialog();
        dialog.Node = new List<Node>();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new StringReader(xmlData));

        string xmlPathPattern = "//Dialog/Node";
        XmlNodeList myNodeList = xmlDoc.SelectNodes(xmlPathPattern);

        foreach(XmlNode node in myNodeList)
        {
            Node currentNode = new Node();

            XmlNodeList myNodeChildrenList = node.ChildNodes;

            currentNode.NodeId = int.Parse(node.FirstChild.InnerXml);

            currentNode.Reply = new List<Reply>();

            for(int i = 1 ; i < myNodeChildrenList.Count-1 ; i++)
            {
                // Parse a Reply
                //Debug.Log(myNodeChildrenList[i].SelectSingleNode("descendant::CharacterID").InnerText);

                Reply currentReply = new Reply();
                currentReply.CharacterID = int.Parse(myNodeChildrenList[i].SelectSingleNode("descendant::CharacterID").InnerText);
                currentReply.CharacterImageLeft = myNodeChildrenList[i].SelectSingleNode("descendant::CharacterImageLeft").InnerText;
                currentReply.CharacterImageRight = myNodeChildrenList[i].SelectSingleNode("descendant::CharacterImageRight").InnerText;
                currentReply.Text = myNodeChildrenList[i].SelectSingleNode("descendant::Text").InnerText;
                currentReply.DisplayTime = float.Parse(myNodeChildrenList[i].SelectSingleNode("descendant::DisplayTime").InnerText);
                currentReply.TimeToDisplay = float.Parse(myNodeChildrenList[i].SelectSingleNode("descendant::TimeToDisplay").InnerText);

                currentNode.Reply.Add(currentReply);
            }
            //Parse Answer
            currentNode.Answers = new Answers();
            XmlNode myNodeAnswers = node.LastChild;
            
            //Setup choices list
            currentNode.Answers.Choices = new List<Choice>();
            XmlNodeList myAnswerChoices = myNodeAnswers.ChildNodes;

            for(int i = 0 ; i < myAnswerChoices.Count-2 ; i++)
            {
                //Parse a Choice
                Choice currentChoice = new Choice();

                currentChoice.ChoiceID = int.Parse(myAnswerChoices[i].SelectSingleNode("descendant::ChoiceID").InnerText);
                currentChoice.ChoiceText = myAnswerChoices[i].SelectSingleNode("descendant::ChoiceText").InnerText;     
                currentChoice.NextNodeID = int.Parse(myAnswerChoices[i].SelectSingleNode("descendant::NextNodeID").InnerXml);
                currentChoice.DisplayTime = float.Parse(myAnswerChoices[i].SelectSingleNode("descendant::DisplayTime").InnerXml);
                currentChoice.TimeBeforeDisplay = float.Parse(myAnswerChoices[i].SelectSingleNode("descendant::TimeBeforeDisplay").InnerXml);

                //Parse Relationship Impacts
                CharactersRelationshipImpacts currentCharactersRelationshipImpact = new CharactersRelationshipImpacts();
                currentCharactersRelationshipImpact.CharacterID = new List<int>();
                currentCharactersRelationshipImpact.RelationshipImpact = new List<int>();

                XmlNodeList choiceCharacterRelationshipImpacts = myAnswerChoices[i].SelectSingleNode("descendant::CharactersRelationshipImpacts").ChildNodes;
                for(int j = 0 ; j < choiceCharacterRelationshipImpacts.Count ; j++)
                {
                    switch(j%2)
                    {
                        case 0:
                            currentCharactersRelationshipImpact.CharacterID.Add(int.Parse(choiceCharacterRelationshipImpacts[j].InnerText));
                            break;

                        case 1:
                            currentCharactersRelationshipImpact.RelationshipImpact.Add(int.Parse(choiceCharacterRelationshipImpacts[j].InnerText));
                            break;
                    }
                }
                currentChoice.CharactersRelationshipImpacts = currentCharactersRelationshipImpact;
                currentNode.Answers.Choices.Add(currentChoice);
            }
            currentNode.Answers.AnswerTime = float.Parse(myAnswerChoices[myAnswerChoices.Count - 2].InnerXml);
            currentNode.Answers.defaultChoice = int.Parse(myAnswerChoices[myAnswerChoices.Count - 1].InnerXml);

            dialog.Node.Add(currentNode);          
        }
    }
}
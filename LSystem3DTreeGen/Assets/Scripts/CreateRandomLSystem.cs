﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRandomLSystem
{
    //    public static List<Module> initialModuleForSpaceFillingLine
    //= new List<Module> {
    //         new Module("A", new List<float> { 1, 0 }, (float v1, float v2) => v1, (float v1, float v2) => v2, (float v1, float v2) => true)
    //};
    //    public static Dictionary<(string, bool), List<Module>> moduleAlphabetForSpaceFillingLine = new Dictionary<(string, bool), List<Module>>
    //    {
    //        {("A", true), new List<Module>{
    //                         new Module("F", new List<float> { 0, 0 }, (float v1, float v2) => v1, (float v1, float v2) => v1, (float v1, float v2) => true),
    //                         new Module("[", new List<float> { 0, 0 }, (float v1, float v2) => v2, (float v1, float v2) => v2, (float v1, float v2) => true),
    //                         new Module("+", new List<float> { 0, 0 }, (float v1, float v2) => d, (float v1, float v2) => v2, (float v1, float v2) => true),
    //                         new Module("A", new List<float> { 0, 0 }, (float v1, float v2) => v1/1.456f, (float v1, float v2) => v2, (float v1, float v2) => true),
    //                         new Module("]", new List<float> { 0, 0 }, (float v1, float v2) => v2, (float v1, float v2) => v2, (float v1, float v2) => true),
    //                         new Module("[", new List<float> { 0, 0 }, (float v1, float v2) => v2, (float v1, float v2) => v2, (float v1, float v2) => true),
    //                         new Module("+", new List<float> { 0, 0 }, (float v1, float v2) => -d, (float v1, float v2) => v2, (float v1, float v2) => true),
    //                         new Module("A", new List<float> { 0, 0 }, (float v1, float v2) => v1/1.456f, (float v1, float v2) => v2, (float v1, float v2) => true),
    //                         new Module("]", new List<float> { 0, 0 }, (float v1, float v2) => v2, (float v1, float v2) => v2, (float v1, float v2) => true)
    //        } }
    //    };
    List<string> allPossibleActions = new List<string>() { "F", "+", "/", "&", "[", "]", "!"};
    List<string> allPossibleMoves = new List<string>() { "F", "+", "/", "&"};
    List<string> allPossibleTurns = new List<string>() { "+", "/", "&" };
    List<string> nonActionable = new List<string>() { "A", "B", "C" };


    public Dictionary<(string, bool), List<Module>> CreateRandomAlphabet()
    {
        List<string> possibleKeys = new List<string>();
        List<string> nonActionableKeys = new List<string>();
        Dictionary<(string, bool), List<Module>> alphabet = new Dictionary<(string, bool), List<Module>>();
        float connectorAngle;
        string connectorTurn;
        //Creates a random amount of non-actionable keys in alphabet between 1 and 3
        int p = Random.Range(1, 4);
        for (int i = 0; i < p; i++)
        {
            string name = nonActionable[i];
            possibleKeys.Add(name);
            nonActionableKeys.Add(name);
        }
        int keyindex = 0;
        //Sets decrease in f over time
        float fDecrease = Random.Range(.5f, .7f);
        //For each key present in alphabet assign a branching structure to that key
        //On some occasions assign another non-actionable key to that branching structure
        foreach (string nameOfKey in nonActionableKeys)
        {
            List<Module> value = new List<Module>();
            
            
            connectorAngle = RandomAngle();
            connectorTurn = GetRandomTurn();

            
            List<Module> branchingStructure = CreateRandomBranchingStructure(nonActionableKeys[keyindex % nonActionableKeys.Count],
                fDecrease, nonActionableKeys.Count);
                
            value.AddRange(branchingStructure);

            if(Rand() > .5f)
            {
                value.Add(new Module(nonActionable[Random.Range(0,nonActionableKeys.Count)], new List<float> { 0, 0 }, (float v1, float v2) => v1,
                (float v1, float v2) => v2, (float v1, float v2) => true));
            }
            //if alphabet doesent already have the key add it to the alphabet
            if(!alphabet.ContainsKey((nameOfKey, true)))
                alphabet.Add((nameOfKey, true), value);
        }
        return alphabet;
    }
    public List<Module> CreateRandomAxiom()
    {
        List<Module> axiom = new List<Module>();
        float f = Random.Range(3, 5);
        axiom.Add(new Module("F", new List<float> { 1, 20 }, (float v1, float v2) => v1, (float v1, float v2) => v2, (float v1, float v2) => true));
        axiom.Add(new Module("!", new List<float> { 22, 10 }, (float v1, float v2) => v1, (float v1, float v2) => v2, (float v1, float v2) => true));
        
        axiom.Add(CreateRandomModule(true, false, f));
        axiom.Add(CreateRandomModule(true, false, f));
        return axiom;   
    }
    List<Module> CreateRandomBranchingStructure(string nonActionableCharacter, float FDecrease, int otherActions)
    {
        List<Module> Guts = new List<Module>();
        int c = Random.Range(1,4);

        float randomDecrease = Random.Range(.6f, .8f);
        float theta = RandomAngle();
        Module F = new Module("F", new List<float> { 0, 0 }, (float v1, float v2) => v1, (float v1, float v2) => v2, (float v1, float v2) => true);
        Guts.Add(new Module("!", new List<float> { 0, 0 }, (float v1, float v2) => v2,
            (float v1, float v2) => v2, (float v1, float v2) => true));
        Guts.Add(F);
        for (int p = 0; p < c; p++)
        {
            int r = 1;
            
            Module Push = new Module("[", new List<float> { 0, 0 }, (float v1, float v2) => v2, (float v1, float v2) => v2, (float v1, float v2) => true);
            Module Pop = new Module("]", new List<float> { 0, 0 }, (float v1, float v2) => v2, (float v1, float v2) => v2, (float v1, float v2) => true);
            Module A = new Module(nonActionableCharacter, new List<float> { 0, 0 }, (float v1, float v2) => v1 * FDecrease, (float v1, float v2) => v2 * .707f,
                                (float v1, float v2) => true);
            
            float v1Modifier = Random.Range(0.0f, 2.0f);
            float v2Modifier = Random.Range(.6f, .9f);
            Guts.Add(new Module("+", new List<float> { 0, 0 }, (float v1, float v2) => -RandomAngle()/30f,
                       (float v1, float v2) => v2, (float v1, float v2) => true));
            Guts.Add(Push);
            
            for (int i = 0; i < r; i++)
            {
                Guts.Add(new Module(GetRandomYawPitch(), new List<float> { 0, 0 }, (float v1, float v2) => theta,
                                (float v1, float v2) => v2 * v2Modifier, (float v1, float v2) => true));
            }
            if(Rand()< .5f)
            {
                Guts.Add(new Module("F", new List<float> { 0, 0 }, (float v1, float v2) => v1 * Random.Range(.8f,1.1f), (float v1, float v2) => v2, (float v1, float v2) => true));
            }
            

            Guts.Add(A);
            Guts.Add(Pop);
            Guts.Add(new Module("/", new List<float> { 0, 0 }, (float v1, float v2) => Rand() * 360 / c,
                       (float v1, float v2) => v2, (float v1, float v2) => true));
        }
        
        
        if(Rand() > .5f)
        {
            Guts.Add(new Module(nonActionable[Random.Range(0,otherActions)], new List<float> { 0, 0 }, (float v1, float v2) => v1 * FDecrease / 2,
            (float v1, float v2) => v2 * Random.Range(.7f, .8f), (float v1, float v2) => true));
        }


        return Guts;
    }
    float RandomAngle()
    {
        return Random.Range(22f, 70f);
    }
    Module GetClosedModule()
    {
        return new Module("]", new List<float> { 0, 0 }, (float v1, float v2) => v1,
                (float v1, float v2) => v2, (float v1, float v2) => true);
    }
    Module CreateRandomModule(bool randomParameters, bool actionable, float fLength)
    {
        List<float> parameters = GetRandomParameters(fLength);


        if (actionable)
        {
            string action = GetRandomAction();

            return new Module(action, parameters, (float v1, float v2) => v1, 
                (float v1, float v2) => v2, (float v1, float v2) => true);
        }
        else
        {
            return new Module(GetRandomNonAction(), parameters, (float v1, float v2) => v1, 
                (float v1, float v2) => v2, (float v1, float v2) => true);
        }
        
    }
    string GetRandomAction()
    {
        return allPossibleActions[Random.Range(0,allPossibleActions.Count)];
    }
    string GetRandomMove()
    {
        return allPossibleMoves[Random.Range(0, allPossibleMoves.Count)];
    }
    string GetRandomTurn()
    {
        return allPossibleTurns[Random.Range(0, allPossibleTurns.Count)];
    }
    string GetRandomYawPitch()
    {
        float f = Rand();
        if (f > .66f) return "&";
        else if (f > .33) return "+";
        else return "$";
    }
    string GetRandomNonAction()
    {
        return nonActionable[Random.Range(0, nonActionable.Count)];
    }
    List<float> GetRandomParameters(float fLength)
    {
        List<float> parameters;
        parameters = new List<float> { fLength, 20 };
        return parameters;
    }
    float Rand()
    {
        return Random.Range(0.0f, 1.0f);
    }
}

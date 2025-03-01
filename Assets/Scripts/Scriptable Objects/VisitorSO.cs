using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Visitor", menuName = "Create Visitor")]
public class VisitorSO : ScriptableObject
{
    public string id;
    public string firstName;
    public string lastName;
    public int age;
    public string profession;

    public int height;
    public int weight;

    public Date expirationDate;
    public List<Trait> stats;
    public List<Trait> myPreferredTraits;

    public Sprite idIcon;
    public Sprite spriteImage;

    public int baseScore;
    public int rareScoreMultiplier;
}

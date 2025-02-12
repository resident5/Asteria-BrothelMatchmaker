using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Visitor
{
    public string Name => $"{firstName} {lastName}";
    public string firstName;
    public string lastName;
    public string id;

    public int age;
    public string profession;

    public int height;
    public int weight;

    public Date expirationDate;
    public List<Trait> stats;
    public List<Trait> myPreferredTraits;

    public Sprite icon;

    public bool isMember = true;

    private int baseScore;
    private int rareScoreMultiplier;

    private int FinalScoreMultiplier => baseScore * rareScoreMultiplier;

    //Changeable Values: These values are to be changed when generating a visitor...
    //These values are used to determine whether the Visitor is a member or an imposter
    public string memberID;

    public Visitor(VisitorSO visitorSO)
    {
        id = visitorSO.id;
        firstName = visitorSO.firstName;
        lastName = visitorSO.lastName;
        age = visitorSO.age;
        profession = visitorSO.profession;
        expirationDate = visitorSO.expirationDate;
        stats = visitorSO.stats;
        myPreferredTraits = visitorSO.myPreferredTraits;
        icon = visitorSO.icon;
    }

    //public string GetHeightInCM()
    //{
    //    return $"{mytraits.height}cm";
    //}

    //public string GetHeightInFeet()
    //{
    //    float totalInches = mytraits.height * .3937f;
    //    int foot = (int)totalInches / 12;
    //    float inches = totalInches % 12;
    //    return $"{foot}ft {inches}in";
    //}

    public int CompareStatTraits(Visitor other)
    {
        int statScore = 0;
        foreach (var trait in myPreferredTraits)
        {
            switch (trait)
            {
                case Trait.PREFERYOUNGER:
                    if (age > other.age)
                        statScore += 100;
                    break;
                case Trait.PREFEROLDER:
                    if (age < other.age)
                        statScore += 100;
                    break;
                case Trait.PREFERSAMEAGE:
                    if (age == other.age)
                        statScore += 100;
                    break;
                case Trait.PREFERTALLER:
                    if (height < other.height)
                        statScore += 100;
                    break;
                case Trait.PREFERSHORTER:
                    if (height > other.height)
                        statScore += 100;
                    break;
                default:
                    break;
            }
        }

        return statScore;
    }
}

public enum Trait
{
    //Traits
    FRIENDLY, SERIOUS,
    STRONG, WEAK,
    SUBMISSIVE, DOMINANT,
    PHYSICAL, MAGICAL,
    HERO, CHAMPION, VISITOR,
    SKINNY, LEAN, MUSCULAR, STOCKY, CHUBBY,
    FURRY, SCALIE, HORNY,

    //Preferred Traits
    PREFERYOUNGER, PREFEROLDER, PREFERSAMEAGE,
    PREFERTALLER, PREFERSHORTER, PREFERSAMEHEIGHT,
}


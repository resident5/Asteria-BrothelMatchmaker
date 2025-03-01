using DIALOGUE;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public Sprite idIcon;
    public Sprite spriteImage;

    public bool isMember = true;

    private int baseScore;
    private int rareScoreMultiplier = 1;

    public int FinalScoreMultiplier => baseScore * rareScoreMultiplier;

    public ImposterType imposterType = ImposterType.NONE;

    //Changeable Values: These values are to be changed when generating a visitor...
    //These values are used to determine whether the Visitor is a member or an imposter
    public string memberID;
    public string displayName => Name;

    public DialogueManager dialogueManager => DialogueManager.Instance;

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
        idIcon = visitorSO.idIcon;
        spriteImage = visitorSO.spriteImage;
    }

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

    public Coroutine Say(string dialogue) => Say(new List<string> { dialogue });

    public Coroutine Say(List<string> lines)
    {
        dialogueManager.ShowSpeakerName(displayName);
        return dialogueManager.Say(lines);
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

public enum ImposterType
{
    NONE,
    FAKE_NAME,
    FAKE_ID,
    FAKE_ICON,
    FAKE_EXPIRATION_DATE
}


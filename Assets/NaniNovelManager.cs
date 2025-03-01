using UnityEngine;
using Naninovel;
using Naninovel.Commands;
using UnityEngine.PlayerLoop;
using Naninovel.Expression;
using System.Collections.Generic;

public class NaniNovelManager : MonoBehaviour
{
    public static NaniNovelManager instance { get; private set; }
    ICustomVariableManager manager;

    public string scriptName;
    public string label;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Init();
        manager = Engine.GetService<ICustomVariableManager>();
        if(Input.GetKeyDown(KeyCode.None))
        {

        }
    }

    public async void Init()
    {
        await RuntimeInitializer.Initialize();

        var switchCommand = new SwitchToAdventureMode { ResetState = false };
        await switchCommand.Execute();
    }

    public void SetVariable(Visitor visitor)
    {
        var manager = Engine.GetService<ICustomVariableManager>();

        Trait strengthTrait = Trait.VISITOR;
        Trait mindTrait = Trait.VISITOR;
        Trait bodyTrait = Trait.VISITOR;
        Trait jobTrait = Trait.VISITOR;
        Trait attackTrait = Trait.VISITOR;
        foreach (var trait in visitor.myPreferredTraits)
        {
            if (trait == Trait.STRONG || trait == Trait.WEAK)
            {
                strengthTrait = trait;
            }

            if (trait == Trait.DOMINANT || trait == Trait.SUBMISSIVE)
            {
                mindTrait = trait;
            }

            if (trait == Trait.LEAN || trait == Trait.STOCKY || trait == Trait.MUSCULAR || trait == Trait.CHUBBY)
            {
                bodyTrait = trait;
            }

            if (trait == Trait.CHAMPION || trait == Trait.VISITOR || trait == Trait.HERO)
            {
                jobTrait = trait;
            }

            if (trait == Trait.PHYSICAL || trait == Trait.MAGICAL)
            {
                attackTrait = trait;
            }
        }

        if (strengthTrait != Trait.VISITOR)
        {
            CustomVariableValue variable = new CustomVariableValue(ConvertTraitIntoStrings(strengthTrait));
            manager.SetVariableValue("visitor_strength_pref", variable);
        }

        if (mindTrait != Trait.VISITOR)
        {
            CustomVariableValue variable = new CustomVariableValue(ConvertTraitIntoStrings(mindTrait));
            manager.SetVariableValue("visitor_mind_pref", variable);
        }

        if (bodyTrait != Trait.VISITOR)
        {
            CustomVariableValue variable = new CustomVariableValue(ConvertTraitIntoStrings(bodyTrait));
            manager.SetVariableValue("visitor_body_pref", variable);
        }

        if (jobTrait != Trait.VISITOR)
        {
            CustomVariableValue variable = new CustomVariableValue(ConvertTraitIntoStrings(jobTrait));
            manager.SetVariableValue("visitor_job_pref", variable);
        }

        if (attackTrait != Trait.VISITOR)
        {
            CustomVariableValue variable = new CustomVariableValue(ConvertTraitIntoStrings(attackTrait));
            manager.SetVariableValue("visitor_attack_pref", variable);
        }
        Debug.Log($"Should have set variable");
    }

    public string ConvertTraitIntoStrings(Trait trait)
    {
        string descriptor = "";

        switch (trait)
        {
            case Trait.FRIENDLY:
                descriptor += "a man that looks friendly";
                break;
            case Trait.SERIOUS:
                descriptor += "someone that looks serious";
                break;
            case Trait.STRONG:
                descriptor += "someone so strong he can protect me";
                break;
            case Trait.WEAK:
                descriptor += "a man so weak I can protect him";
                break;
            case Trait.SUBMISSIVE:
                descriptor += "maybe someone a little bit willing and submissive";
                break;
            case Trait.DOMINANT:
                descriptor += "I... want someone demanding and dominant";
                break;
            case Trait.PHYSICAL:
                descriptor += "someone that has physical strength";
                break;
            case Trait.MAGICAL:
                descriptor += "someone with magical aptitude";
                break;
            case Trait.HERO:
                descriptor += "a Hero, despite how rare they are";
                break;
            case Trait.CHAMPION:
                descriptor += "a Champion through and through";
                break;
            case Trait.VISITOR:
                descriptor += "friendly";
                break;
            case Trait.SKINNY:
                descriptor += "friendly";
                break;
            case Trait.LEAN:
                descriptor += "friendly";
                break;
            case Trait.MUSCULAR:
                descriptor += "and he's gotta be ripped too";
                break;
            case Trait.STOCKY:
                descriptor += "friendly";
                break;
            case Trait.CHUBBY:
                descriptor += "friendly";
                break;
            case Trait.FURRY:
                descriptor += "friendly";
                break;
            case Trait.SCALIE:
                descriptor += "friendly";
                break;
            case Trait.HORNY:
                descriptor += "friendly";
                break;
            case Trait.PREFERYOUNGER:
                break;
            case Trait.PREFEROLDER:
                break;
            case Trait.PREFERSAMEAGE:
                break;
            case Trait.PREFERTALLER:
                break;
            case Trait.PREFERSHORTER:
                break;
            case Trait.PREFERSAMEHEIGHT:
                break;
            default:
                break;
        }


        return descriptor;
    }

    private string ParseTrait(Trait t)
    {
        //Convert the Trait to lowercase then capitalize the first letter
        string trait = t.ToString().ToLower();
        return char.ToUpper(trait[0]) + trait.Substring(1).ToLower();
    }

    public void PlayDialogue()
    {
        var inputManager = Engine.GetServiceOrErr<IInputManager>();
        inputManager.ProcessInput = true;

        var scriptPlayer = Engine.GetServiceOrErr<IScriptPlayer>();
        scriptPlayer.LoadAndPlayAtLabel(scriptName, label).Forget();
    }

    public void PlayDialogue(string sName, string sLabel)
    {
        var inputManager = Engine.GetServiceOrErr<IInputManager>();
        inputManager.ProcessInput = true;

        var scriptPlayer = Engine.GetServiceOrErr<IScriptPlayer>();
        scriptPlayer.LoadAndPlayAtLabel(sName, sLabel).Forget();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    PlayDialogue();
        //}
    }
}


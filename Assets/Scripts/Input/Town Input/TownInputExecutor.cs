using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZemDirections;

public class TownInputExecutor : AbstractInputExecutor<TownInputInterpreter, TownInputListener>
{
    protected override void ManageWindows()
    {
        BuildStructure();
        RecruitHero();
        RecruitCreature();
    }

    protected override bool HasCurrentWindow()
    {
        return FieldUI.Instance.currentWindow;
    }

    public override void ExecuteInputs()
    {
        ManageWindows();
        ExitTown();
    }

    private void BuildStructure()
    {
        if (interpreter.buildStructure) TownManager.Instance.BuildStructurePanel();
    }

    private void RecruitHero()
    {
        if (interpreter.recruitHero) TownManager.Instance.RecruitHeroPanel();
    }

    private void RecruitCreature()
    {
        if (interpreter.recruitCreature) TownManager.Instance.RecruitCreaturePanel();
    }

    private void ExitTown()
    {
        if (!interpreter.exitTown) return;

        AUIPanel currentWindow = TownUI.Instance.currentWindow;
        if (currentWindow)
        {
            TownUI.Instance.CloseCurrentWindow();
            return;
        }

        TownManager.Instance.ExitTown();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceStats : MonoBehaviour
{
    [Header("Experience")]
    public int level;
    public int experience;

    public void Initialize()
    {
        level = 1;
        experience = 0;
    }

    public void Initialize(ExperienceData experienceData)
    {
        if (experienceData == null)
        {
            Initialize();
            return;
        }

        level = experienceData.level;
        experience = ExperienceCalculation.CalculateExperienceToLevel(level);
    }
}

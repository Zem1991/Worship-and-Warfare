﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceStats : MonoBehaviour
{
    [Header("Experience")]
    public int level;
    public long experience;

    public void Initialize(ExperienceData experienceData)
    {
        level = experienceData.level;
        experience = 0;     // experienceData.experience;   //TODO calculate experience from current level somewhere
    }
}

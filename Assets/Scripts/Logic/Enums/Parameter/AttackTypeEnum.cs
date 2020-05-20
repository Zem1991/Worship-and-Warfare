public enum AttackType
{
    MELEE,
    RANGED
}

public static class AttackTypeEnum
{
    public static string GetAnimatorStateName(this AttackType attackType)
    {
        string result = "Unknown";
        switch (attackType)
        {
            case AttackType.MELEE:
                result = "Melee";
                break;
            case AttackType.RANGED:
                result = "Ranged";
                break;
            default:
                break;
        }
        result += " attack";
        return result;
    }
}
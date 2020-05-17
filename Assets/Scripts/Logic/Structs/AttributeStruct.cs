[System.Serializable]
public struct AttributeStruct
{
    public int offense;
    public int defense;
    public int support;
    public int command;
    public int magic;
    public int tech;

    public void Increment(AttributeType attrType)
    {
        switch (attrType)
        {
            case AttributeType.OFFENSE:
                offense++;
                break;
            case AttributeType.DEFENSE:
                defense++;
                break;
            case AttributeType.SUPPORT:
                support++;
                break;
            case AttributeType.COMMAND:
                command++;
                break;
            case AttributeType.MAGIC:
                magic++;
                break;
            case AttributeType.TECH:
                tech++;
                break;
            default:
                break;
        }
    }

    //public void Decrement(AttributeType attrType)
    //{
    //    switch (attrType)
    //    {
    //        case AttributeType.OFFENSE:
    //            offense--;
    //            break;
    //        case AttributeType.DEFENSE:
    //            defense--;
    //            break;
    //        case AttributeType.SUPPORT:
    //            support--;
    //            break;
    //        case AttributeType.COMMAND:
    //            command--;
    //            break;
    //        case AttributeType.MAGIC:
    //            magic--;
    //            break;
    //        case AttributeType.TECH:
    //            tech--;
    //            break;
    //        default:
    //            break;
    //    }
    //}
}

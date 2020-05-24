using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AES_Formula : MonoBehaviour
{
    public string formula;

    public int EvaluateFormula()
    {
        //TODO make my own formula parser
        return int.Parse(formula);
    }
}

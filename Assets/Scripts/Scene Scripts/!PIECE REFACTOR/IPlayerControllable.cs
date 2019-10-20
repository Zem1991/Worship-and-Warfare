using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerControllable
{
    bool IPC_HasController();
    Player IPC_GetController();
    void IPC_SetController(Player player);
}

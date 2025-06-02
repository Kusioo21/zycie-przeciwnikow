using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public void SelectIsrael()
    {
        GameManager.Instance.StartGame(Side.Israel);
    }
    public void SelectPalestine()
    {
        GameManager.Instance.StartGame(Side.Palestine);
    }
}

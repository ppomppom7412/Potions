using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe
{
    //개별 정보
    public string name;
    public int id;

    //재료 리스트 - 레시피
    public int[] list;
    public int count;

    //레시피 전체 정보
    public const int maxKinds = 6;
    public enum Recipes { heal, mana, critical, chrming, elixir, detox, max }

    public Recipe(int kind)
    {
        switch (kind)
        {
            case 0:
                Create_HealRecipe();
                break;
            case 1:
                Create_ManaRecipe();
                break;
            case 2:
                Create_CriticalRecipe();
                break;
            case 3:
                Create_CharmingRecipe();
                break;
            case 4:
                Create_ElixirRecipe();
                break;
            case 5:
                Create_DetoxRecipe();
                break;
            default:
                Debug.Log("D01 :: recipe made error");
                Create_HealRecipe();
                break;
        }//end switch
    }

    /// <summary>
    /// 레시피를 string으로 읽는다
    /// </summary>
    /// <returns></returns>
    public string ReadRecipe()
    {
        string result = "";

        foreach (int i in list)
        {
            result += Flower.StringMaterials(i);
            result += " ";
        }
        return result;
    }

    /// <summary>
    /// 회복 포션 레시피
    /// </summary>
    void Create_HealRecipe()
    {
        name = "Heal";
        id = (int)Recipes.heal;
        list = new int[2];
        count = 2;
        list[0] = 0;
        list[1] = 0;
    }

    /// <summary>
    /// 마나 포션 레시피
    /// </summary>
    void Create_ManaRecipe()
    {
        name = "Mana";
        id = (int)Recipes.mana;
        list = new int[2];
        count = 2;
        list[0] = 1;
        list[1] = 4;
    }

    /// <summary>
    /// 크리티컬 포션 레시피
    /// </summary>
    void Create_CriticalRecipe()
    {
        name = "Critical";
        id = (int)Recipes.critical;
        list = new int[2];
        count = 2;
        list[0] = 2;
        list[1] = 5;
    }

    /// <summary>
    /// 매력 포션 레시피
    /// </summary>
    void Create_CharmingRecipe()
    {
        name = "Charming";
        id = (int)Recipes.chrming;
        list = new int[3];
        count = 3;
        list[0] = 3;
        list[1] = 1;
        list[2] = 3;
    }

    /// <summary>
    /// 엘릭서 레시피
    /// </summary>
    void Create_ElixirRecipe()
    {
        name = "Elixir";
        id = (int)Recipes.elixir;
        list = new int[3];
        count = 3;
        list[0] = 4;
        list[1] = 4;
        list[2] = 2;
    }

    /// <summary>
    /// 해독제 레시피
    /// </summary>
    void Create_DetoxRecipe()
    {
        name = "Detox";
        id = (int)Recipes.detox;
        list = new int[3];
        count = 3;
        list[0] = 5;
        list[1] = 5;
        list[2] = 3;
    }
}




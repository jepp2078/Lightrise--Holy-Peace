﻿using UnityEngine;
using System.Collections;

public class Skill_Passive_General_Rigor : SkillEntity, Skill
{

	private static int id = 9;
    private static string type = "passive";
    private static string group = "general";
    private static string skillName = "Rigor";
    private static string skillDescription = "Rigor is a skill that skill that defines how much damage you will take from all different damage types";
    private static int price = 0;
    private static float skillLevel = 1f;
    private static float effect = 0f;
    public static Player playerInstance;
    private Npc npcInstance;
    private GuiFunction gui;


    public Skill_Passive_General_Rigor() :
		base(id, skillName)
    {
    }

    public int getSkillID()
    {
        return id;
    }
    public string getSkillText()
    {
        return skillName;
    }

    public string getType()
    {
        return type;
    }

    public string getSkillDescription()
    {
        return skillDescription;
    }

    public string getSkillGroup()
    {
        return group;
    }

    public int getPrice()
    {
        return price;
    }

    public float getSkillLevel()
    {
        return skillLevel;
    }

    public float getEffect()
    {
        return effect;
    }

    public bool setSkillLevel(float change)
    {
        float oldSkillLevel = getSkillLevel();
        skillLevel += change;
        bool skillUp = false;
        if (Mathf.Floor(oldSkillLevel) < Mathf.Floor(skillLevel))
        {
            gui.newTextLine("Skill level in " + getSkillText() + " has increased to " + Mathf.Floor(skillLevel) + "!");
            skillUp = true;
        }
        if (skillLevel >= 100)
        {
            if (Mathf.Floor(oldSkillLevel) < 100)
            {
                gui.newTextLine(getSkillText() + " is surging!");
                playerInstance.player.setProtections(new float[15] { 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0, 0, 0 }, true);
            }
            skillLevel = 100;
        }
        else if (skillLevel >= 75)
        {
            if (Mathf.Floor(oldSkillLevel) < 75)
            {
                gui.newTextLine(getSkillText() + " has reached a new level!");
                playerInstance.player.setProtections(new float[15] { 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0, 0, 0 }, true);
            }
        }
        else if (skillLevel >= 50)
        {
            if (Mathf.Floor(oldSkillLevel) < 50)
            {
                gui.newTextLine(getSkillText() + " has reached a new level!");
                playerInstance.player.setProtections(new float[15] { 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0, 0, 0 }, true);
            }
        }
        else if (skillLevel >= 25)
        {
            if (Mathf.Floor(oldSkillLevel) < 25)
            {
                gui.newTextLine(getSkillText() + " has reached a new level!");
                playerInstance.player.setProtections(new float[15] { 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0, 0, 0 }, true);
            }
        }
        else
        {
            effect = 0f;
        }
        return skillUp;
    }
    public void setPlayerInstance(Player player, Npc npc)
    {
        playerInstance = player;
        npcInstance = npc;
    }

    public void setGuiInstance(GuiFunction guiIn, bool player)
    {
        if (player)
            gui = guiIn;
    }
}
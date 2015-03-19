﻿using UnityEngine;
using System.Collections;

public class Skill_Active_Lesser_Magic_Heal_Self : SkillEntity, Skill, HotbarAble, Castable, Spell
{

	private static int id = 20;
    private static string type = "active";
    private static string group = "lesser magic";
    private static string skillName = "Heal Self";
    private static string skillDescription = "Caster regains a small amount of Health. This spell does not require any reagents to cast.";
    private static int price = 0;
    private static float skillLevel = 1f;
    private static float effect = 3.5f;
    private static int inventoryID = 9999;
    private static int hotbarSlot;
    private static float castingCost = 30;
    private static float duration = 3f;
    private static float currentDuration = 2f;
    private static bool activated = false;
    private static string castMsg = "Heal Self";
    private static float gainPrCast = 1.0f;
    private static float cooldown = 15f;
    private static float currentCooldown = 0f;
    private static float castTime = 3;
    Texture texture;
    GameObject particle;
    private Player playerInstance;
    private Npc npcInstance;
    private GuiFunction gui;


    public Skill_Active_Lesser_Magic_Heal_Self() :
		base(id, skillName)
	{
        texture = Resources.Load("lessermagic_healself01", typeof(Texture)) as Texture;
        particle = Resources.Load("BeingHealedEffect", typeof(GameObject)) as GameObject;
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
        if (Mathf.Floor(oldSkillLevel) < Mathf.Floor(skillLevel))
        {
            gui.newTextLine("Skill level in " + getSkillText() + " has increased to " + Mathf.Floor(skillLevel)+"!");
            effect += 0.01f;
        }
        if (skillLevel >= 100)
        {
            if (Mathf.Floor(oldSkillLevel) < 100)
            {
                gui.newTextLine(getSkillText() + " is surging!");
                duration = 5;
            }
            skillLevel = 100;
            return false;
        }
        else if (skillLevel >= 75)
        {
            if (Mathf.Floor(oldSkillLevel) < 75)
            {
                gui.newTextLine(getSkillText() + " has reached a new level!");
            }
        }
        else if (skillLevel >= 50)
        {
            if (Mathf.Floor(oldSkillLevel) < 50)
            {
                gui.newTextLine(getSkillText() + " has reached a new level!");
                castTime = 4.50f;
                duration = 4f;
            }
        }
        else if (skillLevel >= 25)
        {
            if (Mathf.Floor(oldSkillLevel) < 25)
            {
                gui.newTextLine(getSkillText() + " has reached a new level!");
            } 
        }
        return true;
    }

    public void setHotbarSlot(int slot)
    {
        hotbarSlot = slot;
    }

    public int getHotbarSlot()
    {
        return hotbarSlot;
    }

    public int getInventoryID()
    {
        return inventoryID;
    }

    public void cast()
    {
        playerInstance.player.setHealth(0, effect, false, "");
        Instantiate(particle, playerInstance.playerObject.transform.position - (new Vector3(0, 1, 0)), playerInstance.playerObject.transform.rotation);
    }

    public void stopEffect()
    {
    }

    public float getCastingCost()
    {
        return castingCost;
    }

    public float getDuration()
    {
        return duration;
    }

    public bool getState()
    {
        return activated;
    }

    public float getCooldown()
    {
        return cooldown;
    }

    public float getCurrentCooldown()
    {
        return currentCooldown;
    }


    public void setState(bool state)
    {
        activated = state;
    }

    public string getCastMsg()
    {
        return castMsg;
    }


    public bool setCurrentCooldown(float cooldownChange)
    {
        if (cooldownChange == cooldown)
        {
            currentCooldown = cooldown;
            return false;
        }
        else
        {
            currentCooldown -= cooldownChange;
            if (currentCooldown <= 0)
            {
                currentCooldown = 0;
                return true;
            }
            else
            {
                return false;
            }
        }
    }


    public bool setCurrentDuration(float durationChange)
    {
        float oldCurrentDuration = currentDuration;
        currentDuration -= durationChange;
        if (currentDuration < Mathf.Floor(oldCurrentDuration))
        {
            cast();
        }
        if (currentDuration <= 0)
        {
            currentDuration = duration;
            stopEffect();
            return true;
        }
        else
        {
            return false;
        }
    }


    public float getGainPrCast()
    {
        return gainPrCast;
    }

    public void updateGainPrCast()
    {
        gainPrCast = 1.1f - (getSkillLevel()/100);
    }

    public Texture getIcon()
    {
        return texture;
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

    public float getCastTime()
    {
        return castTime;
    }
}


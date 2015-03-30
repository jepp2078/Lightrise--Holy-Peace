﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GuiFunction : MonoBehaviour {
    public RawImage hotbar0, hotbar1, hotbar2, hotbar3, hotbar4, hotbar5, hotbar6, hotbar7, hotbar8, hotbar9, activeSkill, activeWeapon, activeWeaponBg;
    public Text consoleText, castTime, name;
    public RawImage[] castBar = new RawImage[5];
    public Image health, stamina, mana;
    public Image[] skillCooldowns = new Image[10];
    Texture tempIcon;
    string tempMessageString, tempCastString;
    int hotbarIndex, lines = 0;
    bool nameCall = true, hotbarCall = false, activeSkillCall = false, activeWeaponCall = false, drawWeaponCall = false, textCall = false, clearText = false, castTimeCall = false, casting = false;
    Color alpha, bg;
    public Player player;

    void Start()
    {
        hideCastBar();
    }
    public void setHotbarIcon(int hotbarSlot, Texture icon, bool transparent)
    {
        hotbarIndex = hotbarSlot;
        tempIcon = icon;
        if (transparent)
        {
            alpha = new Color(255, 255, 255, 0);
        }
        else
        {
            alpha = new Color(255, 255, 255, 255);
        }
        hotbarCall = true;
        OnGUI();
    }

    public void setActiveSkillIcon(Texture icon, bool transparent)
    {
        tempIcon = icon;
        if (transparent)
        {
            alpha = new Color(255, 255, 255, 0);
        }
        else
        {
            alpha = new Color(255, 255, 255, 255);
        }
        activeSkillCall = true;
        OnGUI();
    }

    public void setActiveWeaponIcon(Texture icon, bool sheathed, bool sheathCallIn)
    {
        if (icon != null && sheathCallIn == false) 
        {
            drawWeaponCall = true;
            tempIcon = icon;
        }
        else
        {
            drawWeaponCall = false;
        }

        if (sheathed)
        {
            alpha = new Color(255, 255, 255, 255);
            bg = new Color(58f / 14790 * 58, 58f / 14790 * 58, 58f / 14790 * 58);
        }
        else
        {
            alpha = new Color(255, 255, 255, 255);
            bg = new Color(58f / 14790 * 58, 255f / 65025f * 255, 58f / 14790 * 58);
        }
        activeWeaponCall = true;
        OnGUI();
    }

    public void newTextLine(string input)
    {
        lines++;
        if (lines > 10)
        {
            clearText = true;
            lines = 1;
        }
        tempMessageString = "[" + DateTime.Now.ToString("hh:mm:ss") + "] " + input + "\n";
        textCall = true;
        OnGUI();


    }

    public void setCastTime(float input)
    {
        if (casting == false)
        {
            showCastBar();
            casting = true;
        }
        if (input != 0) 
        { 
        TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(input));

        tempCastString = string.Format("{0:D2}:{1:D3}s",
                        t.Seconds,
                        t.Milliseconds);
        }
        else
        {
            tempCastString = "";
            hideCastBar();
            casting = false;
        }
        castTimeCall = true;
        OnGUI();
    }
    public void showCastBar()
    {
        for (int i = 0; i < 5; i++)
        {
            castBar[i].color = new Color(255, 255, 255, 255);
        }
    }
    public void hideCastBar()
    {
        for (int i = 0; i < 5; i++)
        {
            castBar[i].color = new Color(255, 255, 255, 0);
        }
    }

    public void setSkillCooldown(float current, float total, int slot)
    {
        skillCooldowns[slot].fillAmount = current / total;
    }

    void OnGUI()
    {
        if (hotbarCall)
        {
            switch (hotbarIndex)
            {
                case 0: hotbar0.texture = tempIcon; hotbar0.color = alpha; break;
                case 1: hotbar1.texture = tempIcon; hotbar1.color = alpha; break;
                case 2: hotbar2.texture = tempIcon; hotbar2.color = alpha; break;
                case 3: hotbar3.texture = tempIcon; hotbar3.color = alpha; break;
                case 4: hotbar4.texture = tempIcon; hotbar4.color = alpha; break;
                case 5: hotbar5.texture = tempIcon; hotbar5.color = alpha; break;
                case 6: hotbar6.texture = tempIcon; hotbar6.color = alpha; break;
                case 7: hotbar7.texture = tempIcon; hotbar7.color = alpha; break;
                case 8: hotbar8.texture = tempIcon; hotbar8.color = alpha; break;
                case 9: hotbar9.texture = tempIcon; hotbar9.color = alpha; break;
            }
            hotbarCall = false;
        }
        else if(activeSkillCall)
        {
            activeSkill.texture = tempIcon; 
            activeSkill.color = alpha;
            activeSkillCall = false;
        }
        else if (activeWeaponCall)
        {
            activeWeaponBg.color = bg;
            if (drawWeaponCall == true)
            {
                activeWeapon.texture = tempIcon;
            }
            activeWeapon.color = alpha;
            activeWeaponCall = false;
        }
        else if (textCall)
        {
            if (clearText)
            {
                consoleText.text = "";
                clearText = false;
            }
            consoleText.text += tempMessageString;
            textCall = false;
        }
        else if (castTimeCall)
        {
            castTime.text = tempCastString;
            textCall = false;
        }
        else if (nameCall)
        {
            name.text = player.player.getName();
            nameCall = false;
        }

        health.fillAmount = player.player.getTempHealthFloat() / player.player.getHealthFloat();
        stamina.fillAmount = player.player.getTempStaminaFloat() / player.player.getStaminaFloat();
        mana.fillAmount = player.player.getTempManaFloat() / player.player.getManaFloat();
    }

}

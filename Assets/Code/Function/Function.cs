﻿using UnityEngine;
using System.Collections;

public class Function : MonoBehaviour {

    public static float getWeaponDamage(PlayerEntity player){
	    Item weapon = ((PlayerObject) player).getWeapon();
	    if(weapon is Item){
		    Weapon tempWeapon = (Weapon) weapon;
		    string weaponType = tempWeapon.getType();
		    float damageMod = ((PlayerObject) player).getWeaponMod(weaponType);
		    return tempWeapon.getDamage() + damageMod;
        }
        else
        {
            return 0;
        }
    }

    public static string equipItem(int invSlot)
    {
        return Player.player.equip(invSlot);
    }

    public static string dequipItem(int equipSlot)
    {
        return Player.player.dequip(equipSlot);
    }

    public static string showInventory(){
		string inventory ="Inventory:\n";
		PlayerEntity player = Player.player;
		if(player is PlayerObject){
			PlayerObject temptempPlayer = (PlayerObject)player;
			try{
				for(int i = 0 ; i<temptempPlayer.getInvSize() ; i++){
					inventory += temptempPlayer.getInventory(i).getItemText()+"\n";
				}
				return inventory;
			}catch{
				return inventory;
			}
		}
		return null;
	}

    public static string showEquipment(){
		string equipment = "Equipment: \n";
		PlayerEntity player = Player.player;
		if(player is PlayerObject){
			PlayerObject temptempPlayer = (PlayerObject)player;
			for(int i=0;i<10;i++){
				switch(i){
				case 0:
					if(temptempPlayer.getEquipment(i)==null){
						equipment += "Head: None\n";
					}else{
					equipment +="Head: " + temptempPlayer.getEquipment(i).getItemText()+"\n";
					}
				break;
				case 1:
					if(temptempPlayer.getEquipment(i)==null){
						equipment += "Torso: None\n";
					}else{
					equipment +="Torso: " + temptempPlayer.getEquipment(i).getItemText()+"\n";
					}
				break;
				case 2:
					if(temptempPlayer.getEquipment(i)==null){
						equipment += "Arms: None\n";
					}else{
					equipment +="Arms: " + temptempPlayer.getEquipment(i).getItemText()+"\n";
					}
				break;
				case 3:
					if(temptempPlayer.getEquipment(i)==null){
						equipment += "Hands: None\n";
					}else{
					equipment +="Hands: " + temptempPlayer.getEquipment(i).getItemText()+"\n";
					}
				break;
				case 4:
					if(temptempPlayer.getEquipment(i)==null){
						equipment += "Legs: None\n";
					}else{
					equipment +="Legs: " + temptempPlayer.getEquipment(i).getItemText()+"\n";
					}
				break;
				case 5:
					if(temptempPlayer.getEquipment(i)==null){
						equipment += "Feet: None\n";
					}else{
					equipment +="Feet: " + temptempPlayer.getEquipment(i).getItemText()+"\n";
					}
				break;
				case 6:
					if(temptempPlayer.getEquipment(i)==null){
						equipment += "Main Hand: None\n";
					}else{
					equipment +="Main Hand: " + temptempPlayer.getEquipment(i).getItemText()+"\n";
					}
				break;
				case 7:
					if(temptempPlayer.getEquipment(i)==null){
						equipment += "Off Hand: None\n";
					}else{
					equipment +="Off Hand: " + temptempPlayer.getEquipment(i).getItemText()+"\n";
					}
				break;
				case 8:
					if(temptempPlayer.getEquipment(i)==null){
						equipment += "Neck: None\n";
					}else{
					equipment +="Neck: " + temptempPlayer.getEquipment(i).getItemText()+"\n";
					}
				break;
				case 9:
					if(temptempPlayer.getEquipment(i)==null){
						equipment += "Fingers: None\n";
					}else{
					equipment +="Fingers: " + temptempPlayer.getEquipment(i).getItemText()+"\n";
					}
				break;
				
				}
			}
		}
		return equipment;
	}

    public static string status(){
		PlayerEntity player = Player.player;
		if(player is PlayerObject){
			PlayerObject temptempPlayer = (PlayerObject)player;
			string tempStats = temptempPlayer.getStatus();
			return tempStats;
		}
		return null;
	}

   
}

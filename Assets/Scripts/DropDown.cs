/*

   .-------.                             .--.    .-------.     .--.            .--.     .--.
   |       |--.--.--------.-----.-----.--|  |    |_     _|--.--|  |_.-----.----|__|---.-|  |-----.
   |   -   |_   _|        |  _  |     |  _  |      |   | |  |  |   _|  _  |   _|  |  _  |  |__ --|
   |_______|__.__|__|__|__|_____|__|__|_____|      |___| |_____|____|_____|__| |__|___._|__|_____|
   © 2019 OXMOND / www.oxmond.com

           ## Edited by Mitchel Smith for the purpose of implementation into Gnome Tycoon ##
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropDown : MonoBehaviour
{
    [SerializeField] private MainMenuScript menuSys;

    public void HandleInputData(int val)
    {
        switch (val)
        {
            case 0:
                menuSys.adjective = "Amazing";
                break;
            case 1:
                menuSys.adjective = "Awesome";
                break;
            case 2:
                menuSys.adjective = "Charmful";
                break;
            case 3:
                menuSys.adjective = "Cheap";
                break;
            case 4:
                menuSys.adjective = "Elegant";
                break;
            case 5:
                menuSys.adjective = "Excellent";
                break;
            case 6:
                menuSys.adjective = "Fantastic";
                break;
            case 7:
                menuSys.adjective = "Happy";
                break;            
            case 8:
                menuSys.adjective = "Inexpensive";
                break;
            case 9:
                menuSys.adjective = "Jolly";
                break;
            case 10:
                menuSys.adjective = "Jovial";
                break;
            case 11:
                menuSys.adjective = "Magical";
                break;
            case 12:
                menuSys.adjective = "Magnificent";
                break;
            case 13:
                menuSys.adjective = "Majestic";
                break;            
            case 14:
                menuSys.adjective = "Nice";
                break;
            case 15:
                menuSys.adjective = "Outstanding";
                break;
            case 16:
                menuSys.adjective = "Smooth";
                break;
            case 17:
                menuSys.adjective = "Solid";
                break;
            case 18:
                menuSys.adjective = "Sturdy";
                break;
            case 19:
                menuSys.adjective = "Stylish";
                break;
            case 20:
                menuSys.adjective = "Whimsical";
                break;            
            case 21:
                menuSys.adjective = "Wonderful";
                break;
        }

        Debug.Log(menuSys.adjective);
    }  
}
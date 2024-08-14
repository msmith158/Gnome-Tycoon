using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroSequence : MonoBehaviour
{
    [Header("Values")]
    private bool skipped; // Change this setting in the DDOL Manager
    [SerializeField] private float fadeTime;
    private int spriteState = 0;
    private float timeRemaining = 0;

    [Header("Object References > General")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject spriteHolder;
    [SerializeField] private Image blockingImage; // This is so that the player can't play the game during the intro
    [SerializeField] private SwitchPanels switchPanels;
    [SerializeField] private TextMeshProUGUI dialogueHeader;
    [SerializeField] private TextMeshProUGUI dialogueBody;
    private DDOLManager ddolManager;

    [Header("Object References > Panel Switch")]
    [SerializeField] private GameObject controlPanel;
    [SerializeField] private GameObject controlPanelDismissalPoint;
    [SerializeField] private GameObject controlPanelActivationPoint;
    [SerializeField] private GameObject upgradesPanel;
    [SerializeField] private GameObject upgradesPanelDismissalPoint;
    [SerializeField] private GameObject upgradesPanelActivationPoint;    
    [SerializeField] private GameObject prestigePanel;
    [SerializeField] private GameObject prestigePanelDismissalPoint;
    [SerializeField] private GameObject prestigePanelActivationPoint;    
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private GameObject notificationPanelDismissalPoint;
    [SerializeField] private GameObject notificationPanelActivationPoint;
    [SerializeField] private GameObject mainScreens;
    [SerializeField] private GameObject mainScreensDismissalPoint;
    [SerializeField] private GameObject mainScreensActivationPoint;
    [SerializeField] private GameObject gnomeShopButton;
    [SerializeField] private GameObject gnomeShopButtonDismissalPoint;
    [SerializeField] private GameObject gnomeShopButtonActivationPoint;

    [Header("Object References > Sprites")]
    [SerializeField] private Image neutralArmUpSprite;
    [SerializeField] private Image neutralArmDownSprite;
    [SerializeField] private Image nervousSprite;
    [SerializeField] private Image unhappySprite;
    [SerializeField] private Image thinkingSprite;

    public void ProgressIntroStates()
    {
        ddolManager = GameObject.Find("ddolManager").GetComponent<DDOLManager>();
        skipped = ddolManager.introSkipped;

        switch (skipped)
        {
            case false:
                spriteState++;

                // Huge list of states for the intro sequence. This works by just calling this method, which will progress the state (thank God for switch statements).
                switch (spriteState)
                {
                    case 1:
                        // Set everything up the first time
                        dialoguePanel.SetActive(true);
                        spriteHolder.SetActive(true);
                        dialogueHeader.enabled = true;
                        dialogueBody.enabled = true;
                        neutralArmUpSprite.enabled = false;
                        neutralArmDownSprite.enabled = false;
                        nervousSprite.enabled = false;
                        unhappySprite.enabled = false;
                        thinkingSprite.enabled = false;
                        blockingImage.enabled = true;
                        controlPanel.transform.position = controlPanelDismissalPoint.transform.position;
                        mainScreens.transform.position = mainScreensDismissalPoint.transform.position;
                        gnomeShopButton.transform.position = gnomeShopButtonDismissalPoint.transform.position;

                        SwitchSprites(null, neutralArmDownSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Welcome! You must be our new manager for the factory.";
                        break;
                    case 2:
                        SwitchSprites(neutralArmDownSprite, neutralArmUpSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Thanks for agreeing to help manage the factory. We really needed an extra set of hands here.";
                        break;
                    case 3:
                        SwitchSprites(neutralArmUpSprite, thinkingSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Things have been running a bit slow here lately. Not much business has been coming around here for a while.";
                        break;
                    case 4:
                        SwitchSprites(thinkingSprite, thinkingSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "The big guys moved in not long ago, you see, and they've been taking up all the business.";
                        break;
                    case 5:
                        SwitchSprites(thinkingSprite, nervousSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "They've been harassing small honest businesses like ours so they can stomp the competition.";
                        break;
                    case 6:
                        SwitchSprites(null, nervousSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Old timers like me and my wife don't stand a chance against them corpo fellas and their deep pockets.";
                        break;
                    case 7:
                        SwitchSprites(nervousSprite, neutralArmDownSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "But that's why we got you. We reckon you'll do a much better job in keeping the business afloat.";
                        break;
                    case 8:
                        SwitchSprites(neutralArmDownSprite, neutralArmUpSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "So let me take you through how to use all the equipment and we'll have you up and running in no-time.";
                        break;
                    case 9:
                        dialoguePanel.SetActive(false);
                        switchPanels.SetActivationValuesThroughScript(controlPanel, 0.5f, controlPanelActivationPoint);
                        switchPanels.ExecuteSmooth(2);
                        StartCoroutine(DelayedAction(1f));
                        break;
                    case 10:
                        SwitchSprites(null, neutralArmDownSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "The manufacture button dispenses the material onto your production line, allowing it to go through the production line and make a gnome.";
                        break;
                    case 11:
                        SwitchSprites(null, neutralArmDownSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Your two big side buttons are the upgrade and prestige buttons. They call the upgrade and prestige menus, which I'll show you in a sec.";
                        break;
                    case 12:
                        SwitchSprites(null, neutralArmDownSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Your two small side buttons are the notification and mode buttons. I'll take you through those as well shortly.";
                        break;
                    case 13:
                        SwitchSprites(neutralArmDownSprite, neutralArmUpSprite, false);
                        spriteHolder.transform.localScale = new Vector3(-1, spriteHolder.transform.localScale.y, spriteHolder.transform.localScale.z);
                        dialoguePanel.SetActive(false);
                        switchPanels.SetDismissalValuesThroughScript(controlPanel, 0.5f, controlPanelDismissalPoint);
                        switchPanels.SetActivationValuesThroughScript(upgradesPanel, 0.5f, upgradesPanelActivationPoint);
                        switchPanels.ExecuteSmooth(0);
                        StartCoroutine(DelayedAction(1f));
                        break;
                    case 14:
                        SwitchSprites(neutralArmDownSprite, neutralArmUpSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Your standard upgrades cost money, which you earn by producing gnomes, whereas permanent upgrades cost Gnome Coins.";
                        break;
                    case 15:
                        SwitchSprites(neutralArmUpSprite, nervousSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Gnome Coins are a special currency that you can't earn by manufacturing gnomes. You'll have to pay with real money for those.";
                        break;
                    case 16:
                        SwitchSprites(nervousSprite, thinkingSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Some upgrades are also limited, meaning you can only buy a certain amount. These upgrades have bars to show how many are left.";
                        break;
                    case 17:
                        SwitchSprites(thinkingSprite, neutralArmUpSprite, false);
                        spriteHolder.transform.localScale = new Vector3(1, spriteHolder.transform.localScale.y, spriteHolder.transform.localScale.z);
                        dialoguePanel.SetActive(false);
                        switchPanels.SetDismissalValuesThroughScript(upgradesPanel, 0.5f, upgradesPanelDismissalPoint);
                        switchPanels.SetActivationValuesThroughScript(prestigePanel, 0.5f, prestigePanelActivationPoint);
                        switchPanels.ExecuteSmooth(0);
                        StartCoroutine(DelayedAction(1f));
                        break;
                    case 18:
                        SwitchSprites(neutralArmDownSprite, thinkingSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Keep in mind that while your gnomes become more valuable, your machines also get a bit slower because of the added weight.";
                        break;
                    case 19:
                        SwitchSprites(thinkingSprite, nervousSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Your upgrades and profit are also reset when you prestige, so be careful not to rush into prestiging too quickly.";
                        break;
                    case 20:
                        SwitchSprites(nervousSprite, neutralArmUpSprite, false);
                        spriteHolder.transform.localScale = new Vector3(-1, spriteHolder.transform.localScale.y, spriteHolder.transform.localScale.z);
                        dialoguePanel.SetActive(false);
                        switchPanels.SetDismissalValuesThroughScript(prestigePanel, 0.5f, prestigePanelDismissalPoint);
                        switchPanels.SetActivationValuesThroughScript(notificationPanel, 0.5f, notificationPanelActivationPoint);
                        switchPanels.ExecuteSmooth(0);
                        StartCoroutine(DelayedAction(1f));
                        break;
                    case 21:
                        SwitchSprites(null, null, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Whenever you receive a notification, your main screen (the one that tracks your profit) will flash green and turn on a little bell.";
                        break;
                    case 22:
                        SwitchSprites(neutralArmDownSprite, neutralArmUpSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Sometimes we'll get letters in the mail or offers from companies from Gnome Coins, so keep an eye out for those.";
                        break;
                    case 23:
                        SwitchSprites(null, null, false);
                        spriteHolder.transform.localScale = new Vector3(1, spriteHolder.transform.localScale.y, spriteHolder.transform.localScale.z);
                        dialoguePanel.SetActive(false);
                        switchPanels.SetDismissalValuesThroughScript(notificationPanel, 0.5f, notificationPanelDismissalPoint);
                        switchPanels.ExecuteSmooth(1);
                        StartCoroutine(DelayedAction(1f));
                        break;
                    case 24:
                        SwitchSprites(thinkingSprite, nervousSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "I've heard it's connected to the prestige panel in some way, but that's all I know. I'm sure you'll figure it out.";
                        break;
                    case 25:
                        SwitchSprites(nervousSprite, thinkingSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Now what else have I missed?";
                        break;
                    case 26:
                        SwitchSprites(thinkingSprite, neutralArmDownSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Ah, of course, the stat tracker screens! I mentioned it before but my rickety old self forgot to show you.";
                        break;
                    case 27:
                        SwitchSprites(neutralArmDownSprite, neutralArmUpSprite, false);
                        dialoguePanel.SetActive(false);
                        switchPanels.SetActivationValuesThroughScript(mainScreens, 0.5f, mainScreensActivationPoint);
                        switchPanels.ExecuteSmooth(2);
                        StartCoroutine(DelayedAction(1f));
                        break;
                    case 28:
                        SwitchSprites(neutralArmDownSprite, neutralArmUpSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "The only thing you really need to know is the passive income, which is calculated depending on the amount of production lines you've automated.";
                        break;
                    case 29:
                        SwitchSprites(null, null, false);
                        dialoguePanel.SetActive(false);
                        switchPanels.SetDismissalValuesThroughScript(mainScreens, 0.5f, mainScreensDismissalPoint);
                        switchPanels.ExecuteSmooth(1);
                        StartCoroutine(DelayedAction(1f));
                        break;
                    case 30:
                        SwitchSprites(neutralArmDownSprite, neutralArmUpSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "And hey, I'm sure with your help, we'll be able to teach those corpo fellas a thing about picking on hardworking guys like us!";
                        break;
                    case 31:
                        SwitchSprites(neutralArmUpSprite, nervousSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Good luck, son. Go show them who's the real boss around here.";
                        break;
                    case 32:
                        // Finish the intro sequence here
                        dialogueHeader.enabled = false;
                        dialogueBody.enabled = false;
                        neutralArmUpSprite.enabled = false;
                        neutralArmDownSprite.enabled = false;
                        nervousSprite.enabled = false;
                        unhappySprite.enabled = false;
                        thinkingSprite.enabled = false;
                        dialoguePanel.SetActive(false);
                        spriteHolder.SetActive(false);

                        mainScreens.GetComponent<SwitchPanels>().SetActivationValuesThroughScript(mainScreens, 0.5f, mainScreensActivationPoint);
                        mainScreens.GetComponent<SwitchPanels>().ExecuteSmooth(2);
                        controlPanel.GetComponent<SwitchPanels>().SetActivationValuesThroughScript(controlPanel, 0.5f, controlPanelActivationPoint);
                        controlPanel.GetComponent<SwitchPanels>().ExecuteSmooth(2);
                        switchPanels.GetComponent<SwitchPanels>().SetActivationValuesThroughScript(gnomeShopButton, 0.5f, gnomeShopButtonActivationPoint);
                        switchPanels.GetComponent<SwitchPanels>().ExecuteSmooth(2);

                        ddolManager.introSkipped = true;
                        blockingImage.enabled = false;
                        break;
                }
                break;
            case true:
                break;
        }
    }

    private void SwitchSprites(Image previousSprite, Image nextSprite, bool fadeIn)
    {
        switch (fadeIn)
        {
            // ### DEV NOTE: PLEASE ADD FADE IN FUNCTION TO THIS LATER ###
            case true:
                if (previousSprite != null)
                {
                    previousSprite.color = new Color(previousSprite.color.r, previousSprite.color.g, previousSprite.color.b, 1);
                    previousSprite.canvasRenderer.SetAlpha(224f);
                    previousSprite.CrossFadeAlpha(0f, fadeTime, false);
                    previousSprite.enabled = false;
                }
                if (nextSprite != null)
                {
                    nextSprite.color = new Color(nextSprite.color.r, nextSprite.color.g, nextSprite.color.b, 0);
                    nextSprite.canvasRenderer.SetAlpha(0.01f);
                    nextSprite.CrossFadeAlpha(1f, fadeTime, false);
                    nextSprite.enabled = true;
                }
                break;
            case false:
                if (previousSprite != null)
                {
                    previousSprite.enabled = false;
                }
                if (nextSprite != null)
                {
                    nextSprite.enabled = true;
                }
                break;
        }
    }

    private IEnumerator DelayedAction(float time)
    {
        timeRemaining = time;
        for (int i = 0; i < timeRemaining; timeRemaining -= Time.deltaTime)
        {
            Debug.Log(timeRemaining);
            yield return null;
        }
        switch (spriteState)
        {
            case 9:
                SwitchSprites(neutralArmUpSprite, neutralArmDownSprite, false);
                dialoguePanel.SetActive(true);
                dialogueHeader.text = "Pop";
                dialogueBody.text = "This here is your control panel. This is where all of your controls are for controlling the factory.";
                break;
            case 13:
                SwitchSprites(neutralArmUpSprite, neutralArmDownSprite, false);
                dialoguePanel.SetActive(true);
                dialogueHeader.text = "Pop";
                dialogueBody.text = "This is your upgrade panel. Here you can buy upgrades to increase the production value, speed, efficiency, et cetera.";
                break;
            case 17:
                SwitchSprites(neutralArmUpSprite, neutralArmDownSprite, false);
                dialoguePanel.SetActive(true);
                dialogueHeader.text = "Pop";
                dialogueBody.text = "This is your prestige panel. By prestiging, you're upgrading your factory equipment to allow it to handle more valuable materials.";
                break;
            case 20:
                SwitchSprites(neutralArmUpSprite, neutralArmDownSprite, false);
                dialoguePanel.SetActive(true);
                dialogueHeader.text = "Pop";
                dialogueBody.text = "This is your notification panel. Whenever the system wants to tell you something, it'll be sent here.";
                break;
            case 23:
                SwitchSprites(neutralArmUpSprite, thinkingSprite, false);
                dialoguePanel.SetActive(true);
                dialogueHeader.text = "Pop";
                dialogueBody.text = "As for the mode button, well, we could never really figure out how to get that working.";
                break;
            case 27:
                SwitchSprites(neutralArmUpSprite, neutralArmDownSprite, false);
                dialoguePanel.SetActive(true);
                dialogueHeader.text = "Pop";
                dialogueBody.text = "These screens show you your profit, passive income, Gnome Coins and prestige level.";
                break;
            case 29:
                SwitchSprites(neutralArmUpSprite, neutralArmDownSprite, false);
                dialoguePanel.SetActive(true);
                dialogueHeader.text = "Pop";
                dialogueBody.text = "Well, I think that's about everything you need to know to start helping us run this place.";
                break;
        }
    }
}
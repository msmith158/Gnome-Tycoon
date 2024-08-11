using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroSequence : MonoBehaviour
{
    public bool skipped = false;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject spriteHolder;
    [SerializeField] private Image blockingImage; // This is so that the player can't play the game during the intro.
    [SerializeField] private SwitchPanels switchPanels;
    [SerializeField] private TextMeshProUGUI dialogueHeader;
    [SerializeField] private TextMeshProUGUI dialogueBody;
    [SerializeField] private Image neutralArmUpSprite;
    [SerializeField] private Image neutralArmDownSprite;
    [SerializeField] private Image nervousSprite;
    [SerializeField] private Image unhappySprite;
    [SerializeField] private Image thinkingSprite;
    [SerializeField] private float fadeTime;
    private int spriteState = 0;
    private float timeRemaining = 0;

    public void ProgressIntroStates()
    {
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
                        //switchPanels.

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
                        dialogueBody.text = "Things have been running a bit slow here lately. Not much business has been coming 'round here for a while.";
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
                        StartCoroutine(DelayTimer(0.5f));
                        SwitchSprites(neutralArmDownSprite, neutralArmUpSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "So let me take you through how to use all the equipment and we'll have you up and running in no-time.";
                        break;
                }
                break;
            case true:
                Debug.Log("Intro sequence skipped!");
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

    private IEnumerator DelayTimer(float time)
    {
        for (int i = 0; i < timeRemaining; timeRemaining -= Time.deltaTime)
        {
            Debug.Log(timeRemaining);
            yield return null;
        }
    }
}

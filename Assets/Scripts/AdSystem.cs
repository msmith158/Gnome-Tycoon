using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class AdSystem : MonoBehaviour
{
    [Header("Values")] 
    [SerializeField] private float delayUntilBlackScreen;
    [SerializeField] private float delayUntilAd;
    [SerializeField] private float delayAfterAd;
    [SerializeField] private int chanceOfGnome;
    [SerializeField] private bool allowForAdSkip = true;
    [SerializeField] private int secondsToSkip;
    [SerializeField] private int secondsToSkipBlank;
    [SerializeField] private int coinsToReward;
    [SerializeField] private int blankAdTime;
    private bool endTrigger = false;
    private bool adSkipped = false;

    [Header("Object References")]
    [SerializeField] private Image blackScreen;
    [SerializeField] private List<VideoClip> videos = new List<VideoClip>();
    [SerializeField] private VideoClip secretVideo;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RawImage rawImage;
    [SerializeField] private Button adSkipButton;
    [SerializeField] private TextMeshProUGUI ageDenialText;
    [SerializeField] private Image ageDenialQrImage;
    [SerializeField] private List<AudioSource> sourcesToStop = new List<AudioSource>();
    private GnomeCoinSystem coinSys;
    private MainMenuScript menuSys;

    private void OnEnable()
    {
        coinSys = GameObject.Find("ddolManager").GetComponent<GnomeCoinSystem>();
        menuSys = GameObject.Find("mainMenuSystemHandler").GetComponent<MainMenuScript>();
    }

    public void PlayFakeAd() { StartCoroutine(PlayAd()); }
    public void SkipAd() { StopCoroutine(PlayAd()); adSkipped = true; StartCoroutine(EndVideo()); }

    private void GiveReward()
    {
        coinSys.AddCoins(coinsToReward);
    }

    private IEnumerator PlayAd()
    {
        foreach (AudioSource sources in sourcesToStop)
        {
            sources.Pause();
        }
        yield return new WaitForSeconds(delayUntilBlackScreen);
        blackScreen.enabled = true;
        yield return new WaitForSeconds(delayUntilAd);

        switch (menuSys.isOver13)
        {
            case true:
                // Pick from a random selection of videos
                int chance = Random.Range(0, chanceOfGnome);
                Debug.Log(chance);
                switch (chance)
                {
                    case 0:
                        videoPlayer.clip = secretVideo;
                        break;
                    case >0:
                        int videoSelection = Random.Range(0, videos.Count);
                        videoPlayer.clip = videos[videoSelection];
                        break;
                }
                rawImage.enabled = true;
                videoPlayer.enabled = true;
                videoPlayer.Play();
        
                switch (allowForAdSkip)
                {
                    case true:
                        StartCoroutine(SkipAdFeature());
                        break;
                    case false:
                        break;
                }

                float duration = (float)videoPlayer.clip.length;
                Debug.Log(duration);
                for (int i = 0; i <= duration; duration -= Time.deltaTime)
                {
                    Debug.Log(duration);
                    switch (endTrigger)
                    {
                        case true:
                            duration = 0;
                            yield return null;
                            break;
                        case false:
                            yield return null;
                            break;
                    }
                }
                break;
            case false:
                ageDenialText.enabled = true;
                ageDenialQrImage.enabled = true;
                switch (allowForAdSkip)
                {
                    case true:
                        StartCoroutine(SkipAdFeature());
                        break;
                    case false:
                        break;
                }

                float ageDenialDuration = blankAdTime;
                Debug.Log(ageDenialDuration);
                for (int i = 0; i <= ageDenialDuration; ageDenialDuration -= Time.deltaTime)
                {
                    Debug.Log(ageDenialDuration);
                    switch (endTrigger)
                    {
                        case true:
                            duration = 0;
                            yield return null;
                            break;
                        case false:
                            yield return null;
                            break;
                    }
                }
                break;
        }
        
        switch (adSkipped)
        {
            case true:
                break;
            case false:
                StartCoroutine(EndVideo());
                adSkipped = false;
                break;
        }
    }

    private IEnumerator SkipAdFeature()
    {
        adSkipButton.gameObject.SetActive(true);
        adSkipButton.interactable = false;
        TextMeshProUGUI buttonText = adSkipButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        switch (menuSys.isOver13)
        {
            case true:
                int seconds = secondsToSkip;
                for (int i = 0; i < seconds; seconds--)
                {
                    buttonText.text = seconds.ToString();
                    yield return new WaitForSeconds(1);
                }
                break;
            case false:
                int blankSeconds = secondsToSkipBlank;
                for (int i = 0; i < blankSeconds; blankSeconds--)
                {
                    buttonText.text = blankSeconds.ToString();
                    yield return new WaitForSeconds(1);
                }
                break;
        }
        buttonText.text = "X";
        adSkipButton.interactable = true;
    }

    // This method is to be invoked via the ad skip button OnClick event
    private IEnumerator EndVideo()
    {
        endTrigger = true;
        switch (menuSys.isOver13)
        {
            case true:
                videoPlayer.Stop();
                rawImage.enabled = false;
                videoPlayer.enabled = false;
                break;
            case false:
                ageDenialText.enabled = false;
                ageDenialQrImage.enabled = false;
                break;
        }
        StopCoroutine(SkipAdFeature());
        adSkipButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(delayAfterAd);
        blackScreen.enabled = false;
        foreach (AudioSource sources in sourcesToStop)
        {
            sources.Play();
        }

        switch (menuSys.isOver13)
        {
            case true:
                GiveReward();
                break;
            case false:
                Debug.Log("No reward was given for the ad due to the player being under 13.");
                break;
        }
        yield return null;
        endTrigger = false;
    }
}
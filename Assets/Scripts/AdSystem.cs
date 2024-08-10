using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.EditorTools;
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
    [SerializeField] private int coinsToReward;

    [Header("Object References")]
    [SerializeField] private Image blackScreen;
    [SerializeField] private List<VideoClip> videos = new List<VideoClip>();
    [SerializeField] private VideoClip secretVideo;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RawImage rawImage;
    [SerializeField] private Button adSkipButton;
    [SerializeField] private List<AudioSource> sourcesToStop = new List<AudioSource>();
    private PrototypeGnomeCoinSystem coinSys;

    private void OnEnable()
    {
        coinSys = GameObject.Find("ddolManager").GetComponent<PrototypeGnomeCoinSystem>();
    }

    public void PlayFakeAd() { StartCoroutine(PlayAd()); }
    public void SkipAd() { StartCoroutine(EndVideo()); }

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
                Debug.Log(videoSelection);
                videoPlayer.clip = videos[videoSelection];
                break;
        }
        rawImage.enabled = true;
        videoPlayer.enabled = true;
        videoPlayer.Play();
        Debug.Log("Hehehehaw");
        
        switch (allowForAdSkip)
        {
            case true:
                Debug.Log("Haiya");
                StartCoroutine(SkipAdFeature());
                break;
            case false:
                Debug.Log("Bruh moment");
                break;
        }
        yield return new WaitForSeconds((float)videoPlayer.clip.length);
        StartCoroutine(EndVideo());
    }

    private IEnumerator SkipAdFeature()
    {
        adSkipButton.gameObject.SetActive(true);
        Debug.Log("Hello");
        adSkipButton.interactable = false;
        TextMeshProUGUI buttonText = adSkipButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        int seconds = secondsToSkip;
        for (int i = 0; i < seconds; seconds--)
        {
            buttonText.text = seconds.ToString();
            yield return new WaitForSeconds(1);
        }
        buttonText.text = "X";
        adSkipButton.interactable = true;
    }

    // This method is to be invoked via the ad skip button OnClick event
    private IEnumerator EndVideo()
    {
        StopCoroutine(PlayAd());
        StopCoroutine(SkipAdFeature());
        rawImage.enabled = false;
        videoPlayer.enabled = false;
        adSkipButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(delayAfterAd);
        blackScreen.enabled = false;
        foreach (AudioSource sources in sourcesToStop)
        {
            sources.Play();
        }
        GiveReward();
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PrototypeFinalPrestigeSystem : MonoBehaviour
{
    [Header("Values: Final Prestige")]
    [SerializeField] private float alarmDelayTime;
    [SerializeField] private float delayUntilCountdown;
    [SerializeField] private float countdownTimer;
    private Color emissiveColor = Color.red;

    [Header("Object References: Final Prestige")]
    [SerializeField] private PrototypeFactorySystem sys;
    [SerializeField] private Material emerLightMaterial;
    [SerializeField] private Light nukeLight;
    [SerializeField] private Light nukeAmbientLight;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Animation emerLightAnim;
    [SerializeField] private AudioSource switchOffSource;
    [SerializeField] private AudioSource generatorOffSource;
    [SerializeField] private AudioSource alarmSource;
    [SerializeField] private AudioSource clockSource;
    [SerializeField] private AudioSource nukeSource;
    [SerializeField] private AudioClip switchOff;
    [SerializeField] private AudioClip generatorOff;
    [SerializeField] private AudioClip alarm;
    [SerializeField] private AudioClip clock;
    [SerializeField] private AudioClip nuke;
    [SerializeField] private List<GameObject> uiToDisable = new List<GameObject>();
    [SerializeField] private List<GameObject> uiToEnable = new List<GameObject>();
    [SerializeField] private List<Light> lightsToTurnOff = new List<Light>();
    [SerializeField] private List<Light> emergencyLights = new List<Light>();

    public void StartNukeSequence()
    {
        StartCoroutine(NukeSequence());
    }

    public IEnumerator NukeSequence()
    {
        for (int i = 0; i < lightsToTurnOff.Count; i++)
        {
            lightsToTurnOff[i].enabled = false;
        }
        if (!switchOffSource.isPlaying)
        {
            switchOffSource.clip = switchOff;
            switchOffSource.Play();
        }
        if (!generatorOffSource.isPlaying)
        {
            generatorOffSource.clip = generatorOff;
            generatorOffSource.Play();
        }
        for (int i = 0; i < uiToDisable.Count; i++)
        {
            uiToDisable[i].SetActive(false);
        }
        Debug.Log("Hehehehaw");

        yield return new WaitForSeconds(alarmDelayTime);

        Debug.Log("Hi");
        if (!alarmSource.isPlaying)
        {
            alarmSource.clip = alarm;
            alarmSource.loop = true;
            alarmSource.Play();
        }
        for (int i = 0; i < emergencyLights.Count; i++)
        {
            emergencyLights[i].enabled = true;
            emergencyLights[i].GetComponent<Animator>().enabled = true;
        }

        emerLightMaterial.EnableKeyword("_EMISSION");

        yield return new WaitForSeconds(delayUntilCountdown);

        for (int i = 0; i < uiToEnable.Count; i++)
        {
            uiToEnable[i].SetActive(true);
        }
        clockSource.PlayOneShot(clock);

        for (float timer = countdownTimer; timer > 0; timer -= Time.deltaTime)
        {
            timerText.text = sys.RoundToNearestHundredth(timer).ToString("F2");
            yield return null;
        }
        timerText.enabled = false;
        Debug.Log("Lol");
        if (!nukeSource.isPlaying)
        {
            nukeSource.clip = nuke;
            nukeSource.loop = true;
            nukeSource.Play();
        }
        nukeLight.gameObject.SetActive(true);
        nukeAmbientLight.gameObject.SetActive(true);
        nukeLight.GetComponent<Animator>().enabled = true;
        nukeAmbientLight.GetComponent<Animator>().enabled = true;
        emerLightMaterial.DisableKeyword("_EMISSION");
        emerLightAnim.wrapMode = WrapMode.Once;
    }
}

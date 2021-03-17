using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CheatMenu : MonoBehaviour
{
    public PostProcessProfile profile;
    public StateMachineMonsterController monsterController;
    public float postExposureDefaultValue;
    public float postExposureIncrement;
    public Light playerLight;

    public Item[] items;
    public Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        if (monsterController == null)
        {
            Debug.LogWarning("Need to indicate monsterController in CheatMenu");
            monsterController = FindObjectOfType<StateMachineMonsterController>();
        }
    }

    private void OnDestroy()
    {
        ColorGrading grading = profile.GetSetting<ColorGrading>();
        grading.postExposure.value = postExposureDefaultValue;

        foreach (Item i in items)
        {
            i.pickedUp = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("lightUp"))
        {
            ColorGrading grading = profile.GetSetting<ColorGrading>();
            grading.postExposure.value += postExposureIncrement;

            Debug.Log("Exposure : " + grading.postExposure.value);
        }
        if (Input.GetButtonDown("lightDown"))
        {
            ColorGrading grading = profile.GetSetting<ColorGrading>();
            grading.postExposure.value -= postExposureIncrement;
            if (grading.postExposure.value < 0)
                grading.postExposure.value = 0;

            Debug.Log("Exposure : " + grading.postExposure.value);
        }

        if (Input.GetButtonDown("TogglePlayerLight"))
        {
            playerLight.enabled = !playerLight.enabled;

            Debug.Log("Player light " + playerLight.enabled);
        }

        if (Input.GetButtonDown("toggleInvisibility"))
        {
            //Change state of player for the IA
            if (monsterController == null)
            {
                Debug.LogWarning("Need to indicate monsterController in the CheatMenu");
                return;
            }
            monsterController.isPlayerVisible = !monsterController.isPlayerVisible;
            Debug.Log("Invisibility activated :" + !monsterController.isPlayerVisible);
        }

        if (Input.GetButtonDown("GetAllItems"))
        {
            foreach(Item i in items)
            {
                if (!i.pickedUp)
                {
                    i.pickedUp = true;
                    inventory.PickUp(i);
                }
            }
            Debug.Log("All objects picked up");
        }
    }

}

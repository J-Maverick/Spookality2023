using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class QuestHandler : MonoBehaviour
{
    public Torch[] flashlights;
    public GameObject[] questOnly;
    public GameObject[] pcOnly;

    public bool quest = false;

    [ContextMenu("Update Quest-PC Objects")]
    public void UpdateObjects() {
        foreach (Torch flashlight in flashlights) {
            flashlight.quest = quest;
        }
        foreach (GameObject questObject in questOnly) {
            questObject.SetActive(quest);
        }
        foreach (GameObject pcObject in pcOnly) {
            pcObject.SetActive(!quest);
        }
    }

}

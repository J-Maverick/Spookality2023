
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;


public enum PotatoMode {
    NORMAL = 0,
    POTATO = 1,
    TURBO_TATER = 2,
}

public class PotatoZone : UdonSharpBehaviour
{
    public HunterItems[] hunterItems;
    public Torch[] flashLights;
    public GameObject[] lamps;
    public PotatoMode potato = PotatoMode.NORMAL;
    public Toggle potatoToggle;
    public Toggle turboToggle;

    public void UpdateStateFromToggles() {
        switch (potato) {
            case PotatoMode.NORMAL:
                if (turboToggle.isOn) {
                    potatoToggle.isOn = true;
                    EnableTurboTater();
                }
                else if (potatoToggle.isOn) {
                    EnablePotato();
                }
                break;
            case PotatoMode.POTATO:
                if (!potatoToggle.isOn) {
                    turboToggle.isOn = false;
                    DisablePotato();
                }
                else if (turboToggle.isOn) {
                    EnableTurboTater();
                }
                break;
            case PotatoMode.TURBO_TATER:
                if (!potatoToggle.isOn) {
                    turboToggle.isOn = false;
                    DisableTurboTater();
                    DisablePotato();
                }
                else if (!turboToggle.isOn) {
                    DisableTurboTater();
                }
                break;
        }
    }

    public void EnablePotato() {
        foreach (HunterItems hunter in hunterItems) {
            hunter.EnablePotato();
        }
        if (potato == PotatoMode.NORMAL) {
            potato = PotatoMode.POTATO;
        }
    }

    public void DisablePotato() {
        if (potato == PotatoMode.TURBO_TATER) {
            DisableTurboTater();
        }
        foreach (HunterItems hunter in hunterItems) {
            hunter.DisablePotato();
        }
        potato = PotatoMode.NORMAL;
    }

    public void EnableTurboTater() {
        EnablePotato();
        foreach (Torch flashlight in flashLights) {
            flashlight.quest = true;
        }
        foreach (GameObject lamp in lamps) {
            lamp.SetActive(false);
        }
        potato = PotatoMode.TURBO_TATER;
    }

    public void DisableTurboTater() {
        foreach (Torch flashlight in flashLights) {
            flashlight.quest = false;
        }
        foreach (GameObject lamp in lamps) {
            lamp.SetActive(true);
        }
        potato = PotatoMode.POTATO;
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelUpButton : MonoBehaviour
{

    public TMP_Text weaponName;
    public TMP_Text weaponDescription;
    public Image weaponIcon;

    private Weapon assignedWepon;

    public void ActivateButton(Weapon weapon){
        weaponName.text = weapon.name;
        weaponDescription.text = weapon.stats[weapon.weaponLevel].description;
        weaponIcon.sprite = weapon.weaponImage;

        assignedWepon = weapon;

    }

    public void SelectUpgrade(){
        assignedWepon.LevelUp();
        UIController.Instance.levelUpPanelClose();


    }
}

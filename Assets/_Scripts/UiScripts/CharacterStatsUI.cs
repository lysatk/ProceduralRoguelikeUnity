using UnityEngine;
using UnityEngine.UI;

public class CharacterStatsUI : MonoBehaviour
{
    public HeroUnitBase player; // Reference to the player's stats script
    public Text pointsText; // Display for available points
    public Text hpText, armorText, speedText, dmgModText, cooldownText; // Displays for each stat
    public Button fullHealButton; // Button for full heal

    private int availablePoints = 5; // Total points available for the player to use

    private void Start()
    {
        player=FindObjectOfType<HeroUnitBase>();
        UpdateUI();
        // Disable the full heal button if the player is at max health
        fullHealButton.interactable = player.stats.CurrentHp < player.stats.MaxHp;
    }

    public void ModifyStat(string stat, int amount)
    {
        if (availablePoints <= 0 && amount > 0) return; // No points to use for upgrading

        switch (stat)
        {
            case "HP":
                player.stats.MaxHp += amount;
                break;
            case "Armor":
                player.stats.Armor += amount;
                break;
            case "Speed":
                player.stats.MovementSpeed += amount;
                break;
            case "DmgMod":
                player.stats.DmgModifier += amount;
                break;
            case "Cooldown":
                player.stats.CooldownModifier += amount;
                break;
        }

        availablePoints -= amount;
        UpdateUI();
    }

    public void FullHeal()
    {
        if (availablePoints <= 0) return; // No points to use for healing

        player.stats.CurrentHp = player.stats.MaxHp;
        availablePoints -= 1; // Full heal costs 1 point
        UpdateUI();
    }

    private void UpdateUI()
    {
        // Update the text fields with the current stats
        hpText.text = "HP: " + player.stats.MaxHp;
        armorText.text = "Armor: " + player.stats.Armor;
        speedText.text = "Speed: " + player.stats.MovementSpeed;
        dmgModText.text = "Damage Mod: " + player.stats.DmgModifier;
        cooldownText.text = "Cooldown: " + player.stats.CooldownModifier;

        // Update the points display
        pointsText.text = "Points: " + availablePoints;

        // Disable the full heal button if the player is at max health or no points left
        fullHealButton.interactable = player.stats.CurrentHp < player.stats.MaxHp && availablePoints > 0;
    }
}

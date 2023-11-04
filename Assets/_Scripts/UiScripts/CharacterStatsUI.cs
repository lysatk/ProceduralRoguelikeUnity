using UnityEngine;
using TMPro; // Add this namespace to use TextMeshPro
using UnityEngine.UI;

public class CharacterStatsUI : MonoBehaviour
{
    public HeroUnitBase player;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI hpText, armorText, speedText, dmgModText, cooldownText;
    public Button fullHealButton;

    private int availablePoints = 5;
    private const int maxPoints = 5; // Maximum points that the player can have

    private void OnEnable()
    {
        if (GameManager.Player != null)
        {
            player = GameManager.Player.GetComponent<HeroUnitBase>();
            if (player != null)
            {
                UpdateUI();
                fullHealButton.interactable = player.stats.CurrentHp < player.stats.MaxHp;
            }
            else
            {
                Debug.LogError("HeroUnitBase component not found on the player game object!");
            }
        }
        else
        {
            Debug.LogError("Player game object reference not set!");
        }
    }



    // Increase methods
    public void IncreaseHP()
    {
        if (availablePoints <= 0) return;
        player.stats.MaxHp++;
        availablePoints--;
        UpdateUI();
    }

    public void IncreaseArmor()
    {
        if (availablePoints <= 0) return;
        player.stats.Armor++;
        availablePoints--;
        UpdateUI();
    }

    public void IncreaseSpeed()
    {
        if (availablePoints <= 0) return;
        player.stats.MovementSpeed++;
        availablePoints--;
        UpdateUI();
    }

    public void IncreaseDmgMod()
    {
        if (availablePoints <= 0) return;
        player.stats.DmgModifier++;
        availablePoints--;
        UpdateUI();
    }

    public void IncreaseCooldown()
    {
        if (availablePoints <= 0) return;
        player.stats.CooldownModifier++;
        availablePoints--;
        UpdateUI();
    }

    public void DecreaseHP()
    {
        if (availablePoints == maxPoints || player.stats.MaxHp <= 1) return;
        player.stats.MaxHp--;
        availablePoints++;
        UpdateUI();
    }

    public void DecreaseArmor()
    {
        if (availablePoints == maxPoints || player.stats.Armor <= 1) return;
        player.stats.Armor--;
        availablePoints++;
        UpdateUI();
    }

    public void DecreaseSpeed()
    {
        if (availablePoints == maxPoints || player.stats.MovementSpeed <= 1) return;
        player.stats.MovementSpeed--;
        availablePoints++;
        UpdateUI();
    }

    public void DecreaseDmgMod()
    {
        if (availablePoints == maxPoints || player.stats.DmgModifier <= 1) return;
        player.stats.DmgModifier--;
        availablePoints++;
        UpdateUI();
    }

    public void DecreaseCooldown()
    {
        if (availablePoints == maxPoints || player.stats.CooldownModifier <= 1) return;
        player.stats.CooldownModifier--;
        availablePoints++;
        UpdateUI();
    }

    public void FullHeal()
    {
        if (availablePoints <= 0) return;

        player.stats.CurrentHp = player.stats.MaxHp;
        availablePoints--; // Full heal costs 1 point
        UpdateUI();
    }

    private void UpdateUI()
    {
        hpText.text = "HP: " + player.stats.MaxHp;
        armorText.text = "Armor: " + player.stats.Armor;
        speedText.text = "Speed: " + player.stats.MovementSpeed;
        dmgModText.text = "Damage Mod: " + player.stats.DmgModifier;
        cooldownText.text = "Cooldown: " + player.stats.CooldownModifier;
        pointsText.text = "Points: " + availablePoints;
        fullHealButton.interactable = player.stats.CurrentHp < player.stats.MaxHp && availablePoints > 0;
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }
    public void ShowUI()
    {
        gameObject.SetActive(true);
    }
}

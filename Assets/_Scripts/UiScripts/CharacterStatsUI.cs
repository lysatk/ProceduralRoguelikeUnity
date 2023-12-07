using UnityEngine;
using TMPro; // Add this namespace to use TextMeshPro
using UnityEngine.UI;
using static System.Net.WebRequestMethods;
using UnityEngine.UIElements;

public class CharacterStatsUI : MonoBehaviour
{
    public HeroUnitBase player;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI hpText, armorText, speedText, dmgModText, cooldownText;
    public UnityEngine.UI.Button fullHealButton;
    private TextMeshProUGUI fullhealButtonText;

    private int availablePoints = 5;
    private const int maxPoints = 5;

    private int initialMaxHp;
    private float initialArmor;
    private float initialSpeed;
    private float initialDmgMod;
    private float initialCooldown;

    private int initialHp;

    private bool isHealToggled = false;


    // Soft and Hard caps for each stat
    private const float hpSoftCap = 100f;
    private const float hpHardCap = 150f;
    private const float armorSoftCap = 5f;
    private const float armorHardCap = 10f;
    private const float dmgModSoftCap = 5f;
    private const float dmgModHardCap = 10f;
    private const float cooldownSoftCap = 0.5f;
    private const float cooldownHardCap = 0.3f;
    private const float speedSoftCap = 9f;
    private const float speedHardCap = 14f;

  
    private void OnEnable()
    {
        GameManager.gamePaused = true;
        fullhealButtonText = fullHealButton.GetComponentInChildren<TextMeshProUGUI>();
        availablePoints = 5;
        if (GameManager.Player != null)
        {
            player = GameManager.Player.GetComponent<HeroUnitBase>();
            if (player != null)
            {
                initialHp = player.stats.CurrentHp;
                initialMaxHp = player.stats.MaxHp;
                initialArmor = player.stats.Armor;
                initialSpeed = player.stats.MovementSpeed;
                initialDmgMod = player.stats.DmgModifier;
                initialCooldown = player.stats.CooldownModifier;

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


    #region Increase methods

    public void IncreaseHP()
    {
        if (availablePoints <= 0 || player.stats.MaxHp >= hpHardCap) return;
        player.stats.MaxHp = (int)Mathf.Min(player.stats.MaxHp + 5, hpHardCap);
        availablePoints--;
        UpdateUI();
    }

    public void IncreaseArmor()
    {
        if (availablePoints <= 0 || player.stats.Armor >= armorHardCap) return;
        float increment = player.stats.Armor < armorSoftCap ? 0.1f : 0.05f;
        player.stats.Armor = Mathf.Min(player.stats.Armor + increment, armorHardCap);
        availablePoints--;
        UpdateUI();
    }

    public void IncreaseSpeed()
    {
        if (availablePoints <= 0 || player.stats.MovementSpeed >= speedHardCap)
        {
            player.stats.MovementSpeed = 14f;
            return;
        }
        if (player.stats.MovementSpeed < speedSoftCap)
        {
            player.stats.MovementSpeed += 0.5f;
        }
        else 
        {
            player.stats.MovementSpeed += 0.25f;
        }
       
        availablePoints--;
        UpdateUI();
    }

    public void IncreaseDmgMod()
    {
        if (availablePoints <= 0 || player.stats.DmgModifier >= dmgModHardCap) return;
        player.stats.DmgModifier = Mathf.Min(player.stats.DmgModifier + 1, dmgModHardCap);
        availablePoints--;
        UpdateUI();
    }


    public void IncreaseCooldown()
    {
        if (availablePoints <= 0 || player.stats.CooldownModifier <= cooldownHardCap) return;
        float decrement = player.stats.CooldownModifier > cooldownSoftCap ? 0.05f : 0.025f;
        player.stats.CooldownModifier = Mathf.Max(player.stats.CooldownModifier - decrement, cooldownHardCap);
        availablePoints--;
        UpdateUI();
    }
    #endregion

    #region Decrease methods
    public void DecreaseHP()
    {
        if (availablePoints == maxPoints || player.stats.MaxHp <= initialMaxHp) return;
        player.stats.MaxHp = Mathf.Max(player.stats.MaxHp - 5, initialMaxHp);
        availablePoints++;
        UpdateUI();
    }

    public void DecreaseArmor()
    {
        if (availablePoints == maxPoints || player.stats.Armor <= initialArmor) return;
        float decrement = player.stats.Armor > armorSoftCap ? 0.1f : 0.05f;
        player.stats.Armor = Mathf.Max(player.stats.Armor - decrement, initialArmor);
        availablePoints++;
        UpdateUI();
    }

    public void DecreaseSpeed()
    {
        if (availablePoints == maxPoints || player.stats.MovementSpeed <= initialSpeed) return;
        if (player.stats.MovementSpeed > speedSoftCap)
        {
            player.stats.MovementSpeed -= 0.5f;
        }
        else
        {
            player.stats.MovementSpeed -= 0.25f;
        }
        availablePoints++;
        UpdateUI();
    }

    public void DecreaseDmgMod()
    {
        if (availablePoints == maxPoints || player.stats.DmgModifier <= initialDmgMod) return;
        player.stats.DmgModifier = Mathf.Max(player.stats.DmgModifier - 1, initialDmgMod);
        availablePoints++;
        UpdateUI();
    }

    public void DecreaseCooldown()
    {
        if (availablePoints == maxPoints || player.stats.CooldownModifier >= initialCooldown) return;
        float increment = player.stats.CooldownModifier <= cooldownSoftCap ? 0.05f : 0.025f;
        player.stats.CooldownModifier = Mathf.Min(player.stats.CooldownModifier + increment, initialCooldown);
        availablePoints++;
        UpdateUI();
    }
    #endregion

    private int hpBeforeHeal;
    public void FullHeal()
    {
        if (!isHealToggled && player.stats.CurrentHp < player.stats.MaxHp && availablePoints > 0)
        {
            hpBeforeHeal = player.stats.CurrentHp;
            player.stats.CurrentHp = player.stats.MaxHp;
            isHealToggled = true;
            fullhealButtonText.text = "Reset HP";
            availablePoints--;
        }
        else if (isHealToggled)
        {
            player.stats.CurrentHp = hpBeforeHeal;
            isHealToggled = false; fullhealButtonText.text = "Heal";
            availablePoints++;
        }
        UpdateUI();
    }
    private void UpdateUI()
    {
        hpText.text = $"HP: {player.stats.CurrentHp}/{player.stats.MaxHp}";
        armorText.text = "Armor: " + player.stats.Armor;

        if (player.stats.MovementSpeed >= 14)
            speedText.text = "Speed: " + player.stats.MovementSpeed +" (MAX)";
        else
        speedText.text = "Speed: " + player.stats.MovementSpeed;

        dmgModText.text = "Damage Mod: " + player.stats.DmgModifier;
        cooldownText.text = "Cooldown: " + player.stats.CooldownModifier;
        pointsText.text = "Points: " + availablePoints;

    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }
    public void ShowUI()
    {
        gameObject.SetActive(true);
    }

    public void ConfirmLevelUp()
    {
        GameManager.Instance.ConfirmLevelUpAndContinue();
    }


}

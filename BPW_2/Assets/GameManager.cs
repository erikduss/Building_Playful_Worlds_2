using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private GameObject Player;
    private EnemyController BossScript;

    private List<EnemyController> enemies = new List<EnemyController>();

    private int armorDamageAbsorbtion = 0;
    private int currentArmorTier = 0;
    private int armorAbsorbtionRate = 10; //Every tier 10% absorbtion gets added

    private int weaponEnchantment = 0;
    private int currentWeaponTier = 0;
    private int weaponEnchantingRate = 10;

    public Text chestInteraction;
    public Text informationText;

    private bool canOpenChest = false;

    private chestController currentChest;

    // Start is called before the first frame update
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public void LevelGenerationComplete(Vector3 spawnRoomPos, List<Room> rooms)
    {
        Vector3 spawnRoomPosition = spawnRoomPos;
        Player.transform.position = new Vector3(spawnRoomPosition.x + 10, spawnRoomPosition.y - 8, spawnRoomPosition.z);

        BossScript = GameObject.FindGameObjectWithTag("Boss").GetComponent<EnemyController>();


        foreach(GameObject item in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies.Add(item.GetComponent<EnemyController>());
        }
        giveKeysToEnemies(rooms);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && canOpenChest)
        {
            attempToOpenChest();
        }
    }

    public void showChestOpenText(bool enabled, chestController chest)
    {
        currentChest = chest;
        chestInteraction.gameObject.SetActive(enabled);
        canOpenChest = enabled;
    }

    public void attempToOpenChest()
    {
        if(Player.GetComponent<PlayerController>().amountOfKeys <= 0)
        {
            fadeText(2, informationText, "Not enough Keys!", Color.red);
        }
        else
        {
            currentChest.openThisChest();
            currentArmorTier++;
            currentWeaponTier++;
            Player.GetComponent<PlayerController>().amountOfKeys--;
            fadeText(2, informationText, "Upgraded sword to level: " + currentWeaponTier + " And armor to level: " + currentArmorTier + "!", Color.green);
        }
    }

    void fadeText(float fadeTime, Text fadingText, string infoText, Color textColor)
    {
        fadingText.text = infoText;
        fadingText.gameObject.SetActive(true);
        Color fixedColor = textColor;
        fixedColor.a = 1;
        fadingText.color = fixedColor;
        fadingText.CrossFadeAlpha(1f, 0f, true);

        fadingText.CrossFadeAlpha(0, fadeTime, false);
    }

    public void upgradeArmorTier()
    {
        currentArmorTier++;
        armorDamageAbsorbtion = (armorAbsorbtionRate * currentArmorTier);
    }

    public void upgradeWeaponTier()
    {
        currentWeaponTier++;
        weaponEnchantment = (weaponEnchantingRate * currentWeaponTier);
    }

    public int calculateDamageTaken(int rawDamage, bool playerIsVictim)
    {
        if (playerIsVictim)
        {
            int newDamage = (rawDamage - (int)(((float)rawDamage / 100) * armorDamageAbsorbtion)); //when the player has found better armor it absorbs more damage
            return newDamage;
        }
        else
        {
            int newDamage = (rawDamage - (int)(((float)rawDamage / 100) * weaponEnchantment)); //when the player has found a better weapon it deals more damage
            return newDamage;
        }
    }

    private void giveKeysToEnemies(List<Room> generatedRooms)
    {
        foreach(Room item in generatedRooms)
        {
            if (item.isTreasureRoom)
            {
                List<EnemyController> enemiesInRoom = enemies.FindAll(x => x.roomID == item.roomPosition);
                int luckyEnemy = Random.Range(0, (enemiesInRoom.Count - 1));
                enemiesInRoom[luckyEnemy].hasKey = true;
            }
        }
    }

    public void SwitchBossActivationState(bool chasePlayer)
    {
        if(BossScript == null)
        {
            BossScript = GameObject.FindGameObjectWithTag("Boss").GetComponent<EnemyController>();
        }
        BossScript.activateChase(chasePlayer);
    }

    public void SwitchEnemyActivationState(bool chasePlayer, EnemyController enemy)
    {
        enemy.activateChase(chasePlayer);
    }
}

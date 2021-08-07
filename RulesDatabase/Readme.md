# Iconic Abilities

From classes:

 - Sorcerer (Player's Handbook 2)
 - Avenger (Player's Handbook 2)
 - Barbarian (Player's Handbook 2)
 - Monk (Player's Handbook 3)
 - Assassin (Dragon Magazine 379)
 - Ranger (Player's Handbook)
 - Rogue (Player's Handbook)
 - Warlock (Player's Handbook)

Browsing abilities with this query:

    SELECT TOP (1000) WizardsId
          ,(SELECT [Name] FROM [rulesdb].[dbo].[ImportedRules] [ClassRule] WHERE ClassRule.WizardsId = [ImportedRules].Class_WizardsId) as ClassName
          ,[Level]
          ,[PowerUsage]
          ,[ActionType]
	      ,(SELECT STRING_AGG([KeywordName],',') FROM Keywords INNER JOIN ImportedRuleKeyword ON Keywords.Id=ImportedRuleKeyword.KeywordsId WHERE ImportedRuleKeyword.RulesId=[ImportedRules].Id) [Keywords]
          ,[Name]
	      ,[Label],[Text]
      FROM [rulesdb].[dbo].[ImportedRules]
      INNER JOIN RulesTextEntry ON RuleId=Id
      Where [Type]='power' and PowerType='Attack' and Class_WizardsId IN ('ID_FMP_CLASS_5',
    'ID_FMP_CLASS_6',
    'ID_FMP_CLASS_7',
    'ID_FMP_CLASS_128',
    'ID_FMP_CLASS_129',
    'ID_FMP_CLASS_148',
    'ID_FMP_CLASS_362',
    'ID_FMP_CLASS_466') AND Level = 5
      order by [Level] ASC, PowerUsage ASC, Id, [Order]

## Basic premise

PC at-will damage is essentially:

    At-Will Damage (lvl 1) = [W] + Abil + Abil2 at 1st level = 10.5 per hit
    + 8 over 30 levels from weapons
    + 5.5 at 21 from at-will growth
    + 5 over 30 levels from stats
    At-Will Damage = 10 + level / 2

But if,

    Encounter Damage = 1.5 * At-Will Damage
    Daily Damage = 2 * At-Will Damage

Then the total damage output for the other powers should be:

    Encounter Damage = 15 + level * 0.75
    Daily Damage = 20 + level

Using that, we can calculate:

    [W] = 5.25 + level / 4 = {5.5, 5.75, 6, 6.25, 6.5, 7.75...}
    Abil =
      level < 4: 3
      level < 11: 4
      level < 18: 5
      level < 24: 6
      level <= 30: 7
    Abil2 =
      level < 8: 2
      level < 14: 3
      level < 21: 4
      level < 28: 5
      level <= 30: 6

This means:

    Lvl 1 Encounter = 15.5 @ [W] = 5.5
    Lvl 3 Encounter = 17.25 @ [W] = 6
    Lvl 27 Encounter = 35.25 @ [W] = 12


## Derived assumptions:

At 1st level:
- Abil = +3
- Abil2 = +2
- [W] = 5.5 (d10 w/ +2 proficiency)

### Basic Attack

* Abil vs AC. 1[W] + Abil

### Standard Actions

At-Will:
* 1-20:
  * Abil vs AC. 1[W] + Abil + Abil2 damage
  * 2x Abil vs AC. 1[W] damage
  * Burst 1, Abil vs NAD. 1d6 + Abil damage
* 21-30:
  * Abil vs AC. 2[W] + Abil + Abil2 damage
  * 2x Abil vs AC. 2[W] damage
  * Burst 1, Abil vs NAD. 2d6 + Abil damage

Encounter powers basically follow:
* 1-10: Abil vs AC. 2[W] + Abil + Abil2 damage
* 11-20: Abil vs AC. 3[W] + Abil + Abil2 damage
* 21-30: Abil vs AC. 4[W] + Abil + Abil2 damage

Daily output follows slightly better than Rage Strike (ID_FMP_POWER_4807):
* 1st level 3[W] + Abil modifier + miss half
* 5th level 4[W] + Abil modifier + miss half
* 9th level 5[W] + Abil modifier + miss half
* 15th level 6[W] + Abil modifier + miss half
* 19th level 7[W] + Abil modifier + miss half
* 20th level 7[W] + Abil modifier + miss half
* 25th level 8[W] + Abil modifier + miss half
* 29th level 9[W] + Abil modifier + miss half
_Note_: Confirmed by finding ID_FMP_POWER_3639 as lvl 29.

### Free Actions

* Trigger + Requirement = Original - Basic Attack
  * Lvl 1 Free-Action daily = 2[W] + Abil (Ex ID_FMP_POWER_4473)
  * Lvl 3 Encounter = Strength + 2 vs. Fortitude. You push the target 1 square and knock it prone. (Ex ID_FMP_POWER_226)

### Immediate Reaction

* If it was Daily, make it powered like a similar-level Encounter.
* If it was Encounter, make it powered like a similar-level At-Will.

### Minor Action

* Lvl 1 Daily
  * Use an at-will standard action power once per turn as a minor action until end of encounter (ID_FMP_POWER_7429)
* Original minus Basic Attack (ID_FMP_POWER_2595)
* Requirement = Original (ID_FMP_POWER_1692)

### Stance, Polymorph, Beast Form

### Summoning

### Status Effects

Formulas:
* AC = non-armor-defense (NAD) + 2
* bonus to-hit = bonus to damage
* automatic = x2 (ID_FMP_POWER_463)
* TODO: Miss half?
* TODO: log4(squares in AOE) = damage multiplier.
    * burst 1 or blast 3 would be x1.5 (ID_FMP_POWER_1437)
    * burst 2 or blast 5 would be x2.3
* Shift = 1 (ID_FMP_POWER_2105)
* Target grants combat advantage 1 turn to 1 ally = [W] (ID_FMP_POWER_10736)
* Move damage to 2ndary attack = x2 (ID_FMP_POWER_10476)
* Grant combat advantage to target = 2 (ID_FMP_POWER_2620)
* TODO: Stationary Damage (ID_FMP_POWER_4309, ID_FMP_POWER_1164)
* Weakened = [W] (ID_FMP_POWER_416, ID_FMP_POWER_393)
* Slowed
  * 1 turn = Abil2??  (ID_FMP_POWER_9410, ID_FMP_POWER_5098, ID_FMP_POWER_9837)
  * save ends = x2 (ID_FMP_POWER_7456, ID_FMP_POWER_2124)
* Dazed
  * 1 turn = Abil2?? (ID_FMP_POWER_12932, ID_FMP_POWER_1059)
  * Save Ends = [W] (ID_FMP_POWER_10277, ID_FMP_POWER_9564)
* Immobilized
  * 1 turn = [W] (ID_FMP_POWER_1931, ID_FMP_POWER_11354)
* TODO: Blinded
* Prone = [W] (ID_FMP_POWER_3009)
* Healing Surge = [W] (ID_FMP_POWER_1431, ID_FMP_POWER_1438)
* -2 penalty to AC
  * 1 turn = Abil2 (ID_FMP_POWER_608)
* -2 (or Abil2) penalty to all defenses
  * 1 turn = 2 * Abil2 (ID_FMP_POWER_1022)
* TODO: Slowed/Unconscious (ID_FMP_POWER_451)
* TODO: Ongoing (ID_FMP_POWER_2272)
* Reroll but must take 2nd = [W] (ID_FMP_POWER_883)
* Disarm and catch = [W] + Abil2 (ID_FMP_POWER_2179)
* Reliable = normal (ID_FMP_POWER_1526)
* Free basic attacks = [W] (ID_FMP_POWER_1525, ID_FMP_POWER_1527)
* Regeneration 5 = [W] (ID_FMP_POWER_936)
* +2 to attack = [W] (ID_FMP_POWER_936)
* ... until end of encounter = x2


## Intriguing powers:

- ID_FMP_POWER_12191 (attack bonus grows with levels)


## Basic Attack

Example powers:
- ID_INTERNAL_POWER_MELEE_BASIC_ATTACK, Melee Basic Attack - Strength vs. AC. 1[W] + Strength modifier damage.
- ID_FMP_POWER_1333, Eldritch Blast - Cha or Con vs. Reflex. 1d10 + Charisma or Constitution modifier damage.
- ID_FMP_POWER_5259, Acid Orb - Charisma vs. Reflex. 1d10 + Charisma modifier acid damage.


## Lvl 1 At-Will

Standard:
- Abil vs AC. 1[W] + Abil + Abil2 damage
- Abil + 2 vs AC. 1[W] + Abil damage
- Abil vs NAD. 1[W] + Abil damage
- 2x Abil vs AC. 1[W] damage
- Burst 1, Abil vs NAD. 1d6 + Abil damage
- Automatic, 2 + Abil (5 x automatic = 10)

Notes:
- +2 vs AC = vs NAD
- +2 vs AC = + 2ndary Abil damage


Example powers:
- ID_FMP_POWER_87, Twin Strike - 2x, Str vs AC. 1[W] damage per attack.
- ID_FMP_POWER_2104, Dual Strike - 2x, Str vs AC. 1[W] damage per attack.
- ID_FMP_POWER_704, Piercing Strike - Dex vs Reflex. 1[W] + Dexterity modifier damage.
- ID_FMP_POWER_917, Careful Attack - Str + 2 vs AC or Dex + 2 vs AC. 1[W] + Strength modifier damage (melee) or 1[W] + Dexterity modifier damage (ranged).
- ID_FMP_POWER_970, Sly Flourish - Dex vs AC. 1[W] + Dexterity modifier + Charisma modifier damage.
- ID_FMP_POWER_1166, Scorching Burst - Burst 1, Intelligence vs. Reflex. 1d6 + Intelligence modifier fire damage.
- ID_FMP_POWER_463, Magic Missile - Automatic 2 + Intelligence modifier force damage.
- ID_FMP_POWER_1457, Dire Radiance - Constitution vs. Fortitude. 1d6 + Con, and if it moves 1d6 + Con.
- ID_FMP_POWER_1458, Hellish Rebuke - Constitution vs. Reflex. 1d6 + Con, and if you take damage 1d6 + Con.
- ID_FMP_POWER_2666, Thorn Whip - Wisdom vs. Fortitude. 1d8 + Wisdom modifier damage, and you pull the target 2 squares.

## Lvl 1 Encounter

- Abil vs AC. 2[W] + Abil + Abil2 damage
- Close Burst 1 - Abil vs. AC. 1[W] + Abil damage
- Blast 5 - Abil vs NAD. 2d6 + Abil damage, miss half

Notes:
- ID_FMP_POWER_2209 is ID_FMP_POWER_1385, but with hedged bet on 1[W] for gamble on +Abil2 damage.
- ID_FMP_POWER_2893 sacrifices +Abil2 for use as a basic attack

Example powers:
- ID_FMP_POWER_1385, Torturous Strike - Dex vs. AC. 2[W] + Dexterity modifier + Str damage.
- ID_FMP_POWER_1510, Dire Wolverine Strike - Burst 1 - Strength vs. AC. 1[W] + Strength modifier damage.
- ID_FMP_POWER_2209, Two-Fanged Strike - 2x Strength vs AC. 1[W] + Strength modifier damage. If both hit, + WIS.
- ID_FMP_POWER_2595, Off-Hand Strike - Minor, off-hand Strength vs AC. 1[W] + Strength modifier damage.
- ID_FMP_POWER_2893, Whirlwind Charge - Wisdom vs. AC. 2[W] + Wisdom modifier damage. Sometimes can be used as basic attack.
- ID_FMP_POWER_3404, Flickering Venom - Charisma vs. Reflex. 2d8 + Charisma modifier force damage, and if you have combat advantage against the target, you also deal poison damage equal to your Intelligence modifier.
- ID_FMP_POWER_1171, Chill Strike - Intelligence vs. Fortitude. 2d8 + Intelligence modifier cold damage, and the target is dazed until the end of your next turn.

Non-striker powers:
- ID_FMP_POWER_159, Burning Hands - Close blast 5, Intelligence vs. Reflex. 2d6 + Intelligence modifier fire damage. Miss half. (10 + AOE + Miss half)

## Lvl 1 Daily

- Abil vs AC. 3[W] + Abil. (~19.5)
- Abil vs AC. 2[W] + Abil + 2[W off-hand] + Abil. (~26)
- Abil vs NAD. 3d10 + Abil, miss 1d10 + Abil. (16.5 + Abil + 2)
- Abil vs NAD. 6d6 + abil, miss half. (21 + Abil + 2)

Example powers:
- ID_FMP_POWER_4807, Rage Strike - lvl 1 daily counts as Abil vs AC. 3[W] + Abil, miss half.
- ID_FMP_POWER_851, Jaws of the Wolf - 2x Strength vs. AC. 2[W] + Strength modifier damage per attack, miss half.
- ID_FMP_POWER_883, Sure Shot - Dexterity vs. AC. 3[W] + Dex. (You can reroll each die once but must use the second result.)
- ID_FMP_POWER_1163, Chromatic Orb - Charisma vs. Reflex. 3d10 + CHA. On Miss, 1d10 + CHA.
- ID_FMP_POWER_2207, Split the Tree - Dexterity vs. AC. Make two attack rolls, take the higher result, and apply it to both targets. 2[W] + Dexterity modifier damage.
- ID_FMP_POWER_4473, Press the Advantage - Free action when bloodying an enemy, Dexterity vs. AC. 2[W] + Dexterity modifier damage.
- ID_FMP_POWER_5264, Dazzling Ray - Charisma vs. Will. 6d6 + Charisma modifier radiant damage, miss half

- ID_FMP_POWER_4472, Precise Incision - Dexterity vs. Reflex. 3[W] + Dexterity modifier damage. (reliable??)
- ID_FMP_POWER_3035 - has ongoing
- ID_FMP_POWER_1323 - has ongoing


## Lvl 3 Encounter

- Abil vs NAD. 2[W] + Abil damage.
- 3x3. Abil vs NAD. 2d8 + Abil damage, 2 types.
- Abil vs AC. 2[W] + Abil + Abil2 damage
- Close Burst 1 - Abil vs. AC. 1[W] + Abil damage
- Burst 2 - Abil vs NAD. 2d6 + Abil damage, miss half

Example powers:
- ID_FMP_POWER_1521, Shadow Wasp Strike - Strength vs. Reflex (melee) or Dexterity vs. Reflex (ranged). 2[W] + Strength modifier damage (melee) or 2[W] + Dexterity modifier damage (ranged).
- ID_FMP_POWER_5269, Acid Claw - Charisma vs. Fortitude. 2d10 + CHA + STR acid damage. + burst 1: acid damage equal to your Strength modifier.
- ID_FMP_POWER_3013, Swirling Stars - Burst 1. Charisma vs. Reflex. 2d8 + Charisma modifier cold and thunder damage.
- ID_FMP_POWER_4477, Flamboyant Strike - Dexterity vs. AC. 2[W] + Dexterity modifier + Charisma modifier damage.

- ID_FMP_POWER_2509, Defender's Cohort - Dexterity vs. AC. 2[W] + DEX + CHA damage. (rattling??)
- ID_FMP_POWER_291, Sweeping Blow - Close burst 1, Strength vs. AC. 1[W] + Strength modifier damage.
- ID_FMP_POWER_1530, Shock Sphere - Burst 2, Intelligence vs. Reflex. 2d6 + Intelligence modifier lightning damage.

- ID_FMP_POWER_4386, Paired Predators - Strength vs. AC. 2[W] + STR damage. Beast companion gets to make a free basic attack. (effectively 2[W] + [B] + Abil damage)
- ID_FMP_POWER_3010 - conditional burst
- ID_FMP_POWER_3753 - as ID_FMP_POWER_3013, but also has slow and only one damage type
- ID_FMP_POWER_5270 - as ID_FMP_POWER_3013, but applies penalty to Fort

## Lvl 5 Daily

Example powers:
- ID_FMP_POWER_1343 - Burst 1, The targets take ongoing 5 fire damage (save ends). Constitution vs. Reflex. 2d10 + Constitution modifier fire damage. (38 but no half)
- ID_FMP_POWER_5847 - The target takes ongoing 5 acid damage (save ends). Charisma vs. Fortitude. 2d10 + Charisma modifier poison damage. (24 no half, 2 types)


- ID_FMP_POWER_554 - ignores concealment

## Lvl 7 Encounter

- Abil vs AC. 2[W] + Abil + Abil2 damage

Example powers:
- ID_FMP_POWER_189, Fire Burst - Burst 2, Intelligence vs. Reflex. 3d6 + Intelligence modifier fire damage.
- ID_FMP_POWER_1428, Reckless Strike - Strength –2 vs. AC. 3[W] + Strength modifier damage. (24)
- ID_FMP_POWER_251, Lightning Bolt - 3x Intelligence vs. Reflex. 2d6 + Intelligence modifier lightning damage. Miss half.
- ID_FMP_POWER_920, Spikes of the Manticore - One or two creatures, Dexterity vs. AC, one attack per target. 2[W] + Dexterity modifier damage (first shot) and 1[W] + Dexterity modifier damage (second shot).
- ID_FMP_POWER_608, Griffon's Wrath - Str vs AC. 2[W] + Str, target takes -2 penalty to AC
- ID_FMP_POWER_1428, Reckless Strike - Str - 2 vs AC. 3[W] + Str

## Lvl 11 Encounter

- Abil vs AC. 3[W] + Abil + Abil2 damage

Example power:
- ID_FMP_POWER_1601, Precision Cut - Strength vs. Reflex. 3[W] + Strength modifier damage.

## Lvl 13 Encounter

- Abil vs AC. 3[W] + Abil + 2 * Abil2 damage

Example powers:
- ID_FMP_POWER_1022, Chains of Sorrow
- ID_FMP_POWER_1026, Talon of the Roc

## Lvl 15 Daily

- Abil vs. AC. 6[W]

Example powers:
- ID_FMP_POWER_1101, Dragon's Fangs. 2x Str vs AC, 3[W] + Str, Miss half.

## Lvl 17 Encounter

- Abil vs AC. 3[W] + Abil + 2 * Abil2 damage

Example powers:
- ID_FMP_POWER_2178, Exacting Strike. Strength + 6 vs. AC, 2[W] + Strength modifier damage.

## Lvl 23 Encounter

- Abil vs AC. 4[W] + Abil + 2 * Abil2 damage

Example powers:
- ID_FMP_POWER_1062
- ID_FMP_POWER_1067

## Lvl 27 Encounter

- Abil vs AC. 4[W] + Abil + 2 * Abil2 damage

Example powers:
- ID_FMP_POWER_5353, Death Stroke - Wis vs. AC. 4[W] + WIS damage. OR Wis + 4 vs AC. 6[W] + WIS damage.
- Seems OP: ID_FMP_POWER_4974, Butcher's Feast - Strength vs. AC. 6[W] + STR damage.

## Lvl 29 Daily

- Abil vs AC. 9[W] + Abil modifier + miss half

Example powers:
- ID_FMP_POWER_3639, Final Oath - Wisdom vs. AC. 9[W] + Wisdom modifier damage. Miss half

# References

https://forum.rpg.net/index.php?threads/4e-how-fast-does-pc-damage-rise-with-levels.744938/post-18583655
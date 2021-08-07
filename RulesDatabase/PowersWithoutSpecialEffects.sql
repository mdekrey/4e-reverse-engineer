use [rulesdb];
WITH Conditions as (
	SELECT 'Blinded' AS [Condition] UNION
	SELECT 'Dazed' AS [Condition] UNION
	SELECT 'Deafened' AS [Condition] UNION
	SELECT 'Dominated' AS [Condition] UNION
	SELECT 'Dying' AS [Condition] UNION
	SELECT 'Helpless' AS [Condition] UNION
	SELECT 'Immobilized' AS [Condition] UNION
	SELECT 'you mark' AS [Condition] UNION
	SELECT 'Marked' AS [Condition] UNION
	SELECT 'Petrified' AS [Condition] UNION
	SELECT 'Prone' AS [Condition] UNION
	SELECT 'Restrained' AS [Condition] UNION
	SELECT 'Slowed' AS [Condition] UNION
	SELECT 'Stunned' AS [Condition] UNION
	SELECT 'Surprised' AS [Condition] UNION
	SELECT 'Unconscious' AS [Condition] UNION
	SELECT 'Weakened' AS [Condition] UNION
	SELECT 'Healing Surge' AS [Condition] UNION
	SELECT 'temporary hit point' AS [Condition] UNION
	SELECT 'Push' AS [Condition] UNION
	SELECT 'Slide' AS [Condition] UNION
	SELECT 'Pull' AS [Condition] UNION
	SELECT 'Shift' AS [Condition] UNION
	SELECT 'Teleport' AS [Condition] UNION
	SELECT 'Penalty' AS [Condition] UNION
	SELECT 'Bonus' AS [Condition] UNION
	SELECT 'Ongoing' AS [Condition] UNION
	SELECT 'Zone' AS [Condition] UNION
	SELECT 'Area' AS [Condition] UNION
	SELECT 'Grants Combat Advantage' AS [Condition] UNION
	SELECT 'Swap Places' AS [Condition] UNION
	SELECT 'Swaps Places' AS [Condition] UNION
	SELECT 'Switch Places' AS [Condition] UNION
	SELECT 'Switches Places' AS [Condition] UNION
	SELECT 'Immediate Interrupt' AS [Condition] UNION
	SELECT 'Minor Action' AS [Condition] UNION
	SELECT 'Free Action' AS [Condition] UNION
	SELECT 'Opportunity Action' AS [Condition] UNION
	SELECT 'If' AS [Condition] UNION
	SELECT 'Two Attacks' AS [Condition] UNION
	SELECT 'Secondary Attack' AS [Condition] UNION
	SELECT 'makes a % attack' AS [Condition] UNION
	SELECT 'You Move' AS [Condition] UNION
	SELECT 'You Can Move' AS [Condition] UNION
	SELECT 'Use the Power' AS [Condition] UNION
	SELECT 'Use the % Power' AS [Condition] UNION
	SELECT 'Ignores Cover' AS [Condition] UNION
	SELECT 'Rolls Twice' AS [Condition] UNION
	SELECT 'Concealment' AS [Condition] UNION
	SELECT 'regain hit points' AS [Condition] UNION
	SELECT 'temporary hit points' AS [Condition] UNION
	SELECT 'invisible' AS [Condition] UNION
	SELECT '+ % vs.' AS [Condition] UNION
	SELECT 'This power can be used as a % basic attack' AS [Condition] UNION
	SELECT 'in place of a % basic attack' AS [Condition] UNION
	SELECT 'The first' AS [Condition] UNION
	SELECT 'The next' AS [Condition] UNION
	SELECT 'And an enemy' AS [Condition] UNION
	SELECT 'enemies adjacent' AS [Condition] UNION
	SELECT 'unoccupied square' AS [Condition] UNION
	SELECT 'gains resist' AS [Condition] UNION
	SELECT 'gains vulnerability' AS [Condition]
), Powers as (
	SELECT *
	FROM [rulesdb].[dbo].[ImportedRules] as Powers
	WHERE Class_WizardsId != ''
	  AND Display NOT LIKE '%Utility%' AND Display NOT LIKE '%Feature%'
	  -- AND ActionType NOT IN ('Opportunity Action', 'Immediate Reaction', 'Free Action')
	  --AND 'Conjuration' not in (SELECT [KeywordName] FROM Keywords INNER JOIN ImportedRuleKeyword ON Keywords.Id=ImportedRuleKeyword.KeywordsId WHERE ImportedRuleKeyword.RulesId=Powers.Id)
	  AND '_ChildPower' not in (SELECT Label FROM RulesTextEntry WHERE RulesTextEntry.RuleId=Powers.Id)
	  AND 'Psionic Power' not in (SELECT [SourceName] FROM Sources INNER JOIN ImportedRuleSource ON Sources.Id=ImportedRuleSource.SourcesId WHERE ImportedRuleSource.RulesId=Powers.Id)
	  AND 'Personal' not in (SELECT [Text] FROM RulesTextEntry INNER JOIN ImportedRuleSource ON RuleId=Powers.Id WHERE Label='Attack Type')
)
SELECT TOP (1000) WizardsId
      ,(SELECT STRING_AGG([SourceName],',') FROM Sources INNER JOIN ImportedRuleSource ON Sources.Id=ImportedRuleSource.SourcesId WHERE ImportedRuleSource.RulesId=Powers.Id) [Source]
      -- ,(SELECT [Name] FROM [ImportedRules] [ClassRule] WHERE ClassRule.WizardsId = Powers.Class_WizardsId) as ClassName
	  ,[Display]
      -- ,[Level]
      ,[PowerUsage]
      ,[ActionType]
	  ,(SELECT STRING_AGG([KeywordName],',') FROM Keywords INNER JOIN ImportedRuleKeyword ON Keywords.Id=ImportedRuleKeyword.KeywordsId WHERE ImportedRuleKeyword.RulesId=Powers.Id) [Keywords]
      ,[Name]
	  ,[Label],[Text]
  FROM Powers
  INNER JOIN RulesTextEntry ON RuleId=Id
  Where [Type]='power' 
  and WizardsId not in ('ID_FMP_POWER_704', 'ID_FMP_POWER_970', 'ID_FMP_POWER_1333', 'ID_FMP_POWER_1710', 'ID_FMP_POWER_2099', 'ID_FMP_POWER_2248', 'ID_FMP_POWER_2848', 'ID_FMP_POWER_3403', 'ID_FMP_POWER_7151', 'ID_FMP_POWER_7452', 'ID_FMP_POWER_1524', 'ID_FMP_POWER_3328')
  and Id NOT IN (SELECT RuleId
	FROM RulesTextEntry
	INNER JOIN Conditions ON (' ' + Text + ' ') like ('%[^a-z]' + Conditions.Condition + '[^a-z]%')
	INNER JOIN Powers ON RuleId=Powers.Id
	WHERE Label NOT IN ('Short Description', 'Standard Action', 'Target', 'Targets', 'Trigger', 'Requirement'))
  order by [Level] ASC, PowerUsage ASC, Id, [Order]

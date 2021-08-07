use [rulesdb];
with ClassRoles as (
	SELECT TOP (1000) WizardsId, Name as ClassName
		,(SELECT STRING_AGG([SourceName],',') FROM Sources INNER JOIN ImportedRuleSource ON Sources.Id=ImportedRuleSource.SourcesId WHERE ImportedRuleSource.RulesId=[ImportedRules].Id) [Source]
		,(SELECT SUBSTRING(Text, 0, CHARINDEX('.', Text)) FROM RulesTextEntry WHERE RulesTextEntry.RuleId=[ImportedRules].Id AND RulesTextEntry.Label='Role') [ClassRole]
	  FROM [rulesdb].[dbo].[ImportedRules]
	  WHERE [Type]='class'
), Powers as (
	SELECT *
	FROM [rulesdb].[dbo].[ImportedRules] as Powers
	WHERE Class_WizardsId != ''
	  AND Display NOT LIKE '%Utility%'
	  AND '_ChildPower' not in (SELECT Label FROM RulesTextEntry WHERE RulesTextEntry.RuleId=Powers.Id)
	  AND 'Psionic Power' not in (SELECT [SourceName] FROM Sources INNER JOIN ImportedRuleSource ON Sources.Id=ImportedRuleSource.SourcesId WHERE ImportedRuleSource.RulesId=Powers.Id)
	  AND 'Personal' not in (SELECT [Text] FROM RulesTextEntry INNER JOIN ImportedRuleSource ON RuleId=Powers.Id WHERE Label='Attack Type')
), Conditions as (
	SELECT 'Blinded' AS [Condition], 'Blinded' AS ConditionName UNION
	SELECT 'Dazed' AS [Condition], 'Dazed' AS ConditionName UNION
	SELECT 'Deafened' AS [Condition], 'Deafened' AS ConditionName UNION
	SELECT 'Dominated' AS [Condition], 'Dominated' AS ConditionName UNION
	SELECT 'Dying' AS [Condition], 'Dying' AS ConditionName UNION
	SELECT 'Helpless' AS [Condition], 'Helpless' AS ConditionName UNION
	SELECT 'Immobilized' AS [Condition], 'Immobilized' AS ConditionName UNION
	SELECT 'you mark' AS [Condition], 'Marked' AS ConditionName UNION
	SELECT 'Marked' AS [Condition], 'Marked' AS ConditionName UNION
	SELECT 'Petrified' AS [Condition], 'Petrified' AS ConditionName UNION
	SELECT 'Prone' AS [Condition], 'Prone' AS ConditionName UNION
	SELECT 'Restrained' AS [Condition], 'Restrained' AS ConditionName UNION
	SELECT 'Slowed' AS [Condition], 'Slowed' AS ConditionName UNION
	SELECT 'Stunned' AS [Condition], 'Stunned' AS ConditionName UNION
	SELECT 'Surprised' AS [Condition], 'Surprised' AS ConditionName UNION
	SELECT 'Unconscious' AS [Condition], 'Unconscious' AS ConditionName UNION
	SELECT 'Weakened' AS [Condition], 'Weakened' AS ConditionName UNION
	SELECT 'Healing Surge' AS [Condition], 'Healing' AS ConditionName UNION
	SELECT 'regain hit points' AS [Condition], 'Healing' AS ConditionName UNION
	SELECT 'temporary hit points' AS [Condition], 'Healing' AS ConditionName UNION
	SELECT 'temporary hit point' AS [Condition], 'Healing' AS ConditionName UNION
	SELECT 'Push' AS [Condition], 'Push' AS ConditionName UNION
	SELECT 'Slide' AS [Condition], 'Slide' AS ConditionName UNION
	SELECT 'Pull' AS [Condition], 'Pull' AS ConditionName UNION
	SELECT 'Shift' AS [Condition], 'Shift' AS ConditionName UNION
	SELECT 'Teleport' AS [Condition], 'Teleport' AS ConditionName UNION
	SELECT 'Penalty' AS [Condition], 'Penalty' AS ConditionName UNION
	SELECT 'Bonus' AS [Condition], 'Bonus' AS ConditionName UNION
	SELECT 'Ongoing' AS [Condition], 'Ongoing' AS ConditionName UNION
	SELECT 'Zone' AS [Condition], 'Zone' AS ConditionName UNION
	SELECT 'Area' AS [Condition], 'Zone' AS ConditionName UNION
	SELECT 'Grants Combat Advantage' AS [Condition], 'Grants Combat Advantage' AS ConditionName UNION
	SELECT 'Swap Places' AS [Condition], 'Trade Places' AS ConditionName UNION
	SELECT 'Swaps Places' AS [Condition], 'Trade Places' AS ConditionName UNION
	SELECT 'Switch Places' AS [Condition], 'Trade Places' AS ConditionName UNION
	SELECT 'Switches Places' AS [Condition], 'Trade Places' AS ConditionName UNION
	SELECT 'Immediate Interrupt' AS [Condition], 'Extra Action' AS ConditionName UNION
	SELECT 'Minor Action' AS [Condition], 'Extra Action' AS ConditionName UNION
	SELECT 'Free Action' AS [Condition], 'Extra Action' AS ConditionName UNION
	SELECT 'Opportunity Action' AS [Condition], 'Extra Action' AS ConditionName UNION
	SELECT 'If' AS [Condition], 'If' AS ConditionName UNION
	SELECT 'Two Attacks' AS [Condition], 'Multiple Attacks' AS ConditionName UNION
	SELECT 'Secondary Attack' AS [Condition], 'Multiple Attacks' AS ConditionName UNION
	SELECT 'makes a % attack' AS [Condition], 'Extra Action' AS ConditionName UNION
	SELECT 'You Move' AS [Condition], 'You Move' AS ConditionName UNION
	SELECT 'You Can Move' AS [Condition], 'You Can Move' AS ConditionName UNION
	SELECT 'Use the Power' AS [Condition], 'Use the Power' AS ConditionName UNION
	SELECT 'Use the % Power' AS [Condition], 'Use the Power' AS ConditionName UNION
	SELECT 'Ignores Cover' AS [Condition], 'Ignores Cover' AS ConditionName UNION
	SELECT 'Rolls Twice' AS [Condition], 'Rolls Twice' AS ConditionName UNION
	SELECT 'Concealment' AS [Condition], 'Concealment' AS ConditionName UNION
	SELECT 'invisible' AS [Condition], 'Invisible' AS ConditionName UNION
	SELECT '+ % vs.' AS [Condition], 'Attack Bonus' AS ConditionName UNION
	SELECT 'This power can be used as a % basic attack' AS [Condition], 'Basic Attack' AS ConditionName UNION
	SELECT 'in place of a % basic attack' AS [Condition], 'Conditional Basic Attack' AS ConditionName UNION
	SELECT 'The first' AS [Condition], 'Once' AS ConditionName UNION
	SELECT 'The next' AS [Condition], 'Once' AS ConditionName UNION
	SELECT 'And an enemy' AS [Condition], 'Once' AS ConditionName UNION
	SELECT 'enemies adjacent' AS [Condition], 'Once' AS ConditionName UNION
	SELECT 'unoccupied square' AS [Condition], 'Zone' AS ConditionName UNION
	SELECT 'gains resist' AS [Condition], 'gains resist' AS ConditionName UNION
	SELECT 'gains vulnerability' AS [Condition], 'gains vulnerability' AS ConditionName
), PowerConditions AS (
	SELECT ClassRole, ClassRoles.ClassName, ConditionName AS Condition, Id, 
		CASE WHEN Id In (
			SELECT RuleId
			FROM RulesTextEntry
			WHERE RuleId=Powers.Id
			  AND Label NOT IN ('Short Description', 'Standard Action', 'Target', 'Targets', 'Trigger', 'Requirement') 
			  AND (' ' + Text + ' ') like ('%[^a-z]' + Conditions.Condition + '[^a-z]%')
		) THEN 1 ELSE 0 END as HasCondition
	FROM Powers
	INNER JOIN ClassRoles ON ClassRoles.WizardsId=Powers.Class_WizardsId
	, Conditions
	GROUP BY ClassRole, ClassName, ConditionName, Conditions.Condition, Id
)
SELECT * From (
SELECT ClassRole, ClassName, Condition, 
	Round(CAST(HasCondition as float) / PowerCount * 100, 1) AS ClassConditionFrequency, 
	ROUND(CAST(SUM(HasCondition) OVER (Partition By ClassRole, Condition) as float) / SUM(PowerCount) OVER (Partition By ClassRole, Condition) * 100, 1) As RoleConditionFrequency
FROM (
	SELECT ClassRole, ClassName, Condition, SUM(HasCondition) As HasCondition, COUNT(Id) As PowerCount
	FROM PowerConditions
	GROUP BY ClassRole, ClassName, Condition
) AS Grouped
) AS Base
Where RoleConditionFrequency > 5
ORDER BY ClassRole, RoleConditionFrequency DESC, Condition, ClassName
;

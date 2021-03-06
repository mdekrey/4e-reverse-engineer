/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) WizardsId, Name
	,(SELECT STRING_AGG([SourceName],',') FROM Sources INNER JOIN ImportedRuleSource ON Sources.Id=ImportedRuleSource.SourcesId WHERE ImportedRuleSource.RulesId=[ImportedRules].Id) [Source]
	,(SELECT SUBSTRING(Text, 0, CHARINDEX('.', Text)) FROM RulesTextEntry WHERE RulesTextEntry.RuleId=[ImportedRules].Id AND RulesTextEntry.Label='Role') [Role]
  FROM [rulesdb].[dbo].[ImportedRules]
  WHERE [Type]='class'
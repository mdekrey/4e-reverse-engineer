use [rulesdb];
/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) WizardsId
      ,(SELECT STRING_AGG([SourceName],',') FROM Sources INNER JOIN ImportedRuleSource ON Sources.Id=ImportedRuleSource.SourcesId WHERE ImportedRuleSource.RulesId=[ImportedRules].Id) [Source]
      ,(SELECT [Name] FROM [rulesdb].[dbo].[ImportedRules] [ClassRule] WHERE ClassRule.WizardsId = [ImportedRules].Class_WizardsId) as ClassName
      ,[Level]
      ,[PowerUsage]
      ,[ActionType]
	  ,(SELECT STRING_AGG([KeywordName],',') FROM Keywords INNER JOIN ImportedRuleKeyword ON Keywords.Id=ImportedRuleKeyword.KeywordsId WHERE ImportedRuleKeyword.RulesId=[ImportedRules].Id) [Keywords]
      ,[Name]
	  ,[Label],[Text]
  FROM [rulesdb].[dbo].[ImportedRules]
  INNER JOIN RulesTextEntry ON RuleId=Id
  Where [Type]='power' 
  and 'ID_FMP_POWER_1270' in (WizardsId, Name)
  order by CAST(Level AS int) ASC, PowerUsage ASC, Id, [Order]

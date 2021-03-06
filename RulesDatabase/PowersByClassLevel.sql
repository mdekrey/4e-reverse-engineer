use [rulesdb];

/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) WizardsId
		,Class_WizardsId
      ,(SELECT [Name] FROM [rulesdb].[dbo].[ImportedRules] [ClassRule] WHERE ClassRule.WizardsId = [ImportedRules].Class_WizardsId) as ClassName
      ,(SELECT STRING_AGG([SourceName],',') FROM Sources INNER JOIN ImportedRuleSource ON Sources.Id=ImportedRuleSource.SourcesId WHERE ImportedRuleSource.RulesId=[ImportedRules].Id) [Source]
      ,[Level]
      ,[PowerUsage]
      ,[ActionType]
	  ,(SELECT STRING_AGG([KeywordName],',') FROM Keywords INNER JOIN ImportedRuleKeyword ON Keywords.Id=ImportedRuleKeyword.KeywordsId WHERE ImportedRuleKeyword.RulesId=[ImportedRules].Id) [Keywords]
      ,[Name]
	  ,[Label],[Text]
  FROM [rulesdb].[dbo].[ImportedRules]
  INNER JOIN RulesTextEntry ON RuleId=Id
  Where [Type]='power' and PowerType='Attack' and Class_WizardsId IN (
  'ID_FMP_CLASS_3',
'ID_FMP_CLASS_9',
'ID_FMP_CLASS_5',
'ID_FMP_CLASS_6',
'ID_FMP_CLASS_7',
'ID_FMP_CLASS_128',
'ID_FMP_CLASS_129',
'ID_FMP_CLASS_148',
'ID_FMP_CLASS_362',
'ID_FMP_CLASS_466') AND Level = 1
  order by  CAST(Level AS int) ASC, PowerUsage ASC, Source, ClassName, Id, [Order]

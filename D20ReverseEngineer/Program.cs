using D20RulesEngine;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace D20ReverseEngineer
{
    static class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(GetKey(6216)); // "D&D4E"
            var ws = new D20Workspace("D&D4E");
            if (ws.Failed() || !ws.HasRights())
            {
                throw new InvalidOperationException();
            }

            var keywords = new HashSet<string>();
            var feats = new HashSet<string>();
            var rules = new List<RuleResult>();
            unsafe
            {
                RulesElement* rule = null;
                while ((rule = ws.IterateRulesElements(rule, null)) != null)
                {
                    var result = LoadRule(ws, rule);
                    //Console.WriteLine(result.Name);
                    //if (!string.IsNullOrEmpty(result.Prereqs))
                    //    System.Diagnostics.Debugger.Break();
                    foreach (var keyword in result.Keywords)
                        keywords.Add(keyword);
                    foreach (var feat in result.AssociatedFeats)
                        feats.Add(feat);
                    rules.Add(result);
                }
            }

            using (var connection = new SqlConnection("Server=127.0.0.1;Database=rulesdb;User Id=sa;Password=l0cal!PW;"))
            using (var insertRule = new SqlCommand(@"
INSERT INTO [dbo].[ImportedRules]
           ([WizardsId]
           ,[Name]
           ,[FlavorText]
           ,[Description]
           ,[ShortDescription]
           ,[Category]
           ,[Prereqs]
           ,[EncounterUses]
           ,[Type]
           ,[PowerUsage]
           ,[SkillPower_WizardsId]
           ,[Display]
           ,[ActionType]
           ,[Class_WizardsId]
           ,[Level]
           ,[PowerType])
     VALUES
           (@Id
           ,@Name
           ,@FlavorText
           ,@Description
           ,@ShortDescription
           ,@Category
           ,@Prereqs
           ,@EncounterUses
           ,@Type
           ,@PowerUsage
           ,@SkillPower_WizardsId
           ,@Display
           ,@ActionType
           ,@Class_WizardsId
           ,@Level
           ,@PowerType);
SELECT SCOPE_IDENTITY();
", connection))
            using (var insertRuleKeyword = new SqlCommand(@"
MERGE Keywords as Target
USING (SELECT @KeywordName AS KeywordName) AS Source
ON Target.KeywordName=Source.KeywordName
WHEN NOT MATCHED THEN
	INSERT (KeywordName) VALUES (Source.KeywordName);

INSERT INTO ImportedRuleKeyword (RulesId, KeywordsId) 
SELECT @ImportedRuleId, Id FROM Keywords WHERE KeywordName=@KeywordName;
", connection))
            using (var insertRuleSource = new SqlCommand(@"
MERGE Sources as Target
USING (SELECT @SourceName AS SourceName) AS Source
ON Target.SourceName=Source.SourceName
WHEN NOT MATCHED THEN
	INSERT (SourceName) VALUES (Source.SourceName);

INSERT INTO ImportedRuleSource (RulesId, SourcesId) 
SELECT @ImportedRuleId, Id FROM Sources WHERE SourceName=@SourceName;
", connection))
            using (var insertRuleAssociatedFeat = new SqlCommand(@"
INSERT INTO [dbo].[ImportedRules_AssociatedFeats]
           ([ImportedRuleId]
           ,[WizardsId])
     VALUES
           (@ImportedRuleId
           ,@FeatId)
", connection))
            using (var insertRuleTextEntry = new SqlCommand(@"
INSERT INTO [dbo].[RulesTextEntry]
           ([RuleId]
           ,[Label]
           ,[Text])
     VALUES
           (@ImportedRuleId
           ,@Label
           ,@Text);
", connection))
            {
                connection.Open();
                insertRule.Parameters.Add("@Id", System.Data.SqlDbType.NVarChar);
                insertRule.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar);
                insertRule.Parameters.Add("@FlavorText", System.Data.SqlDbType.NVarChar);
                insertRule.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar);
                insertRule.Parameters.Add("@ShortDescription", System.Data.SqlDbType.NVarChar);
                insertRule.Parameters.Add("@Category", System.Data.SqlDbType.NVarChar);
                insertRule.Parameters.Add("@Prereqs", System.Data.SqlDbType.NVarChar);
                insertRule.Parameters.Add("@EncounterUses", System.Data.SqlDbType.Int);
                insertRule.Parameters.Add("@Type", System.Data.SqlDbType.NVarChar);
                insertRule.Parameters.Add("@PowerUsage", System.Data.SqlDbType.NVarChar);
                insertRule.Parameters.Add("@SkillPower_WizardsId", System.Data.SqlDbType.NVarChar);
                insertRule.Parameters.Add("@Display", System.Data.SqlDbType.NVarChar);
                insertRule.Parameters.Add("@ActionType", System.Data.SqlDbType.NVarChar);
                insertRule.Parameters.Add("@Class_WizardsId", System.Data.SqlDbType.NVarChar);
                insertRule.Parameters.Add("@Level", System.Data.SqlDbType.NVarChar);
                insertRule.Parameters.Add("@PowerType", System.Data.SqlDbType.NVarChar);

                insertRuleKeyword.Parameters.Add("@KeywordName", System.Data.SqlDbType.NVarChar);
                insertRuleKeyword.Parameters.Add("@ImportedRuleId", System.Data.SqlDbType.Int);

                insertRuleSource.Parameters.Add("@SourceName", System.Data.SqlDbType.NVarChar);
                insertRuleSource.Parameters.Add("@ImportedRuleId", System.Data.SqlDbType.Int);

                insertRuleAssociatedFeat.Parameters.Add("@ImportedRuleId", System.Data.SqlDbType.Int);
                insertRuleAssociatedFeat.Parameters.Add("@FeatId", System.Data.SqlDbType.NVarChar);

                insertRuleTextEntry.Parameters.Add("@ImportedRuleId", System.Data.SqlDbType.Int);
                insertRuleTextEntry.Parameters.Add("@Label", System.Data.SqlDbType.NVarChar);
                insertRuleTextEntry.Parameters.Add("@Text", System.Data.SqlDbType.NVarChar);

                foreach (var rule in rules)
                {
                    using (var transaction = connection.BeginTransaction())
                    {
                        insertRule.Parameters["@Id"].Value = rule.Id;
                        insertRule.Parameters["@Name"] . Value = rule.Name;
                        insertRule.Parameters["@FlavorText"] . Value = rule.FlavorText;
                        insertRule.Parameters["@Description"] . Value = rule.Description;
                        insertRule.Parameters["@ShortDescription"] . Value = rule.ShortDescription;
                        insertRule.Parameters["@Category"] . Value = rule.Category;
                        insertRule.Parameters["@Prereqs"] . Value = rule.Prereqs;
                        insertRule.Parameters["@EncounterUses"] . Value = rule.EncounterUses;
                        insertRule.Parameters["@Type"] . Value = rule.Type;
                        insertRule.Parameters["@PowerUsage"] . Value = rule.PowerUsage;
                        insertRule.Parameters["@SkillPower_WizardsId"] . Value = rule.SkillPower;
                        insertRule.Parameters["@Display"] . Value = rule.Display;
                        insertRule.Parameters["@ActionType"] . Value = rule.ActionType;
                        insertRule.Parameters["@Class_WizardsId"] . Value = rule.Class;
                        insertRule.Parameters["@Level"] . Value = rule.Level;
                        insertRule.Parameters["@PowerType"] . Value = rule.PowerType;
                        insertRule.Transaction = transaction;
                        var importedRuleId = Convert.ToInt32(insertRule.ExecuteScalar());
                        foreach (var keyword in rule.Keywords.Distinct())
                        {
                            insertRuleKeyword.Parameters["@KeywordName"].Value = keyword;
                            insertRuleKeyword.Parameters["@ImportedRuleId"].Value = importedRuleId;
                            insertRuleKeyword.Transaction = transaction;
                            insertRuleKeyword.ExecuteNonQuery();
                        }

                        foreach (var source in rule.Source.Distinct())
                        {
                            insertRuleSource.Parameters["@SourceName"].Value = source;
                            insertRuleSource.Parameters["@ImportedRuleId"].Value = importedRuleId;
                            insertRuleSource.Transaction = transaction;
                            insertRuleSource.ExecuteNonQuery();
                        }

                        foreach (var feat in rule.AssociatedFeats.Distinct())
                        {
                            insertRuleAssociatedFeat.Parameters["@ImportedRuleId"].Value = importedRuleId;
                            insertRuleAssociatedFeat.Parameters["@FeatId"].Value = feat;
                            insertRuleAssociatedFeat.Transaction = transaction;
                            insertRuleAssociatedFeat.ExecuteNonQuery();
                        }

                        foreach (var text in rule.AdditionalFields)
                        {
                            insertRuleTextEntry.Parameters["@ImportedRuleId"].Value = importedRuleId;
                            insertRuleTextEntry.Parameters["@Label"].Value = text.Key.Trim();
                            insertRuleTextEntry.Parameters["@Text"].Value = text.Value;
                            insertRuleTextEntry.Transaction = transaction;
                            insertRuleTextEntry.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                }
            }
        }

        private static unsafe RuleResult LoadRule(D20Workspace ws, RulesElement* rule)
        {
            var result = new RuleResult();

            result.Id = ws.RulesElementID(rule);
            result.Name = ws.RulesElementName(rule);
            result.FlavorText = ws.RulesElementFlavorText(rule);
            result.Description = ws.RulesElementDescription(rule);
            result.ShortDescription = ws.RulesElementShortDesc(rule);
            result.Category = ws.RulesElementCategory(rule);
            result.Prereqs = ws.RulesElementIntPrereqs(rule);
            result.EncounterUses = ws.RulesElementEncounterUses(rule);
            result.Source = (from i in Enumerable.Range(0, ws.RulesElementSourceCount(rule))
                             select ws.RulesElementSource(rule, i)).ToArray();
            result.Type = ws.RulesElementType(rule);
            result.Essentials = ws.RulesElementIsEssentials(rule);

            result.PowerUsage = ws.RulesElementField(rule, "Power Usage");
            result.SkillPower = ws.RulesElementField(rule, "_SkillPower");
            result.Display = ws.RulesElementField(rule, "Display");
            result.Keywords = ws.RulesElementField(rule, "Keywords").Split(new[] { ",", ";", " or " }, StringSplitOptions.None).Select(k => k.Trim()).Where(k => !string.IsNullOrEmpty(k)).ToArray();
            result.ActionType = ws.RulesElementField(rule, "Action Type");
            result.Class = ws.RulesElementField(rule, "Class");
            result.AssociatedFeats = ws.RulesElementField(rule, "_Associated Feats").Split(new[] { ",", ";" }, StringSplitOptions.None).Select(k => k.Trim()).Where(k => !string.IsNullOrEmpty(k)).ToArray();
            result.Level = ws.RulesElementField(rule, "Level");
            result.PowerType = ws.RulesElementField(rule, "Power Type");

            var others = new List<KeyValuePair<string, string>>();
            var count = ws.RulesElementSpecificCount(rule);
            for (var index = 0; index < count; ++index)
            {
                var field = ws.RulesElementSpecificField(rule, index);
                var value = ws.RulesElementSpecificValue(rule, index);
                if (CustomFields.Contains(field))
                    continue;

                others.Add(new KeyValuePair<string, string>(field, value));
            }
            result.AdditionalFields = others.ToArray();

            return result;
        }

        public static string GetKey(int P_0)
        {
            using (var unknownBinStream = typeof(Program).Assembly.GetManifestResourceStream("D20ReverseEngineer.data.obfuscated-strings.bin"))
            {
                unknownBinStream.Position = P_0;
                int num = unknownBinStream.ReadByte();
                int num2 = (num & 0x80) == 0 ? num
                    : ((uint)num & 0x40u) != 0 ? ((num & 0x1F) << 24) + (unknownBinStream.ReadByte() << 16) + (unknownBinStream.ReadByte() << 8) + unknownBinStream.ReadByte()
                    : ((num & 0x3F) << 8) + unknownBinStream.ReadByte();
                byte[] array = new byte[num2];
                unknownBinStream.Read(array, 0, num2);
                if (array.Length == 0)
                {
                    return string.Empty;
                }
                byte[] array2 = Convert.FromBase64String(Encoding.UTF8.GetString(array, 0, array.Length));
                return string.Intern(Encoding.UTF8.GetString(array2, 0, array2.Length));
            }
        }

        private static readonly HashSet<string> CustomFields = new HashSet<string>()
        {
            "Power Usage",
            "_SkillPower",
            "Display",
            "Keywords",
            "Action Type",
            "Class",
            "_Associated Feats",
            "Level",
            "Power Type",
        };
        class RuleResult
        {
            public string Id { get; internal set; }
            public string Name { get; internal set; }
            public string FlavorText { get; internal set; }
            public string Description { get; internal set; }
            public string ShortDescription { get; internal set; }
            public string Category { get; internal set; }
            public string Prereqs { get; internal set; }
            public int EncounterUses { get; internal set; }
            public string[] Source { get; internal set; }
            public string Type { get; internal set; }
            public bool Essentials { get; internal set; }
            public KeyValuePair<string, string>[] AdditionalFields { get; internal set; }


            public string PowerUsage { get; internal set; }
            public string SkillPower { get; internal set; }
            public string Display { get; internal set; }
            public string[] Keywords { get; internal set; }
            public string ActionType { get; internal set; }
            public string Class { get; internal set; }
            public string[] AssociatedFeats { get; internal set; }
            public string Level { get; internal set; }
            public string PowerType { get; internal set; }
        }
    }
}

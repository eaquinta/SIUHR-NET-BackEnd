-- [FirstName] + ISNULL(' ' + [MiddleName], '') + ISNULL(' ' + [ThirdName], '') + ISNULL(' ' + [LastName], '') + ISNULL(' ' + [MatriName], '') + ISNULL(' ' + [MarriedName], '') PERSISTED

ALTER TABLE [dbo].[Personas] DROP COLUMN [Name]
GO
ALTER TABLE [dbo].[Personas] ADD [Name] AS [FirstName] + ISNULL(' ' + [MiddleName], '') + ISNULL(' ' + [ThirdName], '') + ISNULL(' ' + [LastName], '') + ISNULL(' ' + [MatriName], '') + ISNULL(' ' + [MarriedName], '') PERSISTED
GO

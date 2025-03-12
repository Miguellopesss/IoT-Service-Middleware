CREATE TABLE [dbo].[Application] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (50) NOT NULL,
    [CreationDatetime] DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([Name]) REFERENCES [dbo].[NomeTable] ([Nome])
);


GO
CREATE TRIGGER trg_InsertNomeIntoNomeTable_Application
ON [dbo].[Application]
INSTEAD OF INSERT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM [dbo].[NomeTable] WHERE [Nome] IN (SELECT [Name] FROM inserted))
    BEGIN
        RAISERROR('O Nome já existe na tabela NomeTable.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    INSERT INTO [dbo].[NomeTable] ([Nome])
    SELECT DISTINCT i.[Name]
    FROM inserted i

	SET IDENTITY_INSERT [dbo].[Application] OFF;
    INSERT INTO [dbo].[Application] ([Name], [CreationDatetime])
    SELECT i.[Name], i.[CreationDatetime]
    FROM inserted i;
END;
GO
CREATE TRIGGER trg_UpdateNomeIntoNomeTable_Application
ON [dbo].[Application]
INSTEAD OF UPDATE
AS
BEGIN
    IF EXISTS (SELECT 1 FROM [dbo].[NomeTable] WHERE [Nome] IN (SELECT [Name] FROM inserted))
    BEGIN
        RAISERROR('O Nome já existe na tabela NomeTable.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    DECLARE @OldName NVARCHAR(50);
    SELECT @OldName = d.[Name] FROM deleted d;

    INSERT INTO [dbo].[NomeTable] ([Nome])
    SELECT DISTINCT i.[Name]
    FROM inserted i
    WHERE NOT EXISTS (SELECT 1 FROM [dbo].[NomeTable] n WHERE n.[Nome] = i.[Name]);

    UPDATE [dbo].[Application]
    SET [Name] = i.[Name], [CreationDatetime] = i.[CreationDatetime]
    FROM inserted i
    WHERE [dbo].[Application].[Id] = i.[Id];

	DELETE FROM [dbo].[NomeTable] WHERE [Nome] = @OldName;
END;
GO
CREATE TRIGGER trg_DeleteNomeFromNomeTable_Application
ON [dbo].[Application]
INSTEAD OF DELETE
AS
BEGIN
	DELETE FROM [dbo].[Application]
    WHERE [Id] IN (SELECT [Id] FROM deleted);

    DELETE FROM [dbo].[NomeTable]
    WHERE [Nome] IN (SELECT [Name] FROM deleted);
END;
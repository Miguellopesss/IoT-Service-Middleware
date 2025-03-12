CREATE TABLE [dbo].[Container] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (50) NOT NULL,
    [CreationDatetime] DATETIME      NULL,
    [Parent]           INT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Name] ASC),
    CONSTRAINT [FK_Container_NomeTable] FOREIGN KEY ([Name]) REFERENCES [dbo].[NomeTable] ([Nome]),
    CONSTRAINT [FK_Container_Application] FOREIGN KEY ([Parent]) REFERENCES [dbo].[Application] ([Id])
);


GO
CREATE TRIGGER trg_InsertNomeIntoNomeTable_Container
ON [dbo].[Container]
INSTEAD OF INSERT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM [dbo].[NomeTable] n WHERE n.[Nome] IN (SELECT [Name] FROM inserted))
    BEGIN
        RAISERROR('O Nome já existe na tabela NomeTable.', 16, 1);
        RETURN;
    END

    INSERT INTO [dbo].[NomeTable] ([Nome])
    SELECT DISTINCT i.[Name]
    FROM inserted i

    INSERT INTO [dbo].[Container] ([Name], [CreationDatetime], [Parent])
    SELECT i.[Name], i.[CreationDatetime], i.[Parent]
    FROM inserted i;
END;
GO
CREATE TRIGGER trg_UpdateNomeIntoNomeTable_Container
ON [dbo].[Container]
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

    UPDATE [dbo].[Container]
    SET [Name] = i.[Name], [CreationDatetime] = i.[CreationDatetime], [Parent] = i.[Parent]
    FROM inserted i
    WHERE [dbo].[Container].[Id] = i.[Id];

	 DELETE FROM [dbo].[NomeTable] WHERE [Nome] = @OldName;
END;
GO
CREATE TRIGGER trg_DeleteNomeFromNomeTable_Container
ON [dbo].[Container]
INSTEAD OF DELETE
AS
BEGIN
    DELETE FROM [dbo].[Container]
    WHERE [Id] IN (SELECT [Id] FROM deleted);
	DELETE FROM [dbo].[NomeTable]
    WHERE [Nome] IN (SELECT [Name] FROM deleted);
END;
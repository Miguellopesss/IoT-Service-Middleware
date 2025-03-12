CREATE TABLE [dbo].[Record] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (50)  NOT NULL,
    [Content]          NVARCHAR (100) NULL,
    [CreationDatetime] DATETIME       NULL,
    [Parent]           INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Name] ASC),
    CONSTRAINT [FK_Record_NameTable] FOREIGN KEY ([Name]) REFERENCES [dbo].[NomeTable] ([Nome]),
    CONSTRAINT [FK_Record_Container] FOREIGN KEY ([Parent]) REFERENCES [dbo].[Container] ([Id])
);


GO
CREATE TRIGGER trg_InsertNomeIntoNomeTable_Record
ON [dbo].[Record]
INSTEAD OF INSERT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM [dbo].[NomeTable] WHERE [Nome] IN (SELECT [Name] FROM inserted))
    BEGIN
        RAISERROR('O Nome já existe na tabela NomeTable.', 16, 1);
        RETURN;
    END

    INSERT INTO [dbo].[NomeTable] ([Nome])
    SELECT DISTINCT i.[Name]
    FROM inserted i
    WHERE NOT EXISTS (SELECT 1 FROM [dbo].[NomeTable] n WHERE n.[Nome] = i.[Name]);

    INSERT INTO [dbo].[Record] ([Name], [Content], [CreationDatetime], [Parent])
    SELECT i.[Name], i.[Content], i.[CreationDatetime], i.[Parent]
    FROM inserted i;
END;
GO
CREATE TRIGGER trg_DeleteNomeFromNomeTable_Record
ON [dbo].[Record]
INSTEAD OF DELETE
AS
BEGIN
    DELETE FROM [dbo].[Record]
    WHERE [Id] IN (SELECT [Id] FROM deleted);
    DELETE FROM [dbo].[NomeTable]
    WHERE [Nome] IN (SELECT [Name] FROM deleted);
END;
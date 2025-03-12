CREATE TABLE [dbo].[Notification] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (50)  NOT NULL,
    [Endpoint]         NVARCHAR (100) NOT NULL,
    [CreationDatetime] DATETIME       NOT NULL,
    [Parent]           INT            NULL,
    [Event]            INT            NOT NULL,
    [Enabled]          BIT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Name] ASC),
    CONSTRAINT [FK_Notification_NomeTable] FOREIGN KEY ([Name]) REFERENCES [dbo].[NomeTable] ([Nome]),
    CONSTRAINT [FK_Notification_Container] FOREIGN KEY ([Parent]) REFERENCES [dbo].[Container] ([Id])
);


GO
CREATE TRIGGER trg_InsertNomeIntoNomeTable_Notification
ON [dbo].[Notification]
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

    INSERT INTO [dbo].[Notification] ([Name], [Endpoint], [CreationDatetime], [Parent], [Event], [Enabled])
    SELECT i.[Name], i.[Endpoint], i.[CreationDatetime], i.[Parent], i.[Event], i.[Enabled]
    FROM inserted i;
END;
GO
CREATE TRIGGER trg_DeleteNomeFromNomeTable_Notification
ON [dbo].[Notification]
INSTEAD OF DELETE
AS
BEGIN
    DELETE FROM [dbo].[Notification]
    WHERE [Id] IN (SELECT [Id] FROM deleted);
    DELETE FROM [dbo].[NomeTable]
    WHERE [Nome] IN (SELECT [Name] FROM deleted);
END;
IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240322105414_change_data3')
BEGIN
    CREATE TABLE [ScanLog] (
        [Id] int NOT NULL IDENTITY,
        [Line] nvarchar(max) NOT NULL,
        [MaterialNumber] nvarchar(max) NOT NULL,
        [ComponentNumber] nvarchar(max) NOT NULL,
        [Status] nvarchar(max) NULL,
        [ScanDate] date NOT NULL,
        [ScanTime] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_ScanLog] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240322105414_change_data3')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240322105414_change_data3', N'8.0.0-preview.1.23111.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240322105846_change_value')
BEGIN
    CREATE TABLE [ScanLog] (
        [Id] int NOT NULL IDENTITY,
        [Line] nvarchar(max) NOT NULL,
        [MaterialNumber] nvarchar(max) NOT NULL,
        [ComponentNumber] nvarchar(max) NOT NULL,
        [Status] nvarchar(max) NULL,
        [ScanDate] date NOT NULL,
        [ScanTime] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_ScanLog] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240322105846_change_value')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240322105846_change_value', N'8.0.0-preview.1.23111.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240402015259_added_to_scannerDTO')
BEGIN
    ALTER TABLE [scanner] ADD [batchNo] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240402015259_added_to_scannerDTO')
BEGIN
    ALTER TABLE [scanner] ADD [prodDate] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240402015259_added_to_scannerDTO')
BEGIN
    ALTER TABLE [scanner] ADD [supplier] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240402015259_added_to_scannerDTO')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240402015259_added_to_scannerDTO', N'8.0.0-preview.1.23111.4');
END;
GO

COMMIT;
GO


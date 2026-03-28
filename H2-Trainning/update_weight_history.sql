BEGIN TRANSACTION;
CREATE TABLE [WeightHistoryLogs] (
    [Id] int NOT NULL IDENTITY,
    [ClientId] nvarchar(450) NOT NULL,
    [ExerciseId] int NOT NULL,
    [Weight] float NOT NULL,
    [LoggedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_WeightHistoryLogs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_WeightHistoryLogs_AspNetUsers_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_WeightHistoryLogs_Exercises_ExerciseId] FOREIGN KEY ([ExerciseId]) REFERENCES [Exercises] ([Id])
);

CREATE INDEX [IX_WeightHistoryLogs_ClientId] ON [WeightHistoryLogs] ([ClientId]);

CREATE INDEX [IX_WeightHistoryLogs_ExerciseId] ON [WeightHistoryLogs] ([ExerciseId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260328115833_AddWeightHistoryLog', N'10.0.3');

COMMIT;
GO


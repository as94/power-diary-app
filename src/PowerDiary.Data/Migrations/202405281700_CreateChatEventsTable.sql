CREATE TABLE ChatEvents (
    EventId BINARY(16) NOT NULL PRIMARY KEY,
    UserId BINARY(16) NOT NULL,
    RoomId BINARY(16) NOT NULL,
    EventType VARCHAR(50) NOT NULL,
    CreatedAt DATETIME NOT NULL,
    CreatedAtUtc DATETIME NOT NULL,
    EventData JSON NOT NULL
);

CREATE INDEX idx_ChatEvents_CreatedAtUtc_EventType ON ChatEvents (CreatedAtUtc, EventType);
CREATE INDEX idx_ChatEvents_CreatedAt_EventType ON ChatEvents (CreatedAt, EventType);
CREATE TABLE IF NOT EXISTS AuditLogs
(
    Id int auto_increment,
    Timestamp DATETIME not null,
    UserId int not null,
    PartnerId int not null,
    PartnerName nvarchar(256) null,
    Action nvarchar(100) null,
    Entity nvarchar(100) null,
    Status int null,
    ErrorCode int null,
    ErrorMessage nvarchar(2048) null,
    TraceId varchar(128) null,
    Created DATETIME NOT NULL,
    Updated DATETIME NOT NULL,
    constraint AuditLogs_pk primary key (Id),
    constraint AuditLogs_Users_Id_fk foreign key (UserId) references Users (Id)
);

INSERT INTO AuditLogs (Timestamp, UserId, PartnerId, PartnerName, Action, Entity, Status, ErrorCode, ErrorMessage, TraceId, Created, Updated)
VALUES
('2021-06-23 00:00:00', 1, 1, 'IASO', 'Post', 'Partner', 1, null, null, 'some_trace_id', '2021-06-17', '2021-06-17'),
('2021-06-23 00:00:00', 2, 2, 'MASO', 'Update', 'Partner', 0, null, null, 'some_trace_id', '2021-06-17', '2021-06-17'),
('2021-06-23 00:00:00', 1, 3, 'KASO', 'Delete', 'Partner', 2, null, null, 'some_trace_id', '2021-06-17', '2021-06-17');

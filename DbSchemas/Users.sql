CREATE TABLE IF NOT EXISTS Users
(
	Id INT NOT NULL AUTO_INCREMENT,
	FirstName NVARCHAR(256) NOT NULL,
	LastName NVARCHAR(256) NULL,
	Email NVARCHAR(256) NOT NULL,
	Birthday DATE NULL,
	CONSTRAINT Users_pk PRIMARY KEY (Id)
);

CREATE UNIQUE INDEX Users_Email_uindex
ON Users (Email);

INSERT INTO AuditLog.Users (FirstName, LastName, Email, Birthday)
VALUES
('Amal-Sher', 'Kurbanov', 'amalsher.kurbanov@solarwinds.com', '1995-04-27'),
('Vasya', 'Pupkin', 'vasya.pupkin@example.com', '1996-02-29');

create table Users
(
	Id int auto_increment,
	FirstName nvarchar(256) not null,
	LastName nvarchar(256) null,
	Email nvarchar(256) not null,
	Birthday date null,
	constraint Users_pk
		primary key (Id)
);

create unique index Users_Email_uindex
	on Users (Email);


INSERT INTO AuditLog.Users (FirstName, LastName, Email, Birthday)
VALUES
('Amal-Sher', 'Kurbanov', 'amalsher.kurbanov@solarwinds.com', '1995-04-27'),
('Vasya', 'Pupkin', 'vasya.pupkin@example.com', '1996-02-29');

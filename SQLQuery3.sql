USE CafeDB

INSERT INTO Users (Username, Password, Role)
VALUES ('admin', 'admin123', 'Admin');

INSERT INTO Users (Username, Password, Role)
VALUES ('cashier1', '123', 'Cashier');

Select * FROM Users


Delete from Users
where UserId=6
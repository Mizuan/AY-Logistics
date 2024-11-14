use AYLogistics

select * from HouseBL

ALTER TABLE HouseBL
ADD BLmovedBy int NULL
GO

ALTER TABLE HouseBL
ADD BLmovedDate datetime NULL
GO

ALTER TABLE HouseBL
ADD PrevShipmentId int NULL
GO
use AYLogistics

select * from ContainerInfo

GO
ALTER TABLE ContainerInfo
ADD CWeight decimal(8,2) NULL
GO

GO
ALTER TABLE ContainerInfo
ADD CMeasure decimal(8,2) NULL
GO
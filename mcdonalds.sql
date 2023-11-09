use master 
go

if(exists(select * from sys.databases where name = 'McDataBase'))
	drop database McDataBase
go

create database McDataBase
go

use database McDataBase
go

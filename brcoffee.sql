use master
drop database brcoffee
go

create database brcoffee
go

use brcoffee
go

create table customer
(
	id int identity primary key,
	fullname nvarchar(50) not null,
	username varchar(50) unique,
	password varchar(50) not null,
	email varchar(100) unique,
	address nvarchar(200),
	phonenumber varchar(50),
	borndate datetime
)

create table category
(
	id int identity primary key,
	name nvarchar(50) not null,
)

create table drink
(
	id int identity primary key,
	name nvarchar(100) not null,
	idcategory int not null,
	picture varchar(50),
	describe nvarchar(max),
	price float not null default 0,
	foreign key (idcategory) references dbo.category(id)
)

create table bill
(
	id int identity primary key,
	payment bit,
	status bit,
	date datetime default getdate(),
	idcustomer int not null,
	foreign key (idcustomer) references dbo.customer(id)
)

create table billinfo
(
	id int identity primary key,
	idbill int not null,
	iddrink int not null,
	count int not null default 0,
	price decimal check (price >= 0) default 0
	foreign key (idbill) references dbo.bill(id),
	foreign key (iddrink) references dbo.drink(id)
)

alter table dbo.drink add date datetime default getdate()
go

insert dbo.category (name) values (N'Coffee')
insert dbo.category (name) values (N'Fruit Juice')
insert dbo.category (name) values (N'Soft Drink')

insert dbo.Drink (name, idcategory, price) values (N'Black Coffee', 1, 13000)
insert dbo.Drink (name, idcategory, price) values (N'Milk Coffee', 1, 15000)
insert dbo.Drink (name, idcategory, price) values (N'Cappuccino', 1, 25000)
insert dbo.Drink (name, idcategory, price) values (N'Latte', 1, 25000)

insert dbo.Drink (name, idcategory, price) values (N'Lemon', 2, 15000)
insert dbo.Drink (name, idcategory, price) values (N'Orange', 2, 20000)
insert dbo.Drink (name, idcategory, price) values (N'Strawberry', 2, 20000)
insert dbo.Drink (name, idcategory, price) values (N'Coconut', 2, 20000)

insert dbo.Drink (name, idcategory, price) values (N'Aquafina', 3, 10000)
insert dbo.Drink (name, idcategory, price) values (N'Coca Cola', 3, 15000)
insert dbo.Drink (name, idcategory, price) values (N'Pepsi', 3, 15000)

insert dbo.Drink (name, idcategory, price) values (N'Espresso', 1, 15000)
go

create table news 
(
	id int identity primary key,
	title nvarchar(100),
	author nvarchar(50),
	source varchar(50),
	subtitle nvarchar(100),
	content1 nvarchar(max),
	picture1 varchar(50),
	content2 nvarchar(max),
	picture2 varchar(50)
)
go

select * from news

alter table news add topic nvarchar(50)
alter table news add content3 varchar(max)
alter table news add picture3 varchar(50)
go

create table account
(
	username nvarchar(100) primary key,
	displayname nvarchar(100) not null ,
	password varchar(100) not null,
	type int not null default 0 -- 1: administrator, 0: staff
)
go

insert dbo.account values (N'admin', N'Administrator', ' ', 1)
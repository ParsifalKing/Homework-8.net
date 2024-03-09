create table Customers
{
    CustomerId serial primary key,
	CustomerName varchar(150),
	Email varchar(200),
	Address varchar(200)
};

create table Products
(
    ProductId serila primary key,
	ProductName varchar(200),
	Price decimal,
	StockQuantity 
);

create table Orders
(
    OrderId serial primary key,
	CustomerId double,
	OrderDate date,
	TotalAmount decimal
);

create table OrderDateils
(
    OrderDetailId serial primary key,
	OrderId int references Orders(OrderId),
	ProducctId int references Product(Product),
	Quantity decimal,
	UnitPride decimal
);